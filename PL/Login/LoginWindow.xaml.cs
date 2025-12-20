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
using System.ComponentModel;
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
        DataContext = this; // Set DataContext for data binding
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private string _idText = "";
    // the trigger when user types inside the ID box
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
        //check inputs validity
        if (!int.TryParse(txtId.Text, out int userId))  
        {
            MessageBox.Show("Invalid ID format. Please enter a numeric ID.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        string password = txtPassword.Password;
        try {
            int managerId = s_bl.Admin.GetConfig().ManagerId;

            if (userId == managerId)
            {
                // Validate Manager Password
                if (password != s_bl.Admin.GetConfig().ManagerPassword)
                {
                    MessageBox.Show("Incorrect password for Manager.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Confirmation Message Box
                MessageBoxResult result = MessageBox.Show("Hello Manager! Do you want to proceed to the Admin Panel?", "Manager Login", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {                
                    // Open Main Window
                    new MainWindow().Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Login cancelled. You can enter a different ID or close the application.", "Login Cancelled", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                // Check if the courier exists
                BO.Courier? courier = s_bl.Courier.Read(userId, userId);
                if (courier == null)
                {
                    MessageBox.Show("Courier not found.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                // Validate Courier Password
                if (courier!.Password != password)
                {
                    MessageBox.Show("Incorrect password for Courier.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                // Open Courier Window
                new Courier.CourierWindow(userId).Show();
                this.Close();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Login failed: {ex.Message}", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

}
