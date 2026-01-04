using PL.Courier;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
namespace PL.Login;

/// <summary>
/// Interaction logic for LoginWindow.xaml
/// </summary>
public partial class LoginWindow : Window, INotifyPropertyChanged // Implement INotifyPropertyChanged
{
    private static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public LoginWindow()
    {
        InitializeComponent();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private string _idText = "";
    // trigger when user types inside the ID box
    public string IdText
    {
        get => _idText;
        set
        {
            _idText = value;
            OnPropertyChanged(nameof(IdText));
            OnPropertyChanged(nameof(CanLogin)); 
        }
    }
    // Determine if the login button can be enabled
    public bool CanLogin
    {
        // Enable login only if both ID and Password are provided
        get => !string.IsNullOrWhiteSpace(IdText) &&
               !string.IsNullOrWhiteSpace(txtPassword.Password); // Access PasswordBox directly
    }
    // the trigger when user types inside the Password box the access to the Password box is different because it's a PasswordBox
    private void Password_Changed(object sender, RoutedEventArgs e)
    {
        OnPropertyChanged(nameof(CanLogin));
    }
    private void BtnLogin_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(txtId.Text, out int userId))
        {
            MessageBox.Show("Invalid ID format.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        string password = txtPassword.Password;
        try
        {
            int managerId = s_bl.Admin.GetConfig().ManagerId;

            // MANAGER LOGIN
            if (userId == managerId)
            {
                if (password != s_bl.Admin.GetConfig().ManagerPassword)
                {
                    MessageBox.Show("Incorrect password for Manager.", "Login Error");
                    return;
                }

//check if the manager is ALSO a courier
//UNNEDED COMPLEXITY
                //BO.Courier? courier = null;
                //try
                //{
                //    courier = s_bl.Courier.Read(userId, userId);
                //}
                //catch (Exception)
                //{
                //    // Courier not found, proceed as manager only
                //}
                //if (courier != null)
                //{
                //    MessageBoxResult result = MessageBox.Show(
                //        "Hello Manager!\nYou are registered also as a courier.\nDo you want to go to the Admin Panel?\nYes – Admin Panel\nNo – Courier Panel",
                //        "Manager Courier Login",
                //        MessageBoxButton.YesNo,
                //        MessageBoxImage.Question);

                //    if (result == MessageBoxResult.Yes)
                //    {
                //        // Check if MainWindow is already open  
                //        if (MainWindow.Instance != null)
                //        {
                //            MessageBox.Show("Admin Panel is already open.", "Info");
                //            MainWindow.Instance.Activate();
                //            return;
                //        }
                //        // Open Main Window
                //        new MainWindow(userId).Show();
                //    }
                //    else
                //    {
                //        var newCourierWindow = new Courier.CourierWindow(userId, userId);
                //        newCourierWindow.Show();
                //    }

                //}
                //else //manager
                {
                    // Check if MainWindow is already open  
                    if (MainWindow.Instance != null)
                    {
                        MessageBox.Show("Admin Panel is already open.", "Info");
                        MainWindow.Instance.Activate(); //Bring "MainWindow" to Front
                        return;
                    }
                    // Open Main Window
                    MessageBox.Show("Welcome, Manager!", "Login Successful");
                    new MainWindow(userId).Show();
                }
                return;
            }
            // COURIER LOGIN
            BO.Courier? courierUser = s_bl.Courier.Read(userId, userId);
            if (courierUser!.Password != password)
            {
                MessageBox.Show("Incorrect password for Courier.", "Login Error");
                return;
            }
            MessageBox.Show($"Welcome, {courierUser.Name}!", "Login Successful");
            var courierWindow = new Courier.CourierWindow(userId, userId);
            courierWindow.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Login failed: {ex.Message}", "Login Error");
        }
    }

}
