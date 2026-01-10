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

namespace PL.Delivery;


/// <summary>
/// Interaction logic for DeliveryHistory.xaml
/// </summary>
public partial class DeliveryHistory : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    readonly int UserId;
    readonly int CourierId;

    /// <summary>
    /// Ctor
    /// </summary>
    public DeliveryHistory(int thisUserId, int currentCourierId)
    {
        UserId = thisUserId;
        CourierId = currentCourierId;

        InitializeComponent();
        DataContext = this;
    }

    /// <summary>
    /// Dependency property for the DeliveryHistory list
    /// </summary>
    public IEnumerable<BO.ClosedDeliveryInList> DeliveryHistoryList
    {
        get => (IEnumerable<BO.ClosedDeliveryInList>?)GetValue(CurrentDeliveryHistoryProperty)!;
        set => SetValue(CurrentDeliveryHistoryProperty, value);
    }
    public static readonly DependencyProperty CurrentDeliveryHistoryProperty =
        DependencyProperty.Register(nameof(DeliveryHistoryList), typeof(IEnumerable<BO.ClosedDeliveryInList>), typeof(DeliveryHistory), new PropertyMetadata(null));

    /// <summary>
    /// Property for the list filter
    /// </summary>
    //public BO.EnumOrderType? OrderTypeFilter { get; set; } = BO.EnumOrderType.None;
    public BO.EnumOrderType? OrderTypeFilter
    {
        get => (BO.EnumOrderType?)GetValue(OrderTypeFilterProperty);
        set => SetValue(OrderTypeFilterProperty, value);
    }

    public static readonly DependencyProperty OrderTypeFilterProperty =
        DependencyProperty.Register("OrderTypeFilter", typeof(BO.EnumOrderType?), typeof(DeliveryHistory),
            new PropertyMetadata(BO.EnumOrderType.None));

    /// <summary>
    /// Refresh the Delivery History list
    /// also known as query
    /// </summary>
    private void RefreshDeliveryHistoryList()
    {
        //if added SORT functionality, do it here

        if (OrderTypeFilter == BO.EnumOrderType.None) //no filter
        {
            DeliveryHistoryList = s_bl.Order.GetClosedDeliveriesInListsToCourier(UserId, CourierId);
        }
        else // with filter // (OrderTypeFilter != BO.EnumOrderType.None)
        {
            DeliveryHistoryList = s_bl.Order.GetClosedDeliveriesInListsToCourier(UserId, CourierId, OrderTypeFilter);
        }
    }

    private void DeliveryHistoryObserver()
    {
        RefreshDeliveryHistoryList();
    }


    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        s_bl.Order.AddObserver(DeliveryHistoryObserver);
        RefreshDeliveryHistoryList();
    }
    private void Window_Closed(object sender, EventArgs e)
    {
        s_bl.Order.RemoveObserver(DeliveryHistoryObserver);
    }

    private void BtnClearFilter_Click(object sender, RoutedEventArgs e)
    {
        OrderTypeFilter = BO.EnumOrderType.None;
        DeliveryHistoryObserver();
    }

    private void CmbStatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        DeliveryHistoryObserver();

        //// the user chose to clear the filter
        //if (CmbStatusFilter.SelectedItem == null)
        //{
        //    HistoryGrid.ItemsSource = allDeliveries;
        //    return;
        //}

        //// select the status to filter by
        //BO.EnumEndDeliveryStatus selectedStatus = (BO.EnumEndDeliveryStatus)CmbStatusFilter.SelectedItem;

        //// filter the list based on the selected status
        //var filteredList = allDeliveries.Where(d => d.EndDeliveryStatus == selectedStatus).ToList();

        //// update the DataGrid with the filtered list
        //HistoryGrid.ItemsSource = filteredList;
    }
}

