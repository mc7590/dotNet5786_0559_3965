using BlApi;
using PL.Courier;
using PL.Helpers;
using PL.Login;
using PL.Order;
using System.Data.Common;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL;


/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{    
    public static MainWindow? Instance { get; private set; }
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    readonly int UserId;

    public MainWindow(int thisUserId)
    {
        UserId=thisUserId;
        InitializeComponent();
        Instance = this;
        this.Closed += (s, e) => Instance = null;
    }

    /// <summary>
    /// property dependency to bind the current time to the UI
    /// </summary>
    public DateTime CurrentTime
    {
        get { return (DateTime)GetValue(CurrentTimeProperty); }
        set { SetValue(CurrentTimeProperty, value); }
    }
    public static readonly DependencyProperty CurrentTimeProperty =
        DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(s_bl.Admin.GetClock()));


    private void BtnAddOneMinute_Click(object sender, RoutedEventArgs e)
    {
        s_bl.Admin.ForwardClock(BO.EnumTimeUnit.Minute);
    }

    private void BtnAddOneHour_Click(object sender, RoutedEventArgs e)
    {
        s_bl.Admin.ForwardClock(BO.EnumTimeUnit.Hour);
    }

    private void BtnAddOneDay_Click(object sender, RoutedEventArgs e)
    {
        s_bl.Admin.ForwardClock(BO.EnumTimeUnit.Day);
    }

    private void BtnAddOneMonth_Click(object sender, RoutedEventArgs e)
    {
        s_bl.Admin.ForwardClock(BO.EnumTimeUnit.Month);
    }

    private void BtnAddOneYear_Click(object sender, RoutedEventArgs e)
    {
        s_bl.Admin.ForwardClock(BO.EnumTimeUnit.Year);
    }



    /// <summary>
    /// property dependency to bind the Configuration to the UI
    /// </summary>
    public BO.Config Configuration
    {
        get { return (BO.Config)GetValue(ConfigurationProperty); }
        set { SetValue(ConfigurationProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Configuration.
    public static readonly DependencyProperty ConfigurationProperty =
        DependencyProperty.Register("Configuration", typeof(BO.Config), typeof(MainWindow), new PropertyMetadata(s_bl.Admin.GetConfig()));

    private void SaveConfigurationCommand(object sender, RoutedEventArgs e)
    {
        try
        {
            s_bl.Admin.SetConfig(Configuration);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving configuration: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }


    private readonly ObserverMutex _clockMutex = new(); //stage 7

    private void ClockObserver()
    {
        #region Stage 7 (for multithreading)
        if (_clockMutex.CheckAndSetLoadInProgressOrRestartRequired())
            return;

        Dispatcher.BeginInvoke(async () =>
        {
            // The actual work to be done on the UI thread
            CurrentTime = s_bl.Admin.GetClock(); //stage 5

            // After completing the work, check if a restart was requested
            if (await _clockMutex.UnsetLoadInProgressAndCheckRestartRequested())
                ClockObserver();
        });
        #endregion Stage 7 (for multithreading)

    }


    private readonly ObserverMutex _configMutex = new(); //stage 7

    private void ConfigObserver()
    {
        #region Stage 7 (for multithreading)
        if (_configMutex.CheckAndSetLoadInProgressOrRestartRequired())
            return;

        Dispatcher.BeginInvoke(async () =>
        {
            // The actual work to be done on the UI thread
            Configuration = s_bl.Admin.GetConfig(); //stage 5

            // After completing the work, check if a restart was requested
            if (await _configMutex.UnsetLoadInProgressAndCheckRestartRequested())
                ConfigObserver();
        });
        #endregion Stage 7 (for multithreading)
    }

    /// <summary>
    /// Perform the following actions when the screen loads
    /// </summary>
    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        //Initialize CurrentTime Dependency Property
        CurrentTime = s_bl.Admin.GetClock();

        //Initialize Config Dependency Property
        Configuration = s_bl.Admin.GetConfig();

        //Register the clockObserver method with the BL's observer mechanism
        s_bl.Admin.AddClockObserver(ClockObserver);

        //Register the configObserver method with the BL's observer mechanism
        s_bl.Admin.AddConfigObserver(ConfigObserver);
    }

    /// <summary>
    /// Perform the following actions when the screen closes
    /// </summary>
    private void MainWindow_Closed(object sender, EventArgs e)
    {
        //Remove the observers from the BL's observer mechanism
        s_bl.Admin.RemoveClockObserver(ClockObserver);
        s_bl.Admin.RemoveConfigObserver(ConfigObserver);
    }

    /// <summary>
    /// Opens the Orders List window
    /// </summary>
    private void BtnOrders_Click(object sender, RoutedEventArgs e)
    {
        Mouse.OverrideCursor = Cursors.Wait;
        new OrderListWindow(UserId).Show();            
        //Restore the mouse
        Mouse.OverrideCursor = null;
    }

    /// <summary>
    /// Opens the Couriers List window
    /// </summary>
    private void BtnCouriers_Click(object sender, RoutedEventArgs e)
    {
        Mouse.OverrideCursor = Cursors.Wait;
        new CourierListWindow(UserId).Show();
        //Restore the mouse
        Mouse.OverrideCursor = null;
    }

    /// <summary>
    /// Resets the database to its initial state
    /// </summary>
    private void BtnResetDB_Click(object sender, RoutedEventArgs e)
    {

        //Confirmation Message Box
        MessageBoxResult result = MessageBox.Show(
            "Are you sure you want to Reset the database? All existing data will be lost.",
            "Database Reset Confirmation",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            // Set mouse to Wait
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                // Close all open windows (except the main window)
                CloseOtherWindows();

                //Call function
                s_bl.Admin.ResetDB();

                MessageBox.Show("Database successfully Reset!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Reset failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Restore the mouse
                Mouse.OverrideCursor = null;
            }
        }
    }


    /// <summary>
    /// Initializes the database to its initial state
    /// </summary>
    private void BtnInitializeDB_Click(object sender, RoutedEventArgs e)
    {

        //Confirmation Message Box
        MessageBoxResult result = MessageBox.Show(
            "Are you sure you want to initialize the database? All existing data will be lost.",
            "Database Initialization Confirmation",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            //Set mouse to Wait
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                //Close all open windows (except the main window)
                CloseOtherWindows();

                //Call function
                s_bl.Admin.InitializeDB();

                MessageBox.Show("Database successfully initialized!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Initialization failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                //Restore the mouse
                Mouse.OverrideCursor = null;
            }
        }
    }

    /// <summary>
    /// Helps to close all open windows except the current window
    /// </summary>
    private void CloseOtherWindows()
    {
        // Applications.Current.Windows holds a collection of all currently open Window objects.
        foreach (Window w in Application.Current.Windows)
        {
            // Check if the current window is not the current MainWindow instance.
            if (w != this)
            {
                w.Close();
            }
        }
    }



    public int Interval
    {
        get { return (int)GetValue(IntervalProperty); }
        set { SetValue(IntervalProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Interval.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IntervalProperty =
        DependencyProperty.Register("Interval", typeof(int), typeof(MainWindow), new PropertyMetadata(1440));


    public bool IsSimulationRunning
    {
        get { return (bool)GetValue(IsSimulationRunningProperty); }
        set { SetValue(IsSimulationRunningProperty, value); }
    }

    public static readonly DependencyProperty IsSimulationRunningProperty =
        DependencyProperty.Register("IsSimulationRunning", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

    private void BtnSimulation_Click(object sender, RoutedEventArgs e)
    {
        if (!IsSimulationRunning)
        {
            IsSimulationRunning = true;

            //Activate simulation
            s_bl.Admin.StartSimulator(Interval); //stage 7
        }
        else // diactivate simulation
        {
            IsSimulationRunning = false;
            s_bl.Admin.StopSimulator(); //stage 7
        }
    }
}