using BO;
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
        int managerId = s_bl?.Admin.GetConfig().ManagerId ?? 0;
        if (OrderStatus == BO.EnumOrderStatus.None)
        {
            OrderList = s_bl?.Order.GetOrderInList(managerId)!;
        }
        else
        {
            OrderList = s_bl?.Order.GetOrderInList(managerId, BO.EnumOrderFieldSort.OrderStatus, OrderStatus)!;
        }
    }

    /// <summary>
    /// Refresh the order list when the window is loaded
    /// </summary>
    private void Window_Loaded(object sender, RoutedEventArgs e) => RefreshOrderList();
  
}
