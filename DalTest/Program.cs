using Dal;
using DalApi;
namespace DalTest;
using DO;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

internal class Program
{
    private static ICourier s_dalCourier = new CourierImplementation(); //stage 1
    private static IOrder? s_dalOrder = new OrderImplementation(); //stage 1
    private static IDelivery? s_dalDelivery = new DeliveryImplementation(); //stage 1
    private static IConfig? s_dalConfig = new ConfigImplementation(); //stage 1

    static void Main(string[] args)
    {
        try
        {
            MainMenu();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }


    }
    private static void MainMenu()
    {
        bool exit = false;

        while (!exit)
        {
            try
            {
                Console.WriteLine("\n=== MAIN MENU ===");
                Console.WriteLine("1. Manage Couriers");
                Console.WriteLine("2. Manage Orders");
                Console.WriteLine("3. Manage Deliveries");
                Console.WriteLine("4. Manage Config");
                Console.WriteLine("5. Initialize Data (call Initialization.Do)");
                Console.WriteLine("6. Reset all data");
                Console.WriteLine("0. Exit");
                Console.Write("Choose: ");

                if (!Enum.TryParse(Console.ReadLine(), out MainMenuOption choice))
                {
                    Console.WriteLine("Invalid choice!");
                    continue;
                }

                switch (choice)
                {
                    case MainMenuOption.ManageCouriers:
                        CourierMenu();
                        break;

                    case MainMenuOption.ManageOrders:
                        OrderMenu();
                        break;

                    case MainMenuOption.ManageDeliveries:
                        DeliveryMenu();
                        break;

                    case MainMenuOption.ManageConfig:
                        ConfigMenu();
                        break;

                    case MainMenuOption.InitializeData:
                        Initialization.Do(s_dalCourier, s_dalOrder, s_dalDelivery, s_dalConfig);
                        Console.WriteLine("Data initialized successfully!");
                        break;

                    case MainMenuOption.ResetAll:
                        ResetAll();
                        break;

                    case MainMenuOption.Exit:
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option!");
                        break;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }




    /// <summary>
    /// MainMenu -> CourierMenu 
    /// unfinished!!!
    /// </summary>
    private static void CourierMenu()
    {
        bool back = false;
        while (!back)
        {
            Console.WriteLine("\n--- Couriers Menu ---");
            Console.WriteLine("1. Add Courier");
            Console.WriteLine("2. Show Courier by ID");
            Console.WriteLine("3. Show All Couriers");
            Console.WriteLine("4. Update Courier");
            Console.WriteLine("5. Delete Courier");
            Console.WriteLine("6. Delete All Couriers");
            Console.WriteLine("0. Back");
            Console.Write("Choose: ");

            if (!Enum.TryParse(Console.ReadLine(), out CourierMenuOption choice))
            {
                Console.WriteLine("Invalid choice!");
                continue;
            }
            switch (choice)
            {
                case CourierMenuOption.AddCourier:
                    {                    
                     // Add Courier logic here
                        Console.Write("Enter ID: ");
                        int idC = int.Parse(Console.ReadLine()!);
                        Console.Write("Enter Name: ");
                        string name = Console.ReadLine()!;
                        Console.Write("Enter Phone: ");
                        string phone = Console.ReadLine()!;
                        Console.Write("Enter Email: ");
                        string email = Console.ReadLine()!;
                        Console.Write("Enter Password: ");
                        string password = Console.ReadLine()!;
                        Console.Write("Delivery method (Car, Motorcycle, Bicycle, Foot): ");
                        Enum.TryParse(Console.ReadLine(), true, out EnumDeliveryMethod method);
                        Console.Write("Enter Max Personal Distance (optional): ");
                        string? input = Console.ReadLine();
                        double? maxDist = null;
                        if (double.TryParse(input, out double d))
                            maxDist = d;
                        Courier newCourier = new Courier(    
                            Id: idC,   
                            Name: name,   
                            CourierPhone: phone,    
                            Email: email,
                            Password: password,   
                            Active: true,                   // Default value 
                            DeliveryMethod: method,    
                            StartedWorking: DateTime.Now,   // Default value   
                            MaxPersonalDistance: maxDist
                        );
                        s_dalCourier.Create(newCourier);
                        Console.WriteLine("Courier added successfully!");
                    }
                    break;
                case CourierMenuOption.GetCourier:
                    {
                        // Get Courier by ID logic here
                        Console.Write("Enter ID: ");
                        int id = int.Parse(Console.ReadLine()!);
                        s_dalCourier.Read(id);
                    }
                    break;
                case CourierMenuOption.GetAllCouriers:
                    {
                        // Get All Couriers logic here
                        var couriers = s_dalCourier.ReadAll();
                        foreach (var courier in couriers)
                        {
                            Console.WriteLine(courier);
                        }
                    }
                    break;
                case CourierMenuOption.UpdateCourier:
                    {
                        // Update Courier logic here
                        Console.Write("Enter ID of courier to update: ");
                        if (!int.TryParse(Console.ReadLine(), out int id))
                        {
                            Console.WriteLine("Invalid ID format.");
                            break;
                        }
                        Courier? existing = s_dalCourier.Read(id);
                        if (existing == null)
                        {
                            Console.WriteLine("Courier with this ID does not exist.");
                            break;
                        }

                        Console.WriteLine("Enter new values (leave empty to keep current value):");

                        Console.Write($"Name ({existing.Name}): ");
                        string? name = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(name)) name = existing.Name;

                        Console.Write($"Phone ({existing.CourierPhone}): ");
                        string? phone = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(phone)) phone = existing.CourierPhone;

                        Console.Write($"Email ({existing.Email}): ");
                        string? email = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(email)) email = existing.Email;

                        Console.Write($"Password ({existing.Password}): ");
                        string? password = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(password)) password = existing.Password;

                        Console.Write($"Delivery method ({existing.DeliveryMethod}): ");
                        string? methodInput = Console.ReadLine();
                        EnumDeliveryMethod method = existing.DeliveryMethod;
                        if (!string.IsNullOrWhiteSpace(methodInput))
                            Enum.TryParse(methodInput, true, out method);

                        Console.Write($"Max Personal Distance ({existing.MaxPersonalDistance}): ");
                        string? maxDistInput = Console.ReadLine();
                        double? maxDist = existing.MaxPersonalDistance;
                        if (!string.IsNullOrWhiteSpace(maxDistInput) && double.TryParse(maxDistInput, out double d))
                            maxDist = d;

                        // יצירת אובייקט חדש עם הערכים המעודכנים
                        Courier updatedCourier = new Courier(
                            Id: existing.Id,
                            Name: name,
                            CourierPhone: phone,
                            Email: email,
                            Password: password,
                            Active: existing.Active,               // נשאר כמו שהיה
                            DeliveryMethod: method,
                            StartedWorking: existing.StartedWorking, // נשאר כמו שהיה
                            MaxPersonalDistance: maxDist
                        );

                        s_dalCourier.Update(updatedCourier);
                        Console.WriteLine("Courier updated successfully!");
                    }
                        break;
                case CourierMenuOption.DeleteCourier:
                    {
                        // Delete Courier logic here
                        Console.Write("Enter ID: ");
                        int delId = int.Parse(Console.ReadLine()!);
                        s_dalCourier.Delete(delId);
                    }
                    break;
                case CourierMenuOption.Exit:
                    back = true;
                    break;
                default:
                    Console.WriteLine("Invalid option!");
                    break;
            }


        }
    }

    /// <summary>
    /// MainMenu -> DeliveryMenu
    /// unfinished!!!
    /// </summary>
    private static void OrderMenu()
    {
        bool back = false;
        while (!back)
        {
            Console.WriteLine("\n--- Orders Menu ---");
            Console.WriteLine("1. Add Order");
            Console.WriteLine("2. Show Order by ID");
            Console.WriteLine("3. Show All Orders");
            Console.WriteLine("4. Update Order");
            Console.WriteLine("5. Delete Order");
            Console.WriteLine("6. Delete All Orders");
            Console.WriteLine("0. Back");
            Console.Write("Choose: ");
            if (!Enum.TryParse(Console.ReadLine(), out OrderMenuOption choice))
            {
                Console.WriteLine("Invalid choice!");
                continue;
            }
            switch (choice)
            {
                case OrderMenuOption.AddOrder:
                    // Add Order logic here
                    break;
                case OrderMenuOption.GetOrder:
                    // Get Order by ID logic here
                    break;
                case OrderMenuOption.GetAllOrders:
                    // Get All Orders logic here
                    break;
                case OrderMenuOption.UpdateOrder:
                    // Update Order logic here
                    break;
                case OrderMenuOption.DeleteOrder:
                    // Delete Order logic here
                    break;
                case OrderMenuOption.Exit:
                    back = true;
                    break;
                default:
                    Console.WriteLine("Invalid option!");
                    break;
            }
        }
    }

    /// <summary>
    /// MainMenu -> DeliveryMenu
    /// unfinished!!!
    /// </summary>
    private static void DeliveryMenu()
    {
        bool back = false;
        while (!back)
        {
            Console.WriteLine("\n--- Deliveries Menu ---");
            Console.WriteLine("1. Add Delivery");
            Console.WriteLine("2. Show Delivery by ID");
            Console.WriteLine("3. Show All Deliveries");
            Console.WriteLine("4. Update Delivery");
            Console.WriteLine("5. Delete Delivery");
            Console.WriteLine("6. Delete All Deliveries");
            Console.WriteLine("0. Back");
            Console.Write("Choose: ");
            if (!Enum.TryParse(Console.ReadLine(), out DeliveryMenuOption choice))
            {
                Console.WriteLine("Invalid choice!");
                continue;
            }
            switch (choice)
            {
                case DeliveryMenuOption.AddDelivery:
                    // Add Delivery logic here
                    break;
                case DeliveryMenuOption.GetDelivery:
                    // Get Delivery by ID logic here
                    break;
                case DeliveryMenuOption.GetAllDeliveries:
                    // Get All Deliveries logic here
                    break;
                case DeliveryMenuOption.UpdateDelivery:
                    // Update Delivery logic here
                    break;
                case DeliveryMenuOption.DeleteDelivery:
                    // Delete Delivery logic here
                    break;
                case DeliveryMenuOption.Exit:
                    back = true;
                    break;
                default:
                    Console.WriteLine("Invalid option!");
                    break;
            }
        }
    }

    /// <summary>
    /// MainMenu -> ConfigMenu
    /// </summary>
    private static void ConfigMenu()
    {
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\n--- CONFIG MENU ---");
            Console.WriteLine("1. Advance clock by one minute");
            Console.WriteLine("2. Advance clock by one hour");
            Console.WriteLine("3. Advance clock by one day");
            Console.WriteLine("4. Show current clock value");
            Console.WriteLine("5. Set new value for a config parameter");
            Console.WriteLine("6. Show current value of a config variable");
            Console.WriteLine("7. Reset all config values");
            Console.WriteLine("0. Exit submenu");
            Console.Write("Choose an option: ");

            if (!Enum.TryParse(Console.ReadLine(), out ConfigMenuOption choice))
            {
                Console.WriteLine("Invalid choice!");
                continue;
            }
            switch (choice)
            {
                case ConfigMenuOption.Add1MinToClock:
                    s_dalConfig!.Clock = s_dalConfig.Clock.AddMinutes(1);
                    break;
                case ConfigMenuOption.Add1HourToClock:
                    s_dalConfig!.Clock = s_dalConfig.Clock.AddHours(1);
                    break;
                case ConfigMenuOption.Add1DayToClock:
                    s_dalConfig!.Clock = s_dalConfig.Clock.AddDays(1);
                    break;
                case ConfigMenuOption.ShowCurrentClock:
                    Console.WriteLine(s_dalConfig!.Clock);
                    break;
                case ConfigMenuOption.SetConfigParameters:
                    SetConfigParameters();
                    break;
                case ConfigMenuOption.GetConfigParameters:
                    GetConfigParameters();
                    break;
                case ConfigMenuOption.ResetConfigToDefault:
                    s_dalConfig!.Reset();
                    break;
                case ConfigMenuOption.Exit:
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option!");
                    break;

            }
        }
    }

    /// <summary>
    /// Reset all data and config
    /// </summary>
    /// <exception cref="Exception">in case DAL is not initialized yet</exception> 
    private static void ResetAll()
    {
        if (s_dalCourier == null || s_dalOrder == null || s_dalDelivery == null || s_dalConfig == null)
        {
            throw new Exception("Error: DAL not initialized yet!");
        }
        s_dalCourier.DeleteAll(); //stage 1
        s_dalOrder.DeleteAll(); //stage 1
        s_dalDelivery.DeleteAll(); //stage 1                
        s_dalConfig.Reset(); //stage 1
        Console.WriteLine("All data and config reset successfully!");

    }    

    /// <summary>
    /// Set new value for a config parameter
    /// </summary>
    private static void SetConfigParameters()
    {
        bool back = false;
        while (!back)
        {
            Console.WriteLine("\n--- Set Config Parameters ---");
            Console.WriteLine("1. Set clock");
            Console.WriteLine("2. Set Company Address");
            Console.WriteLine("3. Set Latitude");
            Console.WriteLine("4. Set Longitude");
            Console.WriteLine("0. Back");
            Console.Write("Choose an option: ");

            if (!Enum.TryParse(Console.ReadLine(), out SetConfigParametersOption choice))
            {
                Console.WriteLine("Invalid choice!");
                continue;
            }
            switch (choice)
            {
                case SetConfigParametersOption.SetClock:
                    s_dalConfig!.Clock = DateTime.Now;
                    break;
                case SetConfigParametersOption.SetCompanyAddress:
                    {
                        Console.WriteLine("Enter new address: <street>, <building-number>, <city>");
                        string? newAddress = Console.ReadLine();
                        s_dalConfig!.CompanyAddress = newAddress;
                        break;
                    }
                case SetConfigParametersOption.SetLatitude:
                    {
                        Console.WriteLine("Enter new latitude");
                        string? input = Console.ReadLine();
                        double newLat;
                        if (double.TryParse(input, out newLat))
                            s_dalConfig!.Latitude = newLat;
                        else
                        {
                            Console.WriteLine("Error: Invalid latitude format.");
                            s_dalConfig!.Latitude = 0.0;
                        }
                        break;
                    }
                case SetConfigParametersOption.SetLongitude:
                    {
                        Console.WriteLine("Enter new longitude");
                        string? input = Console.ReadLine();
                        double newLon;
                        if (double.TryParse(input, out newLon))
                            s_dalConfig!.Longitude = newLon;
                        else
                        {
                            Console.WriteLine("Error: Invalid longitude format.");
                            s_dalConfig!.Longitude = 0.0;
                        }
                        break;
                    }
                case SetConfigParametersOption.Back:
                    back = true;
                    break;

            }
        }

    }

    /// <summary>
    /// Get current value of a config parameter
    /// </summary>
    private static void GetConfigParameters()
    {
        bool back = false;
        while (!back)
        {
            Console.WriteLine("\n--- Get Config Parameters ---");
            Console.WriteLine("1. Get clock");
            Console.WriteLine("2. Get Company Address");
            Console.WriteLine("3. Get Latitude");
            Console.WriteLine("4. Get Longitude");
            Console.WriteLine("5. Get Max Delivery Distance");
            Console.WriteLine("6. Get Max Delivery Time");
            Console.WriteLine("7. Get Risk Range");
            Console.WriteLine("8. Get Inactivity Threshold");
            Console.WriteLine("0. Back");
            Console.Write("Choose an option: ");

            if (!Enum.TryParse(Console.ReadLine(), out GetConfigParametersOption choice))
            {
                Console.WriteLine("Invalid choice!");
                continue;
            }
            switch (choice)
            {
                case GetConfigParametersOption.GetClock:
                    Console.WriteLine(s_dalConfig!.Clock);
                    break;
                case GetConfigParametersOption.GetCompanyAddress:
                    Console.WriteLine(s_dalConfig!.CompanyAddress);
                    break;
                case GetConfigParametersOption.GetLatitude:
                    Console.WriteLine(s_dalConfig!.Latitude);
                    break;
                case GetConfigParametersOption.GetLongitude:
                    Console.WriteLine(s_dalConfig!.Longitude);
                    break;
                case GetConfigParametersOption.GetMaxDeliveryDistance:
                    Console.WriteLine(s_dalConfig!.MaxDeliveryDistance);
                    break;
                case GetConfigParametersOption.GetMaxDeliveryTime:
                    Console.WriteLine(s_dalConfig!.GetMaxDeliveryTime);
                    break;
                case GetConfigParametersOption.GetRiskRange:
                    Console.WriteLine(s_dalConfig!.RiskRange);
                    break;
                case GetConfigParametersOption.GetInactivityThreshold:
                    Console.WriteLine(s_dalConfig!.InactivityThreshold);
                    break;
                case GetConfigParametersOption.Back:
                    back = true;
                    break;
            }
        }
    }

}

