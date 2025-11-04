using Dal;
using DalApi;
namespace DalTest;
using DO;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;
using System.Xml.Linq;

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
                        throw new Exception("Invalid option!");
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
                        if (!int.TryParse(Console.ReadLine(), out int id))
                        {
                            throw new Exception("Invalid ID format");
                        }
                        Courier? existingID = s_dalCourier.Read(id);
                        if (existingID != null)
                        {
                            throw new Exception("Courier with this ID already exist.");
                        }

                        Console.Write("Enter Name: ");
                        string? name = Console.ReadLine() ?? throw new Exception("Wrong input");

                        Console.Write("Enter Phone: ");
                        string? phone = Console.ReadLine() ?? throw new Exception("Wrong input");

                        Console.Write("Enter Email: ");
                        string? email = Console.ReadLine() ?? throw new Exception("Wrong input");

                        Console.Write("Enter Password: ");
                        string? password = Console.ReadLine() ?? throw new Exception("Wrong input");

                        Console.WriteLine("Enter true/false if the courier is active");
                        if (!bool.TryParse(Console.ReadLine(), out bool active))
                        {
                            throw new Exception("Wrong input");
                        }

                        Console.Write("Delivery method (Car, Motorcycle, Bicycle, Foot): ");
                        if (!Enum.TryParse(Console.ReadLine(), true, out EnumDeliveryMethod method))
                        {
                            throw new Exception("Wrong input");
                        }

                        Console.Write("Enter Max Personal Distance: ");
                        double? maxDist;
                        if (!double.TryParse(Console.ReadLine(), out double maxDistVal))
                        {
                            maxDist = null;
                        }
                        else
                        {
                            maxDist = maxDistVal;
                        }

                        Courier newCourier = new Courier(
                            Id: id,
                            Name: name,
                            CourierPhone: phone,
                            Email: email,
                            Password: password,
                            Active: active,
                            DeliveryMethod: method,
                            StartedWorking: s_dalConfig!.Clock,   // Default value
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

                        if (!int.TryParse(Console.ReadLine(), out int id))
                        {
                            throw new Exception("Invalid ID format");
                        }
                        Courier? existingID = s_dalCourier.Read(id) ?? throw new Exception("Courier with this ID does not exist.");
                        Console.WriteLine(existingID);
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
                            throw new Exception("Invalid ID format.");
                        }
                        Courier? existing = s_dalCourier.Read(id) ?? throw new Exception("Courier with this ID does not exist.");
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

                        Console.WriteLine($"Active ({existing.Active}): ");
                        if (!bool.TryParse(Console.ReadLine(), out bool active))
                        {
                            active = existing.Active;
                        }

                        Console.Write($"Delivery method ({existing.DeliveryMethod}): ");
                        string? methodInput = Console.ReadLine();
                        EnumDeliveryMethod method = existing.DeliveryMethod;
                        if (!string.IsNullOrWhiteSpace(methodInput))
                            if (!Enum.TryParse(methodInput, true, out method))
                                method = existing.DeliveryMethod;

                        Console.Write($"Max Personal Distance ({existing.MaxPersonalDistance}): ");
                        string? maxDistInput = Console.ReadLine();
                        double? maxDist = existing.MaxPersonalDistance;
                        if (!string.IsNullOrWhiteSpace(maxDistInput) && double.TryParse(maxDistInput, out double d))
                            maxDist = d;

                        //create new updated courier object
                        Courier updatedCourier = new Courier(
                            Id: existing.Id,
                            Name: name,
                            CourierPhone: phone,
                            Email: email,
                            Password: password,
                            Active: active,
                            DeliveryMethod: method,
                            StartedWorking: existing.StartedWorking, //Stays the same
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
                        if (!int.TryParse(Console.ReadLine(), out int delID))
                        {
                            throw new Exception("Invalid ID format");
                        }
                        s_dalCourier.Delete(delID);
                    }
                    break;

                case CourierMenuOption.DeleteAllCouriers:
                    {
                        // Delete All Couriers logic here
                        s_dalCourier.DeleteAll();
                        Console.WriteLine("All couriers deleted successfully!");
                    }
                    break;

                case CourierMenuOption.Exit:
                    back = true;
                    break;
                default:
                    throw new Exception("Invalid option!");
            }


        }
    }

    /// <summary>
    /// MainMenu -> OrderMenu
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
                    {
                        //id=0, updated during creation

                        Console.Write("Enter type of order: Regular, Express, Overnight");
                        if (!Enum.TryParse(Console.ReadLine(), true, out EnumOrderType newOrderType))
                        {
                            throw new Exception("Wrong input");
                        }

                        Console.WriteLine("Enter order description");
                        string? newDescription = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(newDescription))
                        {
                            newDescription = null;
                        }

                        Console.WriteLine("Enter order address");
                        string? tryAddress = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(tryAddress))
                        {
                            throw new Exception("Invalid Address format");
                        }
                        string newAddress = tryAddress!;

                        Console.WriteLine("Enter address latitude");
                        string? tryLatitude = Console.ReadLine();
                        if (!double.TryParse(tryLatitude, out double newLatitude))
                        {
                            throw new Exception("Invalid Latitude format");
                        }

                        Console.WriteLine("Enter address longitude");
                        string? tryLongitude = Console.ReadLine();
                        if (!double.TryParse(tryLongitude, out double newLongitude))
                        {
                            throw new Exception("Invalid Longitude format");
                        }

                        Console.WriteLine("Enter customer name");
                        string? tryCustomerName = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(tryCustomerName))
                        {
                            throw new Exception("Invalid Customer Name format");
                        }
                        string newCustomerName = tryCustomerName!;

                        Console.WriteLine("Enter customer phone number");
                        string? tryCustomerPhone = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(tryCustomerPhone))
                        {
                            throw new Exception("Invalid Customer Phone Number format");
                        }
                        string newCustomerPhone = tryCustomerPhone!;

                        Console.Write("Enter order weight ");
                        double? newWeight;
                        if (!double.TryParse(Console.ReadLine(), out double tryWeight))
                        {
                            newWeight = null;
                        }
                        else
                        {
                            newWeight = tryWeight;
                        }

                        Console.Write("Enter true/false if order is fragile ");
                        bool? newFragile;
                        if (!bool.TryParse(Console.ReadLine(), out bool tryFragile))
                        {
                            newFragile = null;
                        }
                        else
                        {
                            newFragile = tryFragile;
                        }

                        Order newOrder = new Order(
                         Id: 0,
                         OrderType: newOrderType,
                         Description: newDescription,
                         Address: newAddress,
                         Latitude: newLatitude,
                         Longitude: newLongitude,
                         CustomerName: newCustomerName,
                         CustomerPhone: newCustomerPhone,
                         OrderCreationTime: DateTime.Now,
                         Weight: newWeight,
                         Fragile: newFragile
                        );
                        s_dalOrder!.Create(newOrder);
                        Console.WriteLine("Order added successfully!");
                    }
                    break;

                case OrderMenuOption.GetOrder:
                    // Get Order by ID logic here
                    {
                        Console.Write("Enter ID: ");

                        if (!int.TryParse(Console.ReadLine(), out int id))
                        {
                            throw new Exception("Invalid ID format");
                        }
                        Order? existingID = s_dalOrder!.Read(id);
                        if (existingID == null)
                        {
                            throw new Exception("Order with this ID does not exist.");
                        }
                        Console.WriteLine(existingID);
                    }
                    break;

                case OrderMenuOption.GetAllOrders:
                    // Get All Orders logic here
                    {
                        var orders = s_dalOrder!.ReadAll();
                        foreach (var order in orders)
                        {
                            Console.WriteLine(order);
                        }
                    }
                    break;

                case OrderMenuOption.UpdateOrder:
                    // Update Order logic here
                    {
                        Console.Write("Enter ID of Order to update: ");
                        if (!int.TryParse(Console.ReadLine(), out int id))
                        {
                            throw new Exception("Invalid ID format.");
                        }
                        Order? existing = s_dalOrder!.Read(id);
                        if (existing == null)
                        {
                            throw new Exception("Order with this ID does not exist.");
                        }

                        Console.WriteLine("Enter new values for the order (leave blank to keep current value):");

                        Console.Write($"Order Type ({existing.OrderType}): ");
                        string? orderTypeInput = Console.ReadLine();
                        EnumOrderType newOrderType = existing.OrderType;
                        if (!string.IsNullOrWhiteSpace(orderTypeInput))
                            if (!Enum.TryParse(orderTypeInput, true, out newOrderType))
                                newOrderType = existing.OrderType;

                        Console.Write($"Description ({existing.Description}): ");
                        string? newDescription = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(newDescription))
                            newDescription = existing.Description;

                        Console.Write($"Address ({existing.Address}): ");
                        string? newAddress = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(newAddress))
                            newAddress = existing.Address;

                        Console.Write($"Latitude ({existing.Latitude}): ");
                        string? latInput = Console.ReadLine();
                        double newLatitude = existing.Latitude;
                        if (!string.IsNullOrWhiteSpace(latInput) && double.TryParse(latInput, out double latVal))
                            newLatitude = latVal;

                        Console.Write($"Longitude ({existing.Longitude}): ");
                        string? lonInput = Console.ReadLine();
                        double newLongitude = existing.Longitude;
                        if (!string.IsNullOrWhiteSpace(lonInput) && double.TryParse(lonInput, out double lonVal))
                            newLongitude = lonVal;

                        Console.Write($"Customer Name ({existing.CustomerName}): ");
                        string? newCustomerName = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(newCustomerName))
                            newCustomerName = existing.CustomerName;

                        Console.Write($"Customer Phone ({existing.CustomerPhone}): ");
                        string? newCustomerPhone = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(newCustomerPhone))
                            newCustomerPhone = existing.CustomerPhone;

                        Console.Write($"Weight ({existing.Weight}): ");
                        string? weightInput = Console.ReadLine();
                        double? newWeight = existing.Weight;
                        if (!string.IsNullOrWhiteSpace(weightInput) && double.TryParse(weightInput, out double weightVal))
                            newWeight = weightVal;

                        Console.Write($"Fragile ({existing.Fragile}): ");
                        string? fragileInput = Console.ReadLine();
                        bool? newFragile = existing.Fragile;
                        if (!string.IsNullOrWhiteSpace(fragileInput) && bool.TryParse(fragileInput, out bool fragileVal))
                            newFragile = fragileVal;

                        Order newOrder = new Order(
                         Id: existing.Id,
                         OrderType: newOrderType,
                         Description: newDescription,
                         Address: newAddress,
                         Latitude: newLatitude,
                         Longitude: newLongitude,
                         CustomerName: newCustomerName,
                         CustomerPhone: newCustomerPhone,
                         OrderCreationTime: DateTime.Now,
                         Weight: newWeight,
                         Fragile: newFragile
                        );
                        s_dalOrder!.Update(newOrder);
                        Console.WriteLine("Order updated successfully!");
                    }
                    break;

                case OrderMenuOption.DeleteOrder:
                    // Delete Order logic here
                    {
                        Console.Write("Enter ID: ");
                        if (!int.TryParse(Console.ReadLine(), out int delID))
                        {
                            throw new Exception("Invalid ID format");
                        }
                        s_dalOrder!.Delete(delID);
                    }
                    break;

                case OrderMenuOption.DeleteAllOrders:
                    // Delete All Orders logic here
                    {
                        s_dalOrder!.DeleteAll();
                        Console.WriteLine("All orders deleted successfully!");
                    }
                    break;

                case OrderMenuOption.Exit:
                    back = true;
                    break;

                default:
                    throw new Exception("Invalid option!");
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
                    {
                        //id=0, updated during creation

                        Console.WriteLine("Enter Order ID: ");
                        if (!int.TryParse(Console.ReadLine(), out int orderId))
                        {
                            throw new Exception("Invalid Order ID format");
                        }
                        if (s_dalOrder!.Read(orderId) == null)
                        {
                            throw new Exception("Order with this ID does not exist.");
                        } //if here- an order with this ID already exists

                        Console.WriteLine("Enter Courier ID: ");
                        if (!int.TryParse(Console.ReadLine(), out int newCourierId))
                        {
                            throw new Exception("Invalid Courier ID format");
                        }
                        if (s_dalCourier.Read(newCourierId) == null)
                        {
                            throw new Exception("Courier with this ID does not exist.");
                        } //if here- a courier with this ID already exists

                        EnumDeliveryMethod newDeliveryMethod = s_dalCourier.Read(newCourierId)!.DeliveryMethod;

                        Console.WriteLine("Enter Delivery distance: ");
                        double? newDistance;
                        if (!double.TryParse(Console.ReadLine(), out double tryDistance))
                        {
                            newDistance = null;
                        }
                        else
                        {
                            newDistance = tryDistance;
                        }

                        Delivery newDelivery = new(
                                Id: 0,
                                OrderId: orderId,
                                CourierId: newCourierId,
                                DeliveryMethod: newDeliveryMethod,
                                DeliveryStartTime: s_dalConfig!.Clock,
                                DistanceInKm: newDistance,
                                EndDeliveryStatus: null,
                                EndDeliveryTime: null
                            );
                        s_dalDelivery!.Create(newDelivery);
                        Console.WriteLine("Delivery added successfully!");
                    }
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
                    throw new Exception("Invalid option!");
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
            Console.WriteLine("4. Advance clock by one week");
            Console.WriteLine("5. Advance clock by one month");
            Console.WriteLine("6. Show current clock");
            Console.WriteLine("7. Set config parameters");
            Console.WriteLine("8. Get config parameters");
            Console.WriteLine("0. Exit submenu");
            Console.Write("Choose an option: ");

            if (!Enum.TryParse(Console.ReadLine(), out ConfigMenuOption choice))
            {
                throw new Exception("Invalid option!");
            }
            switch (choice)
            {
                case ConfigMenuOption.Add1MinToClock:
                    s_dalConfig!.Clock = s_dalConfig.Clock.AddMinutes(1);
                    Console.WriteLine($"Clock advanced by 1 minute {s_dalConfig.Clock}");
                    break;
                case ConfigMenuOption.Add1HourToClock:
                    s_dalConfig!.Clock = s_dalConfig.Clock.AddHours(1);
                    Console.WriteLine($"Clock advanced by 1 hour {s_dalConfig.Clock}");
                    break;
                case ConfigMenuOption.Add1DayToClock:
                    s_dalConfig!.Clock = s_dalConfig.Clock.AddDays(1);
                    Console.WriteLine($"Clock advanced by 1 day {s_dalConfig.Clock}");
                    break;
                case ConfigMenuOption.Add1WeekToClock:
                    s_dalConfig!.Clock = s_dalConfig.Clock.AddDays(7);
                    Console.WriteLine($"Clock advanced by 1 week {s_dalConfig.Clock}");
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
                    Console.WriteLine("Config reset to default successfully!");
                    break;
                case ConfigMenuOption.Exit:
                    exit = true;
                    break;
                default:
                    throw new Exception("Invalid option!");
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
                throw new Exception("Invalid option!");
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

