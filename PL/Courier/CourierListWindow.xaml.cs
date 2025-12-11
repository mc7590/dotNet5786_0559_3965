using PL.Order;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace PL.Courier;

/// <summary>
/// Interaction logic for CourierListWindow.xaml
/// </summary>
public partial class CourierListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public CourierListWindow()
    {
        InitializeComponent();
    }


    public IEnumerable<BO.CourierInList> CourierList
    {
        get { return (IEnumerable<BO.CourierInList>)GetValue(CourierListProperty); }
        set { SetValue(CourierListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CourierList.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CourierListProperty =
        DependencyProperty.Register("CourierList", typeof(IEnumerable<BO.CourierInList>), typeof(CourierListWindow), new PropertyMetadata(null));

    public BO.EnumDeliveryMethod MethodDelivery { get; set; } = BO.EnumDeliveryMethod.None;
    /// <summary>
    /// Filter the list when the selection in the ComboBox changes
    /// </summary>
    private void FilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) => RefreshCourierList();
    
    /// <summary>
    /// Refresh the courier list according to the selected filter
    /// </summary>
    private void RefreshCourierList()    
    {
        int managerId = s_bl?.Admin.GetConfig().ManagerId ?? 0;
        if (MethodDelivery == BO.EnumDeliveryMethod.None)
        {
            CourierList = s_bl?.Courier.GetCouriersInList(managerId)!;
        }
        else
        {
            CourierList = s_bl?.Courier.GetCouriersInList(managerId, null, null, BO.EnumCourierFieldFilter.DeliveryMethod, MethodDelivery)!;
        }
    }

    /// <summary>
    /// List observer to refresh the courier list when there are changes
    /// </summary>
    private void courierListObserver()
    => RefreshCourierList();


    /// <summary>
    /// Add the observer when the window is loaded
    /// </summary>
    private void Window_Loaded(object sender, RoutedEventArgs e) => s_bl.Courier.AddObserver(courierListObserver);

    /// <summary>
    /// Remove the observer when the window is closed
    /// </summary>
    private void Window_Closed(object sender, EventArgs e) => s_bl.Courier.RemoveObserver(courierListObserver);

    public object? SelectedCourier { get; set; }

    /// <summary>
    /// Take care of double click on a DataGrid row
    /// dg = data grid
    /// </summary>
    private void dgCourierList_MouseDoubleClick(object sender, MouseButtonEventArgs e) //public?
    {
        //check if the double click was not on an empty area in the DataGrid
        if (sender is DataGrid dg && dg.SelectedItem is BO.CourierInList selected)
        {
            BO.Courier fullCourier = s_bl.Courier.Read(selected.Id, selected.Id)!;

            CourierWindow win = new CourierWindow(fullCourier.Id);
            win.ShowDialog();

            RefreshCourierList();
        }
    }

    private void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        CourierWindow win = new CourierWindow(0);  
        win.ShowDialog();
        RefreshCourierList();
    }
}
