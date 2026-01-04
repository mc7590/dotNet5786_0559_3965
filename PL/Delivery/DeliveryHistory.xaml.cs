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
    int courierId;
    private IEnumerable<BO.ClosedDeliveryInList> allDeliveries;
    public DeliveryHistory(int thisUserId, int currentCourierId)
    {
        InitializeComponent();
        courierId = currentCourierId;

        allDeliveries = s_bl.Order.GetClosedDeliveriesInListsToCourier(thisUserId, currentCourierId);
        HistoryGrid.ItemsSource = allDeliveries;
    }
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
    }

    private void Window_Closed(object sender, EventArgs e)
    {

    }

    private void BtnClearFilter_Click(object sender, RoutedEventArgs e)
    {
        CmbStatusFilter.SelectedItem = null;
        HistoryGrid.ItemsSource = allDeliveries;
    }

    private void CmbStatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

        // the user chose to clear the filter
        if (CmbStatusFilter.SelectedItem == null)
        {
            HistoryGrid.ItemsSource = allDeliveries;
            return;
        }

        // select the status to filter by
        BO.EnumEndDeliveryStatus selectedStatus = (BO.EnumEndDeliveryStatus)CmbStatusFilter.SelectedItem;

        // filter the list based on the selected status
        var filteredList = allDeliveries.Where(d => d.EndDeliveryStatus == selectedStatus).ToList();

        // update the DataGrid with the filtered list
        HistoryGrid.ItemsSource = filteredList;
    }
}

