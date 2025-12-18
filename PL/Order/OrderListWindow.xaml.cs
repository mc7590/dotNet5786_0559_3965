using BO;
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
    static int ManagerId => s_bl.Admin.GetConfig().ManagerId;

    public OrderListWindow()
    {
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

    public BO.EnumOrderStatus OrderStatus { get; set; } = BO.EnumOrderStatus.None;

    /// <summary>
    /// Filter the list when the selection in the ComboBox changes
    /// </summary>
    private void FilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) => RefreshOrderList();

    /// <summary>
    /// Refresh the order list according to the selected filter
    /// </summary>
    private void RefreshOrderList()
    {
        if (OrderStatus == BO.EnumOrderStatus.None)
        {
            OrderList = s_bl?.Order.GetOrderInList(ManagerId)!;
        }
        else
        {
            OrderList = s_bl?.Order.GetOrderInList(ManagerId, BO.EnumOrderFieldSort.OrderStatus, OrderStatus)!;
        }
    }

    /// <summary>
    /// List observer to refresh the order list when there are changes
    /// </summary>
    private void orderListObserver()
    => RefreshOrderList();


    /// <summary>
    /// Add the observer when the window is loaded
    /// </summary>
    private void Window_Loaded(object sender, RoutedEventArgs e) => s_bl.Order.AddObserver(orderListObserver);

    /// <summary>
    /// Remove the observer when the window is closed
    /// </summary>
    private void Window_Closed(object sender, EventArgs e) => s_bl.Order.RemoveObserver(orderListObserver);

    public object? SelectedOrder { get; set; }

    /// <summary>
    /// Take care of double click on a DataGrid row
    /// dg = data grid
    /// </summary>
    private void dgOrderList_MouseDoubleClick(object sender, MouseButtonEventArgs e) //public?
    {
        //check if the double click was not on an empty area in the DataGrid
        if (sender is DataGrid dg && dg.SelectedItem is BO.OrderInList selected)
        {
            BO.Order fullOrder = s_bl.Order.Read(ManagerId, selected.OrderId)!;

            OrderWindow win = new OrderWindow(fullOrder.Id);
            win.ShowDialog();

            RefreshOrderList();
        }
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
}
