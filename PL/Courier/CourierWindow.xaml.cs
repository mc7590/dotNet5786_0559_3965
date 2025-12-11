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

    private void CourierObserver()
    {
        Dispatcher.Invoke(() =>
        {
            int id = CurrentCourier.Id;
            try
            {
                CurrentCourier = s_bl.Courier.Read(id,id)!;
            }
            catch (Exception)
            {
                this.Close();
            }
        });
    }

    public static readonly DependencyProperty CurrentCourierProperty =
        DependencyProperty.Register("CurrentCourier", typeof(BO.Courier), typeof(CourierWindow), new PropertyMetadata(null));
    public CourierWindow(int id = 0)
    {
        // Set button text before InitializeComponent
        ButtonText = id == 0 ? "Add" : "Update";

        //InitializeComponent();
        InitializeComponent();
        try
        {
            // Load a new Courier or an existing one
            if (id == 0)
            {
                CurrentCourier = new BO.Courier
                {
                    Id = 0,
                    StartedWorking = DateTime.Now
                };
            }
            else
            {
                CurrentCourier = BlApi.Factory.Get().Courier.Read(id, id)!;
            }

        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading courier: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            CurrentCourier = new BO.Courier { Id = 0, StartedWorking = DateTime.Now };
        }
        // Bind all XAML elements to this object (Window)
        //DataContext = this;
    }

    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        var bl = BlApi.Factory.Get();
        int id = s_bl?.Admin.GetConfig().ManagerId ?? 0;
        // Add or Update logic
        try
        {
            if (ButtonText == "Add")
                bl.Courier.Create(id, CurrentCourier);
            else
                bl.Courier.Update(id, CurrentCourier);

            MessageBox.Show("Saved successfully.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving courier: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (CurrentCourier != null && CurrentCourier.Id != 0)
        {
            s_bl.Courier.AddObserver(CurrentCourier.Id, CourierObserver);
        }
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        if (CurrentCourier != null && CurrentCourier.Id != 0)
        {
            s_bl.Courier.RemoveObserver(CurrentCourier.Id, CourierObserver);
        }
    }
}
