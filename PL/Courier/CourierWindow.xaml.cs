using Helpers;
using PL.Delivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PL.Courier;
/// <summary>
/// Interaction logic for CourierWindow.xaml
/// </summary>
public partial class CourierWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    readonly int UserId;

    public CourierWindow(int thisUserId, int id = 0)
    {
        UserId = thisUserId;

        // Set button text before InitializeComponent
        ButtonText = id == 0 ? "Add" : "Update";

        InitializeComponent();
        try
        {
            // Load a new Courier or an existing one
            if (id == 0)
            {
                CurrentCourier = new BO.Courier
                {
                    Id = 0,
                    StartedWorking = s_bl.Admin.GetClock()
                };
            }
            else
            {
                CurrentCourier = BlApi.Factory.Get().Courier.Read(UserId, id)!;
            }

        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading courier: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            CurrentCourier = new BO.Courier { Id = 0, StartedWorking = DateTime.Now };
        }
        // Bind all XAML elements to this object (Window)
        DataContext = this;
    }

    // DependencyProperty for the Add/Update button text
    public static readonly DependencyProperty ButtonTextProperty =
        DependencyProperty.Register("ButtonText", typeof(string),
            typeof(CourierWindow), new PropertyMetadata(""));
    public string ButtonText
    {
        get => (string)GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }

    // Enum list for the ComboBox (DeliveryMethod)
    public IEnumerable<BO.EnumDeliveryMethod> EnumDeliveryMethods =>
        Enum.GetValues(typeof(BO.EnumDeliveryMethod)).Cast<BO.EnumDeliveryMethod>();

    // The current Courier displayed on the screen
    public BO.Courier CurrentCourier
    {
        get { return (BO.Courier?)GetValue(CurrentCourierProperty)!; }
        set { SetValue(CurrentCourierProperty, value); }
    }
    public static readonly DependencyProperty CurrentCourierProperty =
    DependencyProperty.Register("CurrentCourier", typeof(BO.Courier), typeof(CourierWindow), new PropertyMetadata(null));

    private void CourierObserver()
    {

            
        int id = CurrentCourier.Id;
            
        try           
        {   
            CurrentCourier = s_bl.Courier.Read(UserId, id)!;
        }
        catch (Exception)
        {
             this.Close();
        }

    }

    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        var bl = BlApi.Factory.Get();
        // Add or Update logic
        try
        {
            if (ButtonText == "Add")
                bl.Courier.Create(UserId, CurrentCourier);
            else
                bl.Courier.Update(UserId, CurrentCourier);

            MessageBox.Show("Saved successfully.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving courier: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // History Visibility Dependency Property
    public static readonly DependencyProperty HistoryVisibilityProperty =
    DependencyProperty.Register("HistoryVisibility", typeof(Visibility),
        typeof(CourierWindow), new PropertyMetadata(Visibility.Collapsed));

    public Visibility HistoryVisibility
    {
        get => (Visibility)GetValue(HistoryVisibilityProperty);
        set => SetValue(HistoryVisibilityProperty, value);
    }


    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (CurrentCourier != null && CurrentCourier.Id != 0)
        {
            s_bl.Courier.AddObserver(CurrentCourier.Id, CourierObserver);
            s_bl.Order.AddObserver(CourierObserver);
        }
        if (UserId == s_bl.Admin.GetConfig().ManagerId) //the user is manager
            HistoryVisibility = Visibility.Visible;

    }

    private void Window_Closed(object sender, EventArgs e)
    {
        if (CurrentCourier != null && CurrentCourier.Id != 0)
        {
            s_bl.Courier.RemoveObserver(CurrentCourier.Id, CourierObserver);
            s_bl.Order.RemoveObserver(CourierObserver);
        }
    }

    private void BtnSelectOrder_Click(object sender, RoutedEventArgs e)
    {
        new Order.SelectOrderWindow(UserId, CurrentCourier.Id).Show();
    }

    private void BtnDeliveriesHistory_Click(object sender, RoutedEventArgs e)
    {
        new Delivery.DeliveryHistory(UserId, CurrentCourier.Id).Show();
    }

    public BO.EnumEndDeliveryStatus SelectedEndDeliveryStatus { get; set; } = BO.EnumEndDeliveryStatus.Unknown;

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            s_bl.Order.EndOrderStatus(UserId, CurrentCourier.Id, CurrentCourier.ActiveDeliveryOrder!.DeliveryId, SelectedEndDeliveryStatus);
            MessageBox.Show("Delivery completed successfully.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error completing delivery: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

}
