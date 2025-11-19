using Dal;
using DalApi;
namespace DalTest;
using DO;


internal class Program
{
    //private static ICourier s_dalCourier = new CourierImplementation(); //stage 1
    //private static IOrder? s_dalOrder = new OrderImplementation(); //stage 1
    //private static IDelivery? s_dalDelivery = new DeliveryImplementation(); //stage 1
    //private static IConfig? s_dalConfig = new ConfigImplementation(); //stage 1
    //static readonly IDal s_dal = new DalList(); //stage 2
    //static readonly IDal s_dal = new Dal.DalXml(); //stage 3
    static readonly IDal s_dal = Factory.Get; //stage 4


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
                        //Initialization.Do(s_dalCourier, s_dalOrder, s_dalDelivery, s_dalConfig);
                        //Initialization.Do(s_dal); //stage 2
                        Initialization.Do(); //stage 4
                        Console.WriteLine("Data initialized successfully!");
                        break;

                    case MainMenuOption.ResetAll:
                        ResetAll();
                        break;

                    case MainMenuOption.Exit:
                        exit = true;
                        break;

                    default:
                        throw new DalTestInvalidInputException($"Invalid option: {choice}");
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
                            throw new DalTestInvalidInputException($"Invalid Courier ID={id} format");
                        }
                        Courier? existingID = s_dal!.Courier.Read(id);
                        if (existingID != null)
                        {
                            throw new DalAlreadyExistsException($"Courier with ID={existingID.Id} already exists");
                        }

                        //if input is only spaces- will not throw exception here
                        Console.Write("Enter Name: ");
                        string? name = Console.ReadLine() ?? throw new DalTestInvalidInputException("Name was not entered");

                        Console.Write("Enter Phone: ");
                        string? phone = Console.ReadLine() ?? throw new DalTestInvalidInputException("Phone was not entered");

                        Console.Write("Enter Email: ");
                        string? email = Console.ReadLine() ?? throw new DalTestInvalidInputException("Email was not entered");

                        Console.Write("Enter Password: ");
                        string? password = Console.ReadLine() ?? throw new DalTestInvalidInputException("Password was not entered");

                        Console.WriteLine("Enter true/false if the courier is active");
                        if (!bool.TryParse(Console.ReadLine(), out bool active))
                        {
                            throw new DalTestInvalidInputException("Active status was not entered");
                        }

                        Console.Write("Delivery method (Car, Motorcycle, Bicycle, Foot): ");
                        if (!Enum.TryParse(Console.ReadLine(), true, out EnumDeliveryMethod method))
                        {
                            throw new DalTestInvalidInputException("Delivery method was not entered");
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

                        Courier newCourier = new(
                            Id: id,
                            Name: name,
                            CourierPhone: phone,
                            Email: email,
                            Password: password,
                            Active: active,
                            DeliveryMethod: method,
                            StartedWorking: s_dal!.Config.Clock,   // Default value
                            MaxPersonalDistance: maxDist
                        );
                        s_dal.Courier.Create(newCourier);
                        Console.WriteLine("Courier added successfully!");
                    }
                    break;

                case CourierMenuOption.GetCourier:
                    {
                        // Get Courier by ID logic here
                        Console.Write("Enter ID: ");

                        if (!int.TryParse(Console.ReadLine(), out int id))
                        {
                            throw new DalTestInvalidInputException($"Invalid Courier ID={id} format");
                        }
                        Courier? existingID = s_dal.Courier.Read(id) ?? throw new DalDoesNotExistException($"Courier with ID={id} does not exist.");
                        Console.WriteLine(existingID);
                    }
                    break;

                case CourierMenuOption.GetAllCouriers:
                    {
                        // Get All Couriers logic here
                        var couriers = s_dal.Courier.ReadAll();
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
                            throw new DalTestInvalidInputException($"Invalid Courier ID={id} format");
                        }
                        Courier? existing = s_dal.Courier.Read(id) ?? throw new DalDoesNotExistException($"Courier with ID={id} does not exist.");
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

                        s_dal.Courier.Update(updatedCourier);
                        Console.WriteLine("Courier updated successfully!");
                    }
                    break;

                case CourierMenuOption.DeleteCourier:
                    {
                        // Delete Courier logic here
                        Console.Write("Enter ID: ");
                        if (!int.TryParse(Console.ReadLine(), out int delID))
                        {
                            throw new DalTestInvalidInputException($"Invalid Courier ID={delID} format");
                        }
                        s_dal.Courier.Delete(delID);
                    }
                    break;

                case CourierMenuOption.DeleteAllCouriers:
                    {
                        // Delete All Couriers logic here
                        s_dal.Courier.DeleteAll();
                        Console.WriteLine("All couriers deleted successfully!");
                    }
                    break;

                case CourierMenuOption.Exit:
                    back = true;
                    break;
                default:
                    throw new DalTestInvalidInputException($"Invalid option: {choice}");
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
                            throw new DalTestInvalidInputException($"Invalid order type");
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
                            throw new DalTestInvalidInputException($"Invalid Address format: {tryAddress}");
                        }
                        string newAddress = tryAddress!;

                        Console.WriteLine("Enter address latitude");
                        string? tryLatitude = Console.ReadLine();
                        if (!double.TryParse(tryLatitude, out double newLatitude))
                        {
                            throw new DalTestInvalidInputException($"Invalid Latitude format: {tryLatitude}");
                        }

                        Console.WriteLine("Enter address longitude");
                        string? tryLongitude = Console.ReadLine();
                        if (!double.TryParse(tryLongitude, out double newLongitude))
                        {
                            throw new DalTestInvalidInputException($"Invalid Longitude format: {tryLongitude}");
                        }

                        Console.WriteLine("Enter customer name");
                        string? tryCustomerName = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(tryCustomerName))
                        {
                            throw new DalTestInvalidInputException($"Invalid Customer Name format: {tryCustomerName}");
                        }
                        string newCustomerName = tryCustomerName!;

                        Console.WriteLine("Enter customer phone number");
                        string? tryCustomerPhone = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(tryCustomerPhone))
                        {
                            throw new DalTestInvalidInputException($"Invalid Customer Phone Number format {tryCustomerPhone}");
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

                        Order newOrder = new(
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
                        s_dal.Order!.Create(newOrder);
                        Console.WriteLine("Order added successfully!");
                    }
                    break;

                case OrderMenuOption.GetOrder:
                    // Get Order by ID logic here
                    {
                        Console.Write("Enter ID: ");

                        if (!int.TryParse(Console.ReadLine(), out int id))
                        {
                            throw new DalTestInvalidInputException($"Invalid Order ID={id} format");
                        }
                        Order? existingID = s_dal.Order!.Read(id) ?? throw new DalDoesNotExistException($"Order with ID={id} does not exist.");
                        Console.WriteLine(existingID);
                    }
                    break;

                case OrderMenuOption.GetAllOrders:
                    // Get All Orders logic here
                    {
                        var orders = s_dal.Order!.ReadAll();
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
                            throw new DalTestInvalidInputException($"Invalid Order ID={id} format.");
                        }
                        Order? existing = s_dal.Order!.Read(id) ?? throw new DalDoesNotExistException($"Order with ID={id} does not exist.");

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

                        Order newOrder = new(
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
                        s_dal.Order!.Update(newOrder);
                        Console.WriteLine("Order updated successfully!");
                    }
                    break;

                case OrderMenuOption.DeleteOrder:
                    // Delete Order logic here
                    {
                        Console.Write("Enter ID: ");
                        if (!int.TryParse(Console.ReadLine(), out int delID))
                        {
                            throw new DalTestInvalidInputException($"Invalid Order ID={delID} format");
                        }
                        s_dal.Order!.Delete(delID);
                    }
                    break;

                case OrderMenuOption.DeleteAllOrders:
                    // Delete All Orders logic here
                    {
                        s_dal.Order!.DeleteAll();
                        Console.WriteLine("All orders deleted successfully!");
                    }
                    break;

                case OrderMenuOption.Exit:
                    back = true;
                    break;

                default:
                    throw new DalTestInvalidInputException($"Invalid option: {choice}");
            }
        }
    }

    /// <summary>
    /// MainMenu -> DeliveryMenu
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
                            throw new DalTestInvalidInputException($"Invalid Order ID={orderId} format");
                        }
                        if (s_dal.Order!.Read(orderId) == null)
                        {
                            throw new DalDoesNotExistException($"Order with ID={orderId} does not exist.");
                        } //if here- an order with this ID already exists

                        Console.WriteLine("Enter Courier ID: ");
                        if (!int.TryParse(Console.ReadLine(), out int newCourierId))
                        {
                            throw new DalTestInvalidInputException($"Invalid Courier ID={newCourierId} format");
                        }
                        if (s_dal.Courier.Read(newCourierId) == null)
                        {
                            throw new DalDoesNotExistException($"Courier with ID={newCourierId} does not exist.");
                        } //if here- a courier with this ID already exists

                        EnumDeliveryMethod newDeliveryMethod = s_dal.Courier.Read(newCourierId)!.DeliveryMethod;

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
                                DeliveryStartTime: s_dal.Config!.Clock,
                                DistanceInKm: newDistance,
                                EndDeliveryStatus: null,
                                EndDeliveryTime: null
                            );
                        s_dal.Delivery!.Create(newDelivery);
                        Console.WriteLine("Delivery added successfully!");
                    }
                    break;

                case DeliveryMenuOption.GetDelivery:
                    // Get Delivery by ID logic here
                    {
                        Console.Write("Enter ID: ");

                        if (!int.TryParse(Console.ReadLine(), out int id))
                        {
                            throw new DalTestInvalidInputException($"Invalid Delivery ID={id} format");
                        }
                        Delivery? existingID = s_dal.Delivery!.Read(id) ?? throw new DalDoesNotExistException($"Delivery with ID={id} does not exist.");
                        Console.WriteLine(existingID);
                    }
                    break;

                case DeliveryMenuOption.GetAllDeliveries:
                    // Get All Deliveries logic here
                    {
                        var deliveries = s_dal.Delivery!.ReadAll();
                        foreach (var delivery in deliveries)
                        {
                            Console.WriteLine(delivery);
                        }
                    }
                    break;

                case DeliveryMenuOption.UpdateDelivery:
                    // Update Delivery logic here
                    {
                        Console.Write("Enter ID of Delivery to update: ");
                        if (!int.TryParse(Console.ReadLine(), out int id))
                        {
                            throw new DalTestInvalidInputException($"Invalid Delivery ID={id} format");
                        }
                        Delivery? existing = s_dal.Delivery!.Read(id) ?? throw new DalDoesNotExistException($"Delivery with ID={id} does not exist.");

                        Console.WriteLine("Enter new values for the delivery (leave blank to keep current value):");

                        Console.Write($"Distance in Km ({existing.DistanceInKm}): ");
                        string? distanceInput = Console.ReadLine();
                        double? newDistance = existing.DistanceInKm;
                        if (!string.IsNullOrWhiteSpace(distanceInput) && double.TryParse(distanceInput, out double distanceVal))
                            newDistance = distanceVal;

                        if (existing.EndDeliveryStatus != null)
                            Console.WriteLine($"End Delivery Status ({existing.EndDeliveryStatus})");
                        else
                            Console.WriteLine("End Delivery Status (-empty-)");
                        string? status = Console.ReadLine();
                        EnumEndDeliveryStatus? newEndDeliveryStatus = existing.EndDeliveryStatus;
                        if (!string.IsNullOrWhiteSpace(status))
                            if (Enum.TryParse(status, true, out EnumEndDeliveryStatus tryStatus))
                                newEndDeliveryStatus = tryStatus;

                        DateTime? newEndDeliveryTime = null;
                        if (newEndDeliveryStatus != null)
                            newEndDeliveryTime = s_dal.Config!.Clock;


                        Delivery newDelivery = new(
                                Id: existing.Id,
                                OrderId: existing.OrderId,
                                CourierId: existing.CourierId,
                                DeliveryMethod: existing.DeliveryMethod,
                                DeliveryStartTime: existing.DeliveryStartTime,
                                DistanceInKm: newDistance,
                                EndDeliveryStatus: newEndDeliveryStatus,
                                EndDeliveryTime: newEndDeliveryTime
                            );
                        s_dal.Delivery!.Update(newDelivery);
                        Console.WriteLine("Delivery updated successfully!");
                    }
                    break;

                case DeliveryMenuOption.DeleteDelivery:
                    // Delete Delivery logic here
                    {
                        Console.Write("Enter ID: ");
                        if (!int.TryParse(Console.ReadLine(), out int delID))
                        {
                            throw new DalTestInvalidInputException($"Invalid Delivery ID={delID} format");
                        }
                        s_dal.Delivery!.Delete(delID);
                    }
                    break;

                case DeliveryMenuOption.DeleteAllDeliveries:
                    // Delete All Deliveries logic here
                    {
                        s_dal.Delivery!.DeleteAll();
                        Console.WriteLine("All deliveries deleted successfully!");
                    }
                    break;

                case DeliveryMenuOption.Exit:
                    back = true;
                    break;
                default:
                    throw new DalTestInvalidInputException($"Invalid option: {choice}");
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

            string? input = Console.ReadLine();
            if (!Enum.TryParse(input, true, out ConfigMenuOption choice))
            {
                throw new DalTestInvalidInputException($"Invalid option: {input}");
            }
            switch (choice)
            {
                case ConfigMenuOption.Add1MinToClock:
                    s_dal.Config!.Clock = s_dal.Config.Clock.AddMinutes(1);
                    Console.WriteLine($"Clock advanced by 1 minute {s_dal.Config.Clock}");
                    break;
                case ConfigMenuOption.Add1HourToClock:
                    s_dal.Config!.Clock = s_dal.Config.Clock.AddHours(1);
                    Console.WriteLine($"Clock advanced by 1 hour {s_dal.Config.Clock}");
                    break;
                case ConfigMenuOption.Add1DayToClock:
                    s_dal.Config!.Clock = s_dal.Config.Clock.AddDays(1);
                    Console.WriteLine($"Clock advanced by 1 day {s_dal.Config.Clock}");
                    break;
                case ConfigMenuOption.Add1WeekToClock:
                    s_dal.Config!.Clock = s_dal.Config.Clock.AddDays(7);
                    Console.WriteLine($"Clock advanced by 1 week {s_dal.Config.Clock}");
                    break;
                case ConfigMenuOption.ShowCurrentClock:
                    Console.WriteLine(s_dal.Config!.Clock);
                    break;
                case ConfigMenuOption.SetConfigParameters:
                    SetConfigParameters();
                    break;
                case ConfigMenuOption.GetConfigParameters:
                    GetConfigParameters();
                    break;
                case ConfigMenuOption.ResetConfigToDefault:
                    s_dal.Config!.Reset();
                    Console.WriteLine("Config reset to default successfully!");
                    break;
                case ConfigMenuOption.Exit:
                    exit = true;
                    break;
                default:
                    throw new DalTestInvalidInputException($"Invalid option: {choice}");
            }
        }
    }

    /// <summary>
    /// Reset all data and config
    /// </summary>
    /// <exception cref="DalTestInvalidInputException">in case DAL is not initialized yet</exception> 
    private static void ResetAll()
    {
        if (s_dal.Courier == null || s_dal.Order == null || s_dal.Delivery == null || s_dal.Config == null)
        {
            throw new DalTestInvalidInputException("Error: DAL not initialized yet!");
        }
        s_dal.Courier.DeleteAll(); //stage 1
        s_dal.Order.DeleteAll(); //stage 1
        s_dal.Delivery.DeleteAll(); //stage 1                
        s_dal.Config.Reset(); //stage 1
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

            string? inputChoice = Console.ReadLine();
            if (!Enum.TryParse(inputChoice, true, out SetConfigParametersOption choice))
            {
                throw new DalTestInvalidInputException($"Invalid option: {inputChoice}");
            }
            switch (choice)
            {
                case SetConfigParametersOption.SetClock:
                    s_dal.Config!.Clock = DateTime.Now;
                    break;
                case SetConfigParametersOption.SetCompanyAddress:
                    {
                        Console.WriteLine("Enter new address: <street>, <building-number>, <city>");
                        string? newAddress = Console.ReadLine();
                        s_dal.Config!.CompanyAddress = newAddress;
                        break;
                    }
                case SetConfigParametersOption.SetLatitude:
                    {
                        Console.WriteLine("Enter new latitude");
                        string? input = Console.ReadLine();
                        double newLat;
                        if (double.TryParse(input, out newLat))
                            s_dal.Config!.Latitude = newLat;
                        else
                        {
                            Console.WriteLine("Error: Invalid latitude format.");
                            s_dal.Config!.Latitude = s_dal.Config.Latitude;
                        }
                        break;
                    }
                case SetConfigParametersOption.SetLongitude:
                    {
                        Console.WriteLine("Enter new longitude");
                        string? input = Console.ReadLine();
                        double newLon;
                        if (double.TryParse(input, out newLon))
                            s_dal.Config!.Longitude = newLon;
                        else
                        {
                            Console.WriteLine("Error: Invalid longitude format.");
                            s_dal.Config!.Longitude = s_dal.Config.Longitude;
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

            string? inputChoice = Console.ReadLine();
            if (!Enum.TryParse(inputChoice, true, out GetConfigParametersOption choice))
            {
               throw new DalTestInvalidInputException($"Invalid option: {inputChoice}");
            }
            switch (choice)
            {
                case GetConfigParametersOption.GetClock:
                    Console.WriteLine(s_dal.Config!.Clock);
                    break;
                case GetConfigParametersOption.GetCompanyAddress:
                    Console.WriteLine(s_dal.Config!.CompanyAddress);
                    break;
                case GetConfigParametersOption.GetLatitude:
                    Console.WriteLine(s_dal.Config!.Latitude);
                    break;
                case GetConfigParametersOption.GetLongitude:
                    Console.WriteLine(s_dal.Config!.Longitude);
                    break;
                case GetConfigParametersOption.GetMaxDeliveryDistance:
                    Console.WriteLine(s_dal.Config!.MaxDeliveryDistance);
                    break;
                case GetConfigParametersOption.GetMaxDeliveryTime:
                    Console.WriteLine(s_dal.Config!.GetMaxDeliveryTime);
                    break;
                case GetConfigParametersOption.GetRiskRange:
                    Console.WriteLine(s_dal.Config!.RiskRange);
                    break;
                case GetConfigParametersOption.GetInactivityThreshold:
                    Console.WriteLine(s_dal.Config!.InactivityThreshold);
                    break;
                case GetConfigParametersOption.Back:
                    back = true;
                    break;
            }
        }
    }

}

