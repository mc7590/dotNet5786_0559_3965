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

namespace PL.Order
{
    /// <summary>
    /// Interaction logic for OrderInProgressWindow.xaml
    /// </summary>
    public partial class OrderInProgressWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        int CourierId;
        BO.OrderInProgress? order;
        public OrderInProgressWindow(int courierId, int orderId)
        {
            BO.OrderInProgress? orderInProgress = s_bl.Courier?.Read(courierId, courierId)!.ActiveDeliveryOrder;
            InitializeComponent();
            this.DataContext = orderInProgress;
            order = orderInProgress;
            CourierId = courierId;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            s_bl.Order.EndOrderStatus(CourierId, CourierId, order!.DeliveryId);
            Close();
        }
    }
}
