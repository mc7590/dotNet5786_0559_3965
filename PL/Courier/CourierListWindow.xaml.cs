using PL.Order;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    static int ManagerId => s_bl.Admin.GetConfig().ManagerId;

    /// <summary>
    /// Constructor
    /// </summary>
    public CourierListWindow()
    {
        InitializeComponent();
    }
    /// <summary>
    /// Dependency property for the courier list
    /// </summary>
    public ObservableCollection<BO.CourierInList> CourierList
    {
        get => (ObservableCollection<BO.CourierInList>)GetValue(CourierListProperty);
        set => SetValue(CourierListProperty, value);
    }

    public static readonly DependencyProperty CourierListProperty =
        DependencyProperty.Register(
            nameof(CourierList),
            typeof(ObservableCollection<BO.CourierInList>),
            typeof(CourierListWindow),
            new PropertyMetadata(null));

    /// <summary>
    /// Selected delivery method filter
    /// </summary>
    public BO.EnumDeliveryMethod MethodDelivery
    {
        get => (BO.EnumDeliveryMethod)GetValue(MethodDeliveryProperty);
        set => SetValue(MethodDeliveryProperty, value);
    }

    public static readonly DependencyProperty MethodDeliveryProperty =
        DependencyProperty.Register(
            nameof(MethodDelivery),
            typeof(BO.EnumDeliveryMethod),
            typeof(CourierListWindow),
            new PropertyMetadata(BO.EnumDeliveryMethod.None, OnMethodDeliveryChanged));

    private static void OnMethodDeliveryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {         
        if (d is CourierListWindow win)
            win.RefreshCourierList();
    }

    /// <summary>
    /// The selected courier in the DataGrid
    /// </summary>
    public BO.CourierInList? SelectedCourier
    {
        get => (BO.CourierInList?)GetValue(SelectedCourierProperty);
        set => SetValue(SelectedCourierProperty, value);
    }

    public static readonly DependencyProperty SelectedCourierProperty =
        DependencyProperty.Register(
            nameof(SelectedCourier),
            typeof(BO.CourierInList),
            typeof(CourierListWindow),
            new PropertyMetadata(null)
        );

    /// <summary>
    /// Refresh the courier list according to the selected filter
    /// </summary>
    private void RefreshCourierList()
    {


        try
        {
            IEnumerable<BO.CourierInList>? list =
                MethodDelivery == BO.EnumDeliveryMethod.None
                ? s_bl.Courier.GetCouriersInList(ManagerId)
                : s_bl.Courier.GetCouriersInList(
                    ManagerId,
                    null,
                    null,
                    BO.EnumCourierFieldFilter.DeliveryMethod,
                    MethodDelivery);

            CourierList.Clear();
            if (list == null)
                return;

            foreach (var courier in list)
                CourierList.Add(courier);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error loading data: {ex.Message}",
                "Data Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }


    /// <summary>
    /// List observer to refresh the courier list when there are changes
    /// </summary>
    private void courierListObserver()
    {
        RefreshCourierList();
    }

    /// <summary>
    /// Add the observer when the window is loaded
    /// </summary>
    private void Window_Loaded(object sender, RoutedEventArgs e) 
    {
        try
        {
            CourierList = new ObservableCollection<BO.CourierInList>();
            RefreshCourierList();
            s_bl.Courier.AddObserver(courierListObserver);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Initialization error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Remove the observer when the window is closed
    /// </summary>
    private void Window_Closed(object sender, EventArgs e) => s_bl.Courier.RemoveObserver(courierListObserver);


    /// <summary>
    /// Take care of double click on a DataGrid row
    /// dg = data grid
    /// </summary>
    private void dgCourierList_MouseDoubleClick(object sender, MouseButtonEventArgs e) //public?
    {
        //check if the double click was not on an empty area in the DataGrid
        if (SelectedCourier == null)
            return;
        new CourierWindow(SelectedCourier.Id).ShowDialog();
    }

    /// <summary>
    /// Add a new courier
    /// </summary>
    private void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        new CourierWindow(0).ShowDialog();
    }

    /// <summary>
    /// Delete the selected courier
    /// </summary>
    private void DeleteCourier_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedCourier == null)
            return;

        MessageBoxResult result = MessageBox.Show(
            $"Are you sure you want to delete courier {SelectedCourier.Name}?", "Delete Confirmation",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            try
            {
                s_bl.Courier.Delete(ManagerId, SelectedCourier.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not delete courier. Reason: {ex.Message}", "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
