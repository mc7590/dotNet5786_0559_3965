using PL.Courier;
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

namespace PL.Order;

/// <summary>
/// Interaction logic for OrderListWindow.xaml
/// </summary>
public partial class OrderListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    readonly int UserId;

    /// <summary>
    /// constructor
    /// </summary>
    public OrderListWindow(int thisUserId)
    {
        UserId = thisUserId;
        InitializeComponent();
    }

    public IEnumerable<BO.OrderInList> OrderList
    {
        get { return (IEnumerable<BO.OrderInList>)GetValue(OrderListProperty); }
        set { SetValue(OrderListProperty, value); }
    }
    // Using a DependencyProperty as the backing store for OrderList.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OrderListProperty =
        DependencyProperty.Register("OrderList", typeof(IEnumerable<BO.OrderInList>), typeof(OrderListWindow), new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets the current order filter
    /// </summary>
    public BO.EnumOrderStatus OrderStatus { get; set; } = BO.EnumOrderStatus.None;

    /// <summary>
    /// Gets or sets the current order sort type
    /// </summary>
    public BO.EnumOrderFieldSort OrderSort { get; set; } = BO.EnumOrderFieldSort.None;

    /// <summary>
    /// function to handle selection change in the combo box
    /// </summary>
    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RefreshOrderList();
    }

    /// <summary>
    /// Refresh the order list according to the selected filter
    /// </summary>
    private void RefreshOrderList()
    {
        //if (OrderStatus == BO.EnumOrderStatus.None && OrderSort == BO.EnumOrderFieldSort.None)
        //{
        //    OrderList = s_bl?.Order.GetOrderInList(UserId)!;
        //}
        //else if(OrderStatus != BO.EnumOrderStatus.None && OrderSort == BO.EnumOrderFieldSort.None)
        //{
        //    OrderList = s_bl?.Order.GetOrderInList(UserId, BO.EnumOrderFieldFilter.OrderStatus, OrderStatus)!;
        //}
        //else if (OrderStatus == BO.EnumOrderStatus.None && OrderSort != BO.EnumOrderFieldSort.None)
        //{
        //    OrderList = s_bl?.Order.GetOrderInList(UserId, null, null, BO.EnumOrderFieldSort.OrderType, OrderSort)!;
        //}
        //else // both filter + sort are set
        //{
        //    OrderList = s_bl?.Order.GetOrderInList(UserId, BO.EnumOrderFieldFilter.OrderStatus, OrderStatus, BO.EnumOrderFieldSort.OrderType, OrderSort)!;
        //}
        if (OrderStatus == BO.EnumOrderStatus.None && OrderSort == BO.EnumOrderFieldSort.None)
        {
            OrderList = s_bl?.Order.GetOrderInList(UserId)!;
        }
        else if (OrderStatus != BO.EnumOrderStatus.None && OrderSort == BO.EnumOrderFieldSort.None)
        {
            OrderList = s_bl?.Order.GetOrderInList(UserId, BO.EnumOrderFieldFilter.OrderStatus, OrderStatus)!;
        }
        else if (OrderStatus == BO.EnumOrderStatus.None && OrderSort != BO.EnumOrderFieldSort.None)
        {
            OrderList = s_bl?.Order.GetOrderInList(UserId, null, null, OrderSort, OrderSort)!;
        }
        else // both filter + sort are set
        {
            OrderList = s_bl?.Order.GetOrderInList(UserId, BO.EnumOrderFieldFilter.OrderStatus, OrderStatus, OrderSort, OrderSort)!;
        }
    }

    /// <summary>
    /// List observer to refresh the order list when there are changes
    /// </summary>
    private void orderListObserver() => RefreshOrderList();

    /// <summary>
    /// Add the observer when the window is loaded
    /// </summary>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        s_bl.Order.AddObserver(orderListObserver);
    }

    /// <summary>
    /// Remove the observer when the window is closed
    /// </summary>
    private void Window_Closed(object sender, EventArgs e) 
        => s_bl.Order.RemoveObserver(orderListObserver);

    public BO.OrderInList? SelectedOrder
    {
        get => (BO.OrderInList?)GetValue(SelectedOrderProperty);
        set => SetValue(SelectedOrderProperty, value);
    }

    public static readonly DependencyProperty SelectedOrderProperty =
        DependencyProperty.Register(
            nameof(SelectedOrder),
            typeof(BO.OrderInList),
            typeof(OrderListWindow),
            new PropertyMetadata(null)
        );

    /// <summary>
    /// Take care of double click on a DataGrid row
    /// dg = data grid
    /// </summary>
    private void dgOrderList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (SelectedOrder == null)
            return;

        BO.Order fullOrder =
            s_bl.Order.Read(UserId, SelectedOrder.OrderId)!;

        new OrderWindow(UserId, fullOrder.Id).Show();
    }

    /// <summary>
    /// Add a new order
    /// </summary>
    private void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        OrderWindow window = new OrderWindow(UserId, 0);
        window.Show();
        RefreshOrderList();
    }

    private void DeleteOrder_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedOrder == null)
            return;
        try
        {
            s_bl.Order.Delete(UserId, SelectedOrder.OrderId);
            MessageBox.Show("Order deleted successfully");
            RefreshOrderList();
        }
        catch
        {
            MessageBox.Show("Failed to delete order");
        }
    }


}
