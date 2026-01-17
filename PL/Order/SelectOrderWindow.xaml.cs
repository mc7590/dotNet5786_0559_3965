using BO;
using DO;
using PL.Courier;
using PL.Delivery;
using PL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Order;

/// <summary>
/// Interaction logic for SelectOrderWindow.xaml
/// </summary>
public partial class SelectOrderWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    readonly int UserId;
    readonly int CourierId;
    public SelectOrderWindow(int userId, int courierId)
    {
        UserId = userId;
        CourierId = courierId;        
        InitializeComponent();             
    }
    /// <summary>
    /// Dependency property for the OpenOrderInList list
    /// </summary>
    public IEnumerable<BO.OpenOrderInList> OpenOrderInList
    {
        get { return (IEnumerable<BO.OpenOrderInList>?)GetValue(CurrentOpenOrderInListProperty)!; }
        set { SetValue(CurrentOpenOrderInListProperty, value); }
    }
    public static readonly DependencyProperty CurrentOpenOrderInListProperty =
        DependencyProperty.Register("CurrentOpenOrderInList", typeof(IEnumerable<BO.OpenOrderInList>), typeof(SelectOrderWindow), new PropertyMetadata(null));


    private readonly ObserverMutex _openOrderListmutex = new(); //stage 7

    private async Task RefreshOpenOrderList()
    {
        //#region Stage 7 (for multithreading)
        //if (_openOrderListmutex.CheckAndSetLoadInProgressOrRestartRequired())
        //    return;

        //Dispatcher.BeginInvoke(async () =>
        //{
        //    OpenOrderInList = await s_bl.Order.GetListOfOpenOrderToChoose(UserId, CourierId);

        //    // Check if a restart was requested while we were working
        //    if (await _openOrderListmutex.UnsetLoadInProgressAndCheckRestartRequested())
        //        RefreshOpenOrderList();
        //});
        //#endregion Stage 7 (for multithreading)

        #region Stage 7 (for multithreading)
        if (_openOrderListmutex.CheckAndSetLoadInProgressOrRestartRequired())
            return;

        // מפעילים Task שירוץ ברקע
        _ = Task.Run(async () =>
        {
            //heavy work out of dispacher
            var result = await s_bl.Order.GetListOfOpenOrderToChoose(UserId, CourierId);

            _ = Dispatcher.BeginInvoke(async () =>
            {
                OpenOrderInList = result; //the actual work

                //Check if a restart was requested while we were working
                if (await _openOrderListmutex.UnsetLoadInProgressAndCheckRestartRequested())
                    _ = RefreshOpenOrderList();
            });
        });
        #endregion Stage 7 (for multithreading)
    }

    private async void OpenOrderListObserver()
        => await RefreshOpenOrderList();
    private void Window_Loaded(object sender, RoutedEventArgs e)
        => s_bl.Courier.AddObserver(OpenOrderListObserver);
    private void Window_Closed(object sender, EventArgs e)
        => s_bl.Courier.RemoveObserver(OpenOrderListObserver);


    public BO.OpenOrderInList? SelectedOrder
    {
        get => (BO.OpenOrderInList?)GetValue(SelectedOrderProperty);
        set => SetValue(SelectedOrderProperty, value);
    }
    public static readonly DependencyProperty SelectedOrderProperty =
        DependencyProperty.Register(
            nameof(SelectedOrderProperty),
            typeof(BO.OpenOrderInList),
            typeof(SelectOrderWindow),
            new PropertyMetadata(null)
        );

    //until result come isEnable=false so there will be no excess click
    private async void BtnSelectOrderRow_Click(object sender, RoutedEventArgs e)
    {

        try
        {
            if (SelectedOrder != null)
            {

                await s_bl.Courier.AssignOrderToCourier(CourierId, SelectedOrder.OrderId);
                MessageBox.Show("Order assigned successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                // Close the CourierWindow to refresh its data
                Application.Current.Windows.OfType<CourierWindow>().FirstOrDefault()?.Close();
            }
        }
        catch (Exception ex) 
        { 
            MessageBox.Show($"Error assigning order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
    }
}
