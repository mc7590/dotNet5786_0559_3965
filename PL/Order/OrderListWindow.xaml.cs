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

    //public BO.EnumOrderStatus OrderStatus { get; set; } = BO.EnumOrderStatus.None;

    public BO.EnumOrderStatus OrderStatus
    {
        get => (BO.EnumOrderStatus)GetValue(OrderStatusProperty);
        set => SetValue(OrderStatusProperty, value);
    }

    public static readonly DependencyProperty OrderStatusProperty =
        DependencyProperty.Register(
            nameof(OrderStatus),
            typeof(BO.EnumOrderStatus),
            typeof(OrderListWindow),
            new PropertyMetadata(BO.EnumOrderStatus.None, OnOrderStatusChanged)
        );
    private static void OnOrderStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is OrderListWindow win)
            win.RefreshOrderList();
    }
 
    /// <summary>
    /// Refresh the order list according to the selected filter
    /// </summary>
    private void RefreshOrderList()
    {
        if (OrderStatus == BO.EnumOrderStatus.None)
        {
            OrderList = s_bl?.Order.GetOrderInList(UserId)!;
        }
        else
        {
            OrderList = s_bl?.Order.GetOrderInList(UserId, BO.EnumOrderField.OrderStatus, OrderStatus)!;
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
        RefreshOrderList();
    }

    /// <summary>
    /// Remove the observer when the window is closed
    /// </summary>
    private void Window_Closed(object sender, EventArgs e) => s_bl.Order.RemoveObserver(orderListObserver);

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

        new OrderWindow(fullOrder.Id).ShowDialog();
    }

    /// <summary>
    /// Add a new order
    /// </summary>
    private void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        OrderWindow window = new OrderWindow(0);
        window.ShowDialog();
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
