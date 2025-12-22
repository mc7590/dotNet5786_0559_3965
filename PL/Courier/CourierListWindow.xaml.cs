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
    readonly int UserId;

    /// <summary>
    /// Constructor
    /// </summary>
    public CourierListWindow(int ThisUserID)
    {
        UserId = ThisUserID;
        InitializeComponent();

        this.Loaded += Window_Loaded;
        this.Closed += Window_Closed;
        
    }
    /// <summary>
    /// Dependency property for the courier list
    /// </summary>
    public IEnumerable<BO.CourierInList> CourierList
    {
        get => (IEnumerable<BO.CourierInList>)GetValue(CourierListProperty);
        set => SetValue(CourierListProperty, value);
    }

    public static readonly DependencyProperty CourierListProperty =
        DependencyProperty.Register(
            nameof(CourierList),
            typeof(IEnumerable<BO.CourierInList>),
            typeof(CourierListWindow),
            new PropertyMetadata(null));


    /// <summary>
    /// Property for the list filter
    /// </summary>
    public bool? Active { get; set; } = null;

    /// <summary>
    /// Property for the list filter
    /// </summary>
    public BO.EnumDeliveryMethod FilterByMethod { get; set; } = BO.EnumDeliveryMethod.None;

    /// <summary>
    /// Property for the list sort
    /// </summary>
    public BO.EnumCourierFieldSort SortByCourierFields { get; set; } = BO.EnumCourierFieldSort.Name;


    // NO USE!
    //private static void OnMethodDeliveryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    //{         
    //    if (d is CourierListWindow win)
    //        win.RefreshCourierList();
    //}

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
        CourierList = s_bl.Courier.GetCouriersInList(UserId, Active, SortByCourierFields);
    }


    /// <summary>
    /// List observer to refresh the courier list when there are changes
    /// </summary>
    private void courierListObserver()
        => RefreshCourierList();

    /// <summary>
    /// Add the observer when the window is loaded
    /// </summary>
    private void Window_Loaded(object sender, RoutedEventArgs e) 
        =>s_bl.Courier.AddObserver(courierListObserver);
    

    /// <summary>
    /// Remove the observer when the window is closed
    /// </summary>
    private void Window_Closed(object sender, EventArgs e) 
        => s_bl.Courier.RemoveObserver(courierListObserver);


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

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        CourierList = s_bl.Courier.GetCouriersInList(UserId, Active, SortByCourierFields, FilterByMethod);

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
                s_bl.Courier.Delete(UserId, SelectedCourier.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not delete courier. Reason: {ex.Message}", "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
