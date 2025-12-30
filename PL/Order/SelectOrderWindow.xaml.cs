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
    /// Interaction logic for SelectOrderWindow.xaml
    /// </summary>
    public partial class SelectOrderWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        readonly int UserId;
        IEnumerable<BO.OpenOrderInList> orders;
        public SelectOrderWindow(int userId)
        {
            InitializeComponent();
            UserId = userId;
            orders = s_bl.Order.GetListOfOpenOrderToChoose(userId, userId);
            dgOrders.ItemsSource = orders;
        }

        private void BtnSelectOrderRow_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
