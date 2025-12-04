using System;
using System.Collections.Generic;
using Helpers;

namespace BlTest;

class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("\n=== BL TEST - Main Menu ===");
            Console.WriteLine("1. Admin");
            Console.WriteLine("2. Order");
            Console.WriteLine("3. Courier");
            Console.WriteLine("0. Exit");
            Console.Write("Choose option: ");

            string? choice = Console.ReadLine();
            if (choice == "0") break;
            try
            {
                switch (choice)
                {
                    case "1": AdminMenu(); break;
                    case "2": OrderMenu(); break;
                    case "3": CourierMenu(); break;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

            Console.WriteLine("Exiting BlTest.");
    }


    #region Admin Menu
    static void AdminMenu()
    {
        while (true)
        {
            Console.WriteLine("\n--- Admin Menu ---");
            Console.WriteLine("1. ResetDB");
            Console.WriteLine("2. InitializeDB");
            Console.WriteLine("3. ForwardClock");
            Console.WriteLine("4. GetClock");
            Console.WriteLine("5. GetConfig");
            Console.WriteLine("6. SetConfig");
            Console.WriteLine("0. Back");
            Console.Write("Choose: ");

            string? opt = Console.ReadLine();
            if (opt == "0") return;


            switch (opt)
            {
                case "1":
                    s_bl.Admin.ResetDB();
                    Console.WriteLine("ResetDB called.");
                    break;
                case "2":
                    s_bl.Admin.InitializeDB();
                    Console.WriteLine("InitializeDB called.");
                    break;
                case "3":
                    Console.Write("Forward by TimeUnit, Enter choice: (Minute / Hour / Day / Month / Year): ");
                    string? unitStr = Console.ReadLine();

                    if (Enum.TryParse(unitStr, true, out BO.EnumTimeUnit unit))
                    {
                        s_bl.Admin.ForwardClock(unit);
                        Console.WriteLine("Clock forwarded.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid TimeUnit.");
                    }
                    break;
                case "4":
                    Console.WriteLine($"Current Clock: {s_bl.Admin.GetClock()}");
                    break;
                case "5":
                    var cfg = s_bl.Admin.GetConfig();
                    //Console.WriteLine($"Max Range = {cfg.MaxRange}");
                    Console.WriteLine($"Manager Id = {cfg.ManagerId}");
                    Console.WriteLine($"Manager Password = {cfg.ManagerPassword}");
                    Console.WriteLine($"Company Address = {cfg.CompanyAddress}");
                    Console.WriteLine($"Latitude = {cfg.Latitude}");
                    Console.WriteLine($"Longitude = {cfg.Longitude}");
                    Console.WriteLine($"MaxDeliveryDistanceKm = {cfg.MaxDeliveryDistance}");
                    Console.WriteLine($"AvgCarSpeedKmH = {cfg.AveCarSpeedKmH}");
                    Console.WriteLine($"AvgMotorcycleSpeedKmH = {cfg.AveMotorcycleSpeedKmH}");
                    Console.WriteLine($"AvgBicycleSpeedKmH = {cfg.AveBicycleSpeedKmH}");
                    Console.WriteLine($"AvgWalkingSpeedKmH = {cfg.AveWalkingSpeedKmH}");
                    Console.WriteLine($"MaxDeliveryTimeRange = {cfg.GetMaxDeliveryTime}");
                    Console.WriteLine($"RiskRange = {cfg.RiskRange}");
                    Console.WriteLine($"InactivityTimeRange = {cfg.InactivityThreshold}");
                    break;
                case "6":
                    var currentCfg = s_bl.Admin.GetConfig();
                    Console.WriteLine("Press ENTER to leave a value unchanged.");

                    string? input;

                    Console.Write($"ManagerId ({currentCfg.ManagerId}): ");
                    input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input) && int.TryParse(input, out int managerId)) currentCfg.ManagerId = managerId;

                    Console.Write($"ManagerPassword ({currentCfg.ManagerPassword}): ");
                    input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input)) currentCfg.ManagerPassword = input;

                    Console.Write($"CompanyAddress ({currentCfg.CompanyAddress}): ");
                    input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input)) currentCfg.CompanyAddress = input;

                    Console.Write($"Latitude ({currentCfg.Latitude}): ");
                    input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input) && double.TryParse(input, out double lat)) currentCfg.Latitude = lat;

                    Console.Write($"Longitude ({currentCfg.Longitude}): ");
                    input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input) && double.TryParse(input, out double lon)) currentCfg.Longitude = lon;

                    Console.Write($"MaxDeliveryDistance ({currentCfg.MaxDeliveryDistance}): ");
                    input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input) && double.TryParse(input, out double maxDist)) currentCfg.MaxDeliveryDistance = maxDist;

                    Console.Write($"AvgCarSpeedKmH ({currentCfg.AveCarSpeedKmH}): ");
                    input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input) && double.TryParse(input, out double carSpeed)) currentCfg.AveCarSpeedKmH = carSpeed;

                    Console.Write($"AvgMotorcycleSpeedKmH ({currentCfg.AveMotorcycleSpeedKmH}): ");
                    input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input) && double.TryParse(input, out double motoSpeed)) currentCfg.AveMotorcycleSpeedKmH = motoSpeed;

                    Console.Write($"AvgBicycleSpeedKmH ({currentCfg.AveBicycleSpeedKmH}): ");
                    input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input) && double.TryParse(input, out double bikeSpeed)) currentCfg.AveBicycleSpeedKmH = bikeSpeed;

                    Console.Write($"AvgWalkingSpeedKmH ({currentCfg.AveWalkingSpeedKmH}): ");
                    input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input) && double.TryParse(input, out double walkSpeed)) currentCfg.AveWalkingSpeedKmH = walkSpeed;

                    Console.Write($"MaxDeliveryTime (hh:mm:ss) ({currentCfg.GetMaxDeliveryTime}): ");
                    input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input) && TimeSpan.TryParse(input, out TimeSpan maxTime)) currentCfg.GetMaxDeliveryTime = maxTime;

                    Console.Write($"RiskRange (hh:mm:ss) ({currentCfg.RiskRange}): ");
                    input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input) && TimeSpan.TryParse(input, out TimeSpan risk)) currentCfg.RiskRange = risk;

                    Console.Write($"InactivityThreshold (hh:mm:ss) ({currentCfg.InactivityThreshold}): ");
                    input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input) && TimeSpan.TryParse(input, out TimeSpan inactivity)) currentCfg.InactivityThreshold = inactivity;

                    s_bl.Admin.SetConfig(currentCfg);
                    Console.WriteLine("Configuration updated successfully.");
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }

        }
    }
    #endregion

    #region Order Menu
    static void OrderMenu()
    {
        while (true)
        {
            Console.WriteLine("\n--- Order Menu ---");
            Console.WriteLine("1. GetOrderSummary");
            Console.WriteLine("2. ListOrder");
            Console.WriteLine("3. ReadOrderDetails");
            Console.WriteLine("4. UpdateOrder");
            Console.WriteLine("5. CancelOrder");
            Console.WriteLine("6. DeleteOrder");
            Console.WriteLine("7. AddOrder");
            Console.WriteLine("8. EndOrderStatus");
            Console.WriteLine("9. GetClosedDeliveriesForCourier");
            Console.WriteLine("10. GetOpenOrdersToChooseForCourier");
            Console.WriteLine("0. Back");
            Console.Write("Choose: ");

            string? opt = Console.ReadLine();
            if (opt == "0") return;

            switch (opt)
            {
                case "1":
                    Console.Write("Asker ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int askerId1)) break;
                    var summary = s_bl.Order.GetAmountOfOrdersByStatus(askerId1);
                    foreach (var s in summary) Console.WriteLine(s);
                    break;
                case "2":
                    Console.Write("Asker ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int askerId2)) break;
                    var ordersList = s_bl.Order.GetOrderInList(askerId2, null);
                    foreach (var o in ordersList) Console.WriteLine(o);
                    break;
                case "3":
                    Console.Write("Asker ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int askerId3)) break;
                    Console.Write("Order ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int orderId3)) break;
                    Console.WriteLine(s_bl.Order.Read(askerId3, orderId3));
                    break;
                case "4":
                    Console.Write("Asker ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int askerId4)) break;
                    BO.Order existingOrder = InputNewOrder();
                    s_bl.Order.Update(askerId4, existingOrder);
                    Console.WriteLine("Order updated.");
                    break;
                case "5":
                    Console.Write("Asker ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int askerId5)) break;
                    Console.Write("Order ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int orderId5)) break;
                    s_bl.Order.CancelOrder(askerId5, orderId5);
                    Console.WriteLine("Order canceled.");
                    break;
                case "6":
                    Console.Write("Asker ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int askerId6)) break;
                    Console.Write("Order ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int orderId6)) break;
                    s_bl.Order.Delete(askerId6, orderId6);
                    Console.WriteLine("Order deleted.");
                    break;
                case "7":
                    Console.Write("Asker ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int askerId7)) break;
                    var newOrder = InputNewOrder();
                    s_bl.Order.Create(askerId7, newOrder);
                    Console.WriteLine("Order added.");
                    break;
                case "8":
                    Console.Write("Asker ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int askerId8)) break;
                    Console.Write("Order ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int orderId8)) break;
                    Console.Write("Delivery ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int deliveryId8)) break;
                    s_bl.Order.EndOrderStatus(askerId8, orderId8, deliveryId8);
                    Console.WriteLine("Order treatment ended.");
                    break;
                case "9":
                    Console.Write("Asker ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int askerId9)) break;
                    Console.Write("Courier ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int courierId9)) break;
                    var closed = s_bl.Order.GetClosedDeliveriesInListsToCourier(askerId9, courierId9);
                    foreach (var c in closed) Console.WriteLine(c);
                    break;
                case "10":
                    Console.Write("Asker ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int askerId10)) break;
                    Console.Write("Courier ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int courierId10)) break;
                    var open = s_bl.Order.GetListOfOpenOrderToChoose(askerId10, courierId10);
                    foreach (var o in open) Console.WriteLine(o);
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    public static BO.Order InputNewOrder()
    {
        Console.Write("Enter Order Type (Standard / Express / HeavyLoad): ");
        Enum.TryParse(Console.ReadLine(), true, out BO.EnumOrderType orderType);

        Console.Write("Verbal Description: ");
        string? verbalDescription = Console.ReadLine();

        Console.Write("Full Address: ");
        string? fullAddress = Console.ReadLine();

        Console.Write("Latitude: ");
        double.TryParse(Console.ReadLine(), out double latitude);

        Console.Write("Longitude: ");
        double.TryParse(Console.ReadLine(), out double longitude);

        Console.Write("Customer Full Name: ");
        string? customerFullName = Console.ReadLine();

        Console.Write("Customer Phone: ");
        string? customerPhone = Console.ReadLine();

        Console.Write("Weight: ");
        double.TryParse(Console.ReadLine(), out double weight);

        Console.Write("Is fragile? (yes/no): ");
        bool isFragile = Console.ReadLine()?.ToLower() == "yes";

        return new BO.Order
        {
            OrderType = orderType,
            Description = verbalDescription,
            Address = fullAddress,
            Latitude = latitude,
            Longitude = longitude,
            CustomerName = customerFullName,
            CustomerPhone = customerPhone,
            Weight = weight,
            Fragile = isFragile,
            CreationTime = DateTime.Now
        };
    }
    #endregion

    #region Courier Menu
    static void CourierMenu()
    {
        while (true)
        {
            Console.WriteLine("\n--- Courier Menu ---");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. GetCouriersList");
            Console.WriteLine("3. GetCourierDetails");
            Console.WriteLine("4. AddNewCourier");
            Console.WriteLine("5. UpdateCourier");
            Console.WriteLine("6. DeleteCourier");
            Console.WriteLine("0. Back");
            Console.Write("Choose: ");

            string? opt = Console.ReadLine();
            if (opt == "0") return;

            switch (opt)
            {
                case "1":
                    Console.Write("Courier ID (empty = null): ");
                    string? idStr = Console.ReadLine();
                    string? cId = string.IsNullOrWhiteSpace(idStr) ? null : idStr;
                    Console.Write("Password: ");
                    string? pwd = Console.ReadLine();
                    var user = s_bl.Courier.Login(cId ?? string.Empty, pwd ?? string.Empty);
                    Console.WriteLine(user);
                    break;
                case "2":
                    Console.Write("Asker ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int asker2)) break;
                    IEnumerable<BO.CourierInList>? list = s_bl.Courier.GetCouriersInList(asker2);
                    if (list == null) { Console.WriteLine("No couriers found."); break; }
                    foreach (var c in list) Console.WriteLine(c);
                    break;
                case "3":
                    Console.Write("Asker ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int asker3)) break;
                    Console.Write("Courier ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int courierId3)) break;
                    Console.WriteLine(s_bl.Courier.Read(asker3, courierId3));
                    break;
                case "4":
                    Console.Write("Asker ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int asker4)) break;
                    var newCourier = InputNewCourier();
                    s_bl.Courier.Create(asker4, newCourier);
                    Console.WriteLine("Courier added.");
                    break;
                case "5":
                    Console.Write("Asker ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int asker5)) break;
                    var updated = InputNewCourier();
                    s_bl.Courier.Update(asker5, updated);
                    Console.WriteLine("Courier updated.");
                    break;
                case "6":
                    Console.Write("Asker ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int asker6)) break;
                    Console.Write("Courier ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int courierId6)) break;
                    s_bl.Courier.Delete(asker6, courierId6);
                    Console.WriteLine("Courier deleted.");
                    break;
            }
        }
    }    

    public static BO.Courier InputNewCourier()
    {
        Console.Write("Courier ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) throw new Exception("Invalid ID");

        Console.Write("Start Date (yyyy-mm-dd): ");
        DateTime.TryParse(Console.ReadLine(), out DateTime startAtCompany);

        Console.Write("Full Name: ");
        string? name = Console.ReadLine();

        Console.Write("Phone: ");
        string? phone = Console.ReadLine();

        Console.Write("Email: ");
        string? email = Console.ReadLine();

        Console.Write("Password: ");
        string? password = Console.ReadLine();

        Console.Write("Is Active (true/false): ");
        bool.TryParse(Console.ReadLine(), out bool isActive);

        Console.Write("Max Distance (blank for null): ");
        string? maxDistStr = Console.ReadLine();
        double? maxDistance = string.IsNullOrWhiteSpace(maxDistStr) ? null : double.Parse(maxDistStr);

        return new BO.Courier
        {
            Id = id,
            StartedWorking = startAtCompany,
            Name = name,
            CourierPhone = phone,
            Email = email,
            Password = password,
            Active = isActive,
            MaxPersonalDistance = maxDistance
        };
    }
    #endregion

    static void PrintBlException(Exception ex)
    {
        switch (ex)
        {
            case BO.BlDoesNotExistException e:
                Console.WriteLine($"BL ERROR - Does Not Exist: {e.Message}"); break;
            case BO.BlAlreadyExistsException e:
                Console.WriteLine($"BL ERROR - Already Exists: {e.Message}"); break;
            case BO.BlInvalidInputException e:
                Console.WriteLine($"BL ERROR - Invalid Input: {e.Message}"); break;
            case BO.BlInvalidOperationException e:
                Console.WriteLine($"BL ERROR - Invalid Operation: {e.Message}"); break;
            case BO.BlUnauthorizedException e:
                Console.WriteLine($"BL ERROR - Unauthorized: {e.Message}"); break;
            case BO.BLTemporaryNotAvailableException e:
                Console.WriteLine($"BL ERROR - Temporary Not Available: {e.Message}"); break;
            default:
                Console.WriteLine($"Unexpected error: {ex.Message}"); break;
        }

        if (ex.InnerException != null)
            Console.WriteLine("Inner: " + ex.InnerException.Message);
    }
}

////using BO;
////using DO;

//using DalApi;

//namespace BlTest;
//using Helpers;
//using System.Runtime.CompilerServices;

//public class Program
//{
//    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

//    static void Main(string[] args)
//    {
//        try
//        {
//            MainMenu(); // ability to activate all functions called from BlImplementation 
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine(ex.Message);
//        }


//    }
//    private static void MainMenu()
//    {
//        bool exit = false;

//        while (!exit)
//        {
//            try
//            {
//                Console.WriteLine("\n=== MAIN MENU ===");
//                Console.WriteLine("1. Courier functions");
//                Console.WriteLine("2. Order functions");
//                Console.WriteLine("3. Config functions (Admin)");
//                //Console.WriteLine("5. Initialize Data (call Initialization.Do)");
//                //Console.WriteLine("6. Reset all data");
//                Console.WriteLine("0. Exit");
//                Console.Write("Choose: ");

//                if (!Enum.TryParse(Console.ReadLine(), out BO.MainMenuOption choice))
//                {
//                    Console.WriteLine("Invalid choice!");
//                    continue;
//                }

//                switch (choice)
//                {
//                    case BO.MainMenuOption.CourierFunctions:
//                        CourierFunctionsMenu();
//                        break;

//                    case BO.MainMenuOption.OrderFunctions:
//                        OrderFunctionsMenu();
//                        break;

//                    case BO.MainMenuOption.ConfigFunctions:
//                        ConfigFunctionsMenu();
//                        break;

//                    case BO.MainMenuOption.Exit:
//                        exit = true;
//                        break;

//                    default:
//                        throw new BO.BlInvalidInputException($"Invalid option: {choice}");
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }
//        }
//    }

//    /// <summary>
//    /// MainMenu -> CourierFunctionsMenu 
//    /// </summary>
//    private static void CourierFunctionsMenu()
//    {
//        bool back = false;
//        while (!back)
//        {
//            Console.WriteLine("\n--- Courier Functions Menu ---");
//            Console.WriteLine("1. Log - In"); //should be forced to log in before using courier functions
//            Console.WriteLine("2. Add Courier");
//            Console.WriteLine("3. Show Courier by ID");
//            Console.WriteLine("4. Show All Couriers");
//            Console.WriteLine("5. Update Courier");
//            Console.WriteLine("6. Delete Courier");
//            Console.WriteLine("7. Number of Deliveries On Time for Courier");
//            Console.WriteLine("8. Number of Deliveries Late for Courier");
//            Console.WriteLine("9. Assign Delivery to Courier");
//            Console.WriteLine("10. Close Deliveries for Courier");
//            Console.WriteLine("0. Back");
//            Console.Write("Choose: ");

//            if (!Enum.TryParse(Console.ReadLine(), out BO.CourierMenuOption choice))
//            {
//                Console.WriteLine("Invalid choice!");
//                continue;
//            }
//            switch (choice)
//            {
//                case BO.CourierMenuOption.LogIn:
//                    // Implement Log In functionality
//                    {
//                        Console.Write("Enter the ID to log in: ");
//                        if (!int.TryParse(Console.ReadLine(), out int id))
//                        {
//                            Console.WriteLine("Invalid id format.");
//                            return;
//                        }
//                        string idString = id.ToString();
//                        Console.WriteLine("Enter the password to log in: ");
//                        string? password = Console.ReadLine() ?? throw new BO.BlInvalidInputException("Password cannot be null.");
//                        BO.EnumUserRole User = s_bl.Courier.Login(idString,password);
//                        Console.WriteLine($"Logged in as");
//                    }
//                    break;

//                case BO.CourierMenuOption.AddCourier:
//                    // Implement Add Courier functionality
//                    Console.WriteLine("Enter your ID to check request: ");
//                    if (!int.TryParse(Console.ReadLine(), out int askerId))
//                    {
//                        throw new BO.BlInvalidInputException("Invalid id format.");
//                    }
//                    //
//                    //get info
//                    DO.Courier? doCourier = new()
//                    {
//                        Id = boCourier.Id,
//                        Name = boCourier.Name!,
//                        CourierPhone = boCourier.CourierPhone!,
//                        Email = boCourier.Email!,
//                        Password = boCourier.Password!,
//                        Active = boCourier.Active,
//                        DeliveryMethod = (DO.EnumDeliveryMethod)boCourier.DeliveryMethod,
//                        StartedWorking = DateTime.Now,
//                        MaxPersonalDistance = boCourier.MaxPersonalDistance
//                    };
//                    BO.Courier newCourier= 
//                    s_bl.Courier.Create(askerId, newCourier);
//                    Console.WriteLine("Courier added.");

//                    break;

//                case BO.CourierMenuOption.ShowCourierById:
//                    // Implement Show Courier by ID functionality
//                    {
//                        Console.Write("Enter courier ID to display: ");
//                        if (!int.TryParse(Console.ReadLine(), out int id))
//                        {
//                            throw new BO.BlInvalidInputException("Invalid id format.");
//                        }
//                        var bo = s_bl.Courier.Read(id, id);
//                        Console.WriteLine(bo);
//                    }
//                    break;
//                case BO.CourierMenuOption.ShowListOfCouriers:
//                    // Implement Show List of Couriers functionality
//                    {
//                        Console.WriteLine("Enter your ID to check request: ");
//                        if (!int.TryParse(Console.ReadLine(), out int id))
//                        {
//                            throw new BO.BlInvalidInputException("Invalid id format.");
//                        }
//                        IEnumerable<BO.CourierInList>? list = s_bl.Courier.GetCouriersInList(id);
//                        if (list == null)
//                        {
//                            return;
//                        }
//                        list.ToList().ForEach(Console.WriteLine);
//                    }
//                    break;
//                case BO.CourierMenuOption.UpdateCourier:
//                    // Implement Update Courier functionality
//                    {
//                        Console.WriteLine("Enter the ID of the requester: ");
//                        if (!int.TryParse(Console.ReadLine(), out int idR))
//                        {
//                            throw new BO.BlInvalidInputException("Invalid id format.");
//                        }
//                        Console.Write("Enter ID of courier to update: ");
//                        if (!int.TryParse(Console.ReadLine(), out int id))
//                        {
//                            throw new BO.BlInvalidInputException($"Invalid Courier ID={id} format");
//                        }
//                        BO.Courier existing = s_bl.Courier.Read(idR, id) ?? throw new BO.BlDoesNotExistException($"Courier with ID={id} does not exist.");
//                        Console.WriteLine("Enter new values (leave empty to keep current value):");

//                        Console.Write($"Name ({existing.Name}): ");
//                        string? name = Console.ReadLine();
//                        if (string.IsNullOrWhiteSpace(name)) name = existing.Name;

//                        Console.Write($"Phone ({existing.CourierPhone}): ");
//                        string? phone = Console.ReadLine();
//                        if (string.IsNullOrWhiteSpace(phone)) phone = existing.CourierPhone;

//                        Console.Write($"Email ({existing.Email}): ");
//                        string? email = Console.ReadLine();
//                        if (string.IsNullOrWhiteSpace(email)) email = existing.Email;

//                        Console.Write($"Password ({existing.Password}): ");
//                        string? password = Console.ReadLine();
//                        if (string.IsNullOrWhiteSpace(password)) password = existing.Password;

//                        Console.Write($"Delivery method ({existing.DeliveryMethod}): ");
//                        string? methodInput = Console.ReadLine();
//                        BO.EnumDeliveryMethod method = existing.DeliveryMethod;
//                        if (!string.IsNullOrWhiteSpace(methodInput))
//                            if (!Enum.TryParse(methodInput, true, out method))
//                                method = existing.DeliveryMethod;

//                        Console.Write($"Max Personal Distance ({existing.MaxPersonalDistance}): ");
//                        string? maxDistInput = Console.ReadLine();
//                        double? maxDist = existing.MaxPersonalDistance;
//                        if (!string.IsNullOrWhiteSpace(maxDistInput) && double.TryParse(maxDistInput, out double d))
//                            maxDist = d;
//                        s_bl.Courier.Update(idR, new BO.Courier
//                        {
//                            Id = id,
//                            Name = name,
//                            CourierPhone = phone,
//                            Email = email,
//                            Password = password,
//                            Active = existing.Active,
//                            DeliveryMethod = method,
//                            StartedWorking = existing.StartedWorking,
//                            MaxPersonalDistance = maxDist,
//                            TotalOnTimeDeliveries = existing.TotalOnTimeDeliveries,
//                            TotalLateDeliveries = existing.TotalLateDeliveries,
//                            ActiveDeliveryOrder = existing.ActiveDeliveryOrder
//                        });
//                    }
//                    break;
//                case BO.CourierMenuOption.DeleteCourier:
//                    // Implement Delete Courier functionality
//                    {
//                        Console.WriteLine("Enter the ID of the requester: ");
//                        if (!int.TryParse(Console.ReadLine(), out int idR))
//                        {
//                            throw new BO.BlInvalidInputException("Invalid id format.");
//                        }
//                        Console.Write("Enter ID of courier to delete: ");
//                        if (!int.TryParse(Console.ReadLine(), out int id))
//                        {
//                            throw new BO.BlInvalidInputException($"Invalid Courier ID={id} format");
//                        }
//                        s_bl.Courier.Delete(idR, id);
//                    }
//                    break;
//                case BO.CourierMenuOption.NumberOfDeliveriesOnTimeForCourier:
//                    // Implement Number of Deliveries On Time for Courier functionality
//                    break;
//                case BO.CourierMenuOption.NumberOfDeliveriesLateForCourier:
//                    // Implement Number of Deliveries Late for Courier functionality
//                    break;
//                case BO.CourierMenuOption.AssignDeliveryToCourier:
//                    // Implement Assign Delivery to Courier functionality
//                    break;
//                case BO.CourierMenuOption.CloseDeliveriesForCourier:
//                    // Implement Close Deliveries for Courier functionality
//                    break;
//                case BO.CourierMenuOption.Exit:
//                    back = true;
//                    break;
//                default:
//                    throw new BO.BlInvalidInputException($"Invalid option: {choice}");
//            }
//        }
//    }

//    private static void OrderFunctionsMenu()
//    {
//        bool back = false;
//        while (!back)
//        {
//            Console.WriteLine("\n--- Order Functions Menu ---");
//            Console.WriteLine("1. Create Order");
//            Console.WriteLine("2. Read Order");
//            Console.WriteLine("3. Show List Of Orders");
//            Console.WriteLine("4. Update Order");
//            Console.WriteLine("5. Delete Order");
//            Console.WriteLine("6. Cancel Order");
//            Console.WriteLine("7. Amount Of Order By Status");
//            Console.WriteLine("8. End Order Status");
//            Console.WriteLine("9. Create Delivery For Order");
//            Console.WriteLine("10. Closed Deliveries In List To Courier");
//            Console.WriteLine("11. List Of Open Orders To Choose");
//            Console.WriteLine("0. Back");
//            Console.Write("Choose: ");

//            if (!Enum.TryParse(Console.ReadLine(), out BO.OrderMenuOptions choice))
//            {
//                Console.WriteLine("Invalid choice!");
//                continue;
//            }
//            switch (choice)
//            {
//                case BO.OrderMenuOptions.AddOrder:
//                    // Implement Create Order functionality
//                    break;
//                case BO.OrderMenuOptions.ShowOrderById:
//                    // Implement Show Order by ID functionality
//                    break;
//                case BO.OrderMenuOptions.ShowListOfOrders:
//                    // Implement Show List of Orders functionality
//                    break;
//                case BO.OrderMenuOptions.UpdateOrder:
//                    // Implement Update Order functionality
//                    break;
//                case BO.OrderMenuOptions.DeleteOrder:
//                    // Implement Delete Order functionality
//                    break;
//                case BO.OrderMenuOptions.CancelOrder:
//                    // Implement Cancel Order functionality
//                    break;
//                case BO.OrderMenuOptions.AmountOfOrderByStatus:
//                    // Implement 
//                    break;
//                case BO.OrderMenuOptions.EndOrderStatus:
//                    // Implement 
//                    break;
//                case BO.OrderMenuOptions.CreateDeliveryForOrder:
//                    // Implement 
//                    break;
//                case BO.OrderMenuOptions.ClosedDeliveriesInListToCourier:
//                    // Implement 
//                    break;
//                case BO.OrderMenuOptions.ListOfOpenOrderToChoose:
//                    // Implement 
//                    break;
//                case BO.OrderMenuOptions.Exit:
//                    back = true;
//                    break;
//                default:
//                    throw new BO.BlInvalidInputException($"Invalid option: {choice}");
//            }
//        }
//    }

//    private static void ConfigFunctionsMenu()
//    {
//        bool back = false;
//        while(!back)
//        {
//            //MoveClock = 1,
//            //GetClock = 2,
//            //GetConfig = 3,
//            //InitializeDB = 4,
//            //ResetDB = 5,
//            //SetConfig = 6
//            Console.WriteLine("\n--- Config Functions Menu (Admin) ---");
//            Console.WriteLine("1. Move Clock");
//            Console.WriteLine("2. Get Clock");
//            Console.WriteLine("3. Get Config");
//            Console.WriteLine("4. Initialize DB");
//            Console.WriteLine("5. Reset DB");
//            Console.WriteLine("6. Set Config");
//            Console.WriteLine("0. Back");

//            if (!Enum.TryParse(Console.ReadLine(), out BO.ConfigMenuOptions choice))
//            {
//                Console.WriteLine("Invalid choice!");
//                continue;
//            }
//            switch (choice)
//            {
//                case BO.ConfigMenuOptions.MoveClock:
//                    //implement
//                    break;
//                    case BO.ConfigMenuOptions.GetClock:
//                        //implement
//                        break;
//                    case BO.ConfigMenuOptions.GetConfig:
//                        //implement
//                        break;
//                    case BO.ConfigMenuOptions.InitializeDB:
//                        //implement
//                        break;
//                    case BO.ConfigMenuOptions.ResetDB:
//                        //implement
//                        break;
//                    case BO.ConfigMenuOptions.SetConfig:
//                        //implement
//                        break;
//                case BO.ConfigMenuOptions.Exit:
//                    back = true;
//                    break;
//                default:
//                    throw new BO.BlInvalidInputException($"Invalid option: {choice}");

//            }

//        }
//    }
//}


