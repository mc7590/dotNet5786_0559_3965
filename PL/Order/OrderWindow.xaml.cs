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
/// Interaction logic for OrderWindow.xaml
/// </summary>
public partial class OrderWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    // DependencyProperty for the Add/Update button text
    public static readonly DependencyProperty ButtonTextProperty =
        DependencyProperty.Register("ButtonText", typeof(string),
            typeof(OrderWindow), new PropertyMetadata(""));

    public string ButtonText
    {
        get => (string)GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }

    /// <summary>
    /// The current Order displayed on the screen
    /// </summary>
    public BO.Order CurrentOrder
    {
        get { return (BO.Order?)GetValue(CurrentOrderProperty)!; }
        set { SetValue(CurrentOrderProperty, value); }
    }
    public static readonly DependencyProperty CurrentOrderProperty =
    DependencyProperty.Register("CurrentOrder", typeof(BO.Order), typeof(OrderWindow), new PropertyMetadata(null));

    private void OrderObserver()
    {
        Dispatcher.Invoke(() =>
        {
            int id = CurrentOrder.Id;
            try
            {
                CurrentOrder = s_bl.Order.Read(id, id)!;
            }
            catch (Exception)
            {
                this.Close();
            }
        });
    }

    public OrderWindow(int orderId =0)
    {
        // Set button text before InitializeComponent
        ButtonText = orderId == 0 ? "Add" : "Update";

        //InitializeComponent();
        InitializeComponent();
        try
        {
            // Load a new Order or an existing one
            if (orderId == 0)
            {
                CurrentOrder = new BO.Order
                {
                    Id = 0,
                    CreationTime = s_bl.Admin.GetClock()
                };
            }
            else
            {
                CurrentOrder = BlApi.Factory.Get().Order.Read(s_bl.Admin.GetConfig().ManagerId, orderId)!; //gotten the manager id!
            }

        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            CurrentOrder = new BO.Order { Id = 0, CreationTime = s_bl.Admin.GetClock() };
        }
    }
    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        var bl = BlApi.Factory.Get();
        int id = s_bl?.Admin.GetConfig().ManagerId ?? 0;
        // Add or Update logic
        try
        {
            if (ButtonText == "Add")
                bl.Order.Create(id, CurrentOrder);
            else
                bl.Order.Update(id, CurrentOrder);

            MessageBox.Show("Saved successfully.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (CurrentOrder != null && CurrentOrder.Id != 0)
        {
            s_bl.Order.AddObserver(CurrentOrder.Id, OrderObserver);
        }
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        if (CurrentOrder != null && CurrentOrder.Id != 0)
        {
            s_bl.Order.RemoveObserver(CurrentOrder.Id, OrderObserver);
        }
    }
}
