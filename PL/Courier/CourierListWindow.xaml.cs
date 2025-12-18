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
    static int ManagerId => s_bl.Admin.GetConfig().ManagerId;

    /// <summary>
    /// Constructor
    /// </summary>
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
            new PropertyMetadata(BO.EnumDeliveryMethod.None, OnMethodDeliveryChanged)
        );
    private static void OnMethodDeliveryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {         
        if (d is CourierListWindow win)
            win.RefreshCourierList();
    }
    
    /// <summary>
    /// Refresh the courier list according to the selected filter
    /// </summary>
    private void RefreshCourierList()    
    {
        if (MethodDelivery == BO.EnumDeliveryMethod.None)
        {
            CourierList = s_bl?.Courier.GetCouriersInList(ManagerId)!;
        }
        else
        {
            CourierList = s_bl?.Courier.GetCouriersInList(ManagerId, null, null, BO.EnumCourierFieldFilter.DeliveryMethod, MethodDelivery)!;
        }
    }

    /// <summary>
    /// List observer to refresh the courier list when there are changes
    /// </summary>
    private void courierListObserver()  => RefreshCourierList();


    /// <summary>
    /// Add the observer when the window is loaded
    /// </summary>
    private void Window_Loaded(object sender, RoutedEventArgs e) 
    {
        s_bl.Courier.AddObserver(courierListObserver);
        RefreshCourierList();
    }

    /// <summary>
    /// Remove the observer when the window is closed
    /// </summary>
    private void Window_Closed(object sender, EventArgs e) => s_bl.Courier.RemoveObserver(courierListObserver);

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
    /// Take care of double click on a DataGrid row
    /// dg = data grid
    /// </summary>
    private void dgCourierList_MouseDoubleClick(object sender, MouseButtonEventArgs e) //public?
    {
        //check if the double click was not on an empty area in the DataGrid
        if (SelectedCourier == null)
            return;
        BO.Courier fullCourier = s_bl.Courier.Read(ManagerId, ((BO.CourierInList)SelectedCourier).Id)!;
        new CourierWindow(fullCourier.Id).ShowDialog();
    }

    /// <summary>
    /// Add a new courier
    /// </summary>
    private void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        CourierWindow win = new CourierWindow(0);  
        win.ShowDialog();
        RefreshCourierList();
    }

    /// <summary>
    /// Delete the selected courier
    /// </summary>
    private void DeleteCourier_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedCourier == null)
            return;
        try
        {
            s_bl.Courier.Delete(ManagerId, SelectedCourier.Id);
            MessageBox.Show("Courier deleted successfully");
        }
        catch
        {
            MessageBox.Show("Failed to delete courier");
        }
    }
}
