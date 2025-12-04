//using BO;
//using DO;

using DalApi;

namespace BlTest;
using Helpers;
using System.Runtime.CompilerServices;

public class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    static void Main(string[] args)
    {
        try
        {
            MainMenu(); // ability to activate all functions called from BlImplementation 
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
                Console.WriteLine("1. Courier functions");
                Console.WriteLine("2. Order functions");
                Console.WriteLine("3. Config functions (Admin)");
                //Console.WriteLine("5. Initialize Data (call Initialization.Do)");
                //Console.WriteLine("6. Reset all data");
                Console.WriteLine("0. Exit");
                Console.Write("Choose: ");

                if (!Enum.TryParse(Console.ReadLine(), out BO.MainMenuOption choice))
                {
                    Console.WriteLine("Invalid choice!");
                    continue;
                }

                switch (choice)
                {
                    case BO.MainMenuOption.CourierFunctions:
                        CourierFunctionsMenu();
                        break;

                    case BO.MainMenuOption.OrderFunctions:
                        OrderFunctionsMenu();
                        break;

                    case BO.MainMenuOption.ConfigFunctions:
                        ConfigFunctionsMenu();
                        break;

                    case BO.MainMenuOption.Exit:
                        exit = true;
                        break;

                    default:
                        throw new BO.BlInvalidInputException($"Invalid option: {choice}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    /// <summary>
    /// MainMenu -> CourierFunctionsMenu 
    /// </summary>
    private static void CourierFunctionsMenu()
    {
        bool back = false;
        while (!back)
        {
            Console.WriteLine("\n--- Courier Functions Menu ---");
            Console.WriteLine("1. Log - In"); //should be forced to log in before using courier functions
            Console.WriteLine("2. Add Courier");
            Console.WriteLine("3. Show Courier by ID");
            Console.WriteLine("4. Show All Couriers");
            Console.WriteLine("5. Update Courier");
            Console.WriteLine("6. Delete Courier");
            Console.WriteLine("7. Number of Deliveries On Time for Courier");
            Console.WriteLine("8. Number of Deliveries Late for Courier");
            Console.WriteLine("9. Assign Delivery to Courier");
            Console.WriteLine("10. Close Deliveries for Courier");
            Console.WriteLine("0. Back");
            Console.Write("Choose: ");

            if (!Enum.TryParse(Console.ReadLine(), out BO.CourierMenuOption choice))
            {
                Console.WriteLine("Invalid choice!");
                continue;
            }
            switch (choice)
            {
                case BO.CourierMenuOption.LogIn:
                    // Implement Log In functionality
                    {
                        Console.Write("Enter the ID to log in: ");
                        if (!int.TryParse(Console.ReadLine(), out int id))
                        {
                            Console.WriteLine("Invalid id format.");
                            return;
                        }
                        string idString = id.ToString();
                        Console.WriteLine("Enter the password to log in: ");
                        string? password = Console.ReadLine() ?? throw new BO.BlInvalidInputException("Password cannot be null.");
                        BO.EnumUserRole User = s_bl.Courier.Login(idString,password);
                        Console.WriteLine($"Logged in as");
                    }
                    break;

                case BO.CourierMenuOption.AddCourier:
                    // Implement Add Courier functionality
                    Console.WriteLine("Enter your ID to check request: ");
                    if (!int.TryParse(Console.ReadLine(), out int askerId))
                    {
                        throw new BO.BlInvalidInputException("Invalid id format.");
                    }
                    //
                    //get info
                    DO.Courier? doCourier = new()
                    {
                        Id = boCourier.Id,
                        Name = boCourier.Name!,
                        CourierPhone = boCourier.CourierPhone!,
                        Email = boCourier.Email!,
                        Password = boCourier.Password!,
                        Active = boCourier.Active,
                        DeliveryMethod = (DO.EnumDeliveryMethod)boCourier.DeliveryMethod,
                        StartedWorking = DateTime.Now,
                        MaxPersonalDistance = boCourier.MaxPersonalDistance
                    };
                    BO.Courier newCourier= 
                    s_bl.Courier.Create(askerId, newCourier);
                    Console.WriteLine("Courier added.");

                    break;

                case BO.CourierMenuOption.ShowCourierById:
                    // Implement Show Courier by ID functionality
                    {
                        Console.Write("Enter courier ID to display: ");
                        if (!int.TryParse(Console.ReadLine(), out int id))
                        {
                            throw new BO.BlInvalidInputException("Invalid id format.");
                        }
                        var bo = s_bl.Courier.Read(id, id);
                        Console.WriteLine(bo);
                    }
                    break;
                case BO.CourierMenuOption.ShowListOfCouriers:
                    // Implement Show List of Couriers functionality
                    {
                        Console.WriteLine("Enter your ID to check request: ");
                        if (!int.TryParse(Console.ReadLine(), out int id))
                        {
                            throw new BO.BlInvalidInputException("Invalid id format.");
                        }
                        IEnumerable<BO.CourierInList>? list = s_bl.Courier.GetCouriersInList(id);
                        if (list == null)
                        {
                            return;
                        }
                        list.ToList().ForEach(Console.WriteLine);
                    }
                    break;
                case BO.CourierMenuOption.UpdateCourier:
                    // Implement Update Courier functionality
                    {
                        Console.WriteLine("Enter the ID of the requester: ");
                        if (!int.TryParse(Console.ReadLine(), out int idR))
                        {
                            throw new BO.BlInvalidInputException("Invalid id format.");
                        }
                        Console.Write("Enter ID of courier to update: ");
                        if (!int.TryParse(Console.ReadLine(), out int id))
                        {
                            throw new BO.BlInvalidInputException($"Invalid Courier ID={id} format");
                        }
                        BO.Courier existing = s_bl.Courier.Read(idR, id) ?? throw new BO.BlDoesNotExistException($"Courier with ID={id} does not exist.");
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
                        BO.EnumDeliveryMethod method = existing.DeliveryMethod;
                        if (!string.IsNullOrWhiteSpace(methodInput))
                            if (!Enum.TryParse(methodInput, true, out method))
                                method = existing.DeliveryMethod;

                        Console.Write($"Max Personal Distance ({existing.MaxPersonalDistance}): ");
                        string? maxDistInput = Console.ReadLine();
                        double? maxDist = existing.MaxPersonalDistance;
                        if (!string.IsNullOrWhiteSpace(maxDistInput) && double.TryParse(maxDistInput, out double d))
                            maxDist = d;
                        s_bl.Courier.Update(idR, new BO.Courier
                        {
                            Id = id,
                            Name = name,
                            CourierPhone = phone,
                            Email = email,
                            Password = password,
                            Active = existing.Active,
                            DeliveryMethod = method,
                            StartedWorking = existing.StartedWorking,
                            MaxPersonalDistance = maxDist,
                            TotalOnTimeDeliveries = existing.TotalOnTimeDeliveries,
                            TotalLateDeliveries = existing.TotalLateDeliveries,
                            ActiveDeliveryOrder = existing.ActiveDeliveryOrder
                        });
                    }
                    break;
                case BO.CourierMenuOption.DeleteCourier:
                    // Implement Delete Courier functionality
                    {
                        Console.WriteLine("Enter the ID of the requester: ");
                        if (!int.TryParse(Console.ReadLine(), out int idR))
                        {
                            throw new BO.BlInvalidInputException("Invalid id format.");
                        }
                        Console.Write("Enter ID of courier to delete: ");
                        if (!int.TryParse(Console.ReadLine(), out int id))
                        {
                            throw new BO.BlInvalidInputException($"Invalid Courier ID={id} format");
                        }
                        s_bl.Courier.Delete(idR, id);
                    }
                    break;
                case BO.CourierMenuOption.NumberOfDeliveriesOnTimeForCourier:
                    // Implement Number of Deliveries On Time for Courier functionality
                    break;
                case BO.CourierMenuOption.NumberOfDeliveriesLateForCourier:
                    // Implement Number of Deliveries Late for Courier functionality
                    break;
                case BO.CourierMenuOption.AssignDeliveryToCourier:
                    // Implement Assign Delivery to Courier functionality
                    break;
                case BO.CourierMenuOption.CloseDeliveriesForCourier:
                    // Implement Close Deliveries for Courier functionality
                    break;
                case BO.CourierMenuOption.Exit:
                    back = true;
                    break;
                default:
                    throw new BO.BlInvalidInputException($"Invalid option: {choice}");
            }
        }
    }

    private static void OrderFunctionsMenu()
    {
        bool back = false;
        while (!back)
        {
            Console.WriteLine("\n--- Order Functions Menu ---");
            Console.WriteLine("1. Create Order");
            Console.WriteLine("2. Read Order");
            Console.WriteLine("3. Show List Of Orders");
            Console.WriteLine("4. Update Order");
            Console.WriteLine("5. Delete Order");
            Console.WriteLine("6. Cancel Order");
            Console.WriteLine("7. Amount Of Order By Status");
            Console.WriteLine("8. End Order Status");
            Console.WriteLine("9. Create Delivery For Order");
            Console.WriteLine("10. Closed Deliveries In List To Courier");
            Console.WriteLine("11. List Of Open Orders To Choose");
            Console.WriteLine("0. Back");
            Console.Write("Choose: ");

            if (!Enum.TryParse(Console.ReadLine(), out BO.OrderMenuOptions choice))
            {
                Console.WriteLine("Invalid choice!");
                continue;
            }
            switch (choice)
            {
                case BO.OrderMenuOptions.AddOrder:
                    // Implement Create Order functionality
                    break;
                case BO.OrderMenuOptions.ShowOrderById:
                    // Implement Show Order by ID functionality
                    break;
                case BO.OrderMenuOptions.ShowListOfOrders:
                    // Implement Show List of Orders functionality
                    break;
                case BO.OrderMenuOptions.UpdateOrder:
                    // Implement Update Order functionality
                    break;
                case BO.OrderMenuOptions.DeleteOrder:
                    // Implement Delete Order functionality
                    break;
                case BO.OrderMenuOptions.CancelOrder:
                    // Implement Cancel Order functionality
                    break;
                case BO.OrderMenuOptions.AmountOfOrderByStatus:
                    // Implement 
                    break;
                case BO.OrderMenuOptions.EndOrderStatus:
                    // Implement 
                    break;
                case BO.OrderMenuOptions.CreateDeliveryForOrder:
                    // Implement 
                    break;
                case BO.OrderMenuOptions.ClosedDeliveriesInListToCourier:
                    // Implement 
                    break;
                case BO.OrderMenuOptions.ListOfOpenOrderToChoose:
                    // Implement 
                    break;
                case BO.OrderMenuOptions.Exit:
                    back = true;
                    break;
                default:
                    throw new BO.BlInvalidInputException($"Invalid option: {choice}");
            }
        }
    }

    private static void ConfigFunctionsMenu()
    {
        bool back = false;
        while(!back)
        {
            //MoveClock = 1,
            //GetClock = 2,
            //GetConfig = 3,
            //InitializeDB = 4,
            //ResetDB = 5,
            //SetConfig = 6
            Console.WriteLine("\n--- Config Functions Menu (Admin) ---");
            Console.WriteLine("1. Move Clock");
            Console.WriteLine("2. Get Clock");
            Console.WriteLine("3. Get Config");
            Console.WriteLine("4. Initialize DB");
            Console.WriteLine("5. Reset DB");
            Console.WriteLine("6. Set Config");
            Console.WriteLine("0. Back");

            if (!Enum.TryParse(Console.ReadLine(), out BO.ConfigMenuOptions choice))
            {
                Console.WriteLine("Invalid choice!");
                continue;
            }
            switch (choice)
            {
                case BO.ConfigMenuOptions.MoveClock:
                    //implement
                    break;
                    case BO.ConfigMenuOptions.GetClock:
                        //implement
                        break;
                    case BO.ConfigMenuOptions.GetConfig:
                        //implement
                        break;
                    case BO.ConfigMenuOptions.InitializeDB:
                        //implement
                        break;
                    case BO.ConfigMenuOptions.ResetDB:
                        //implement
                        break;
                    case BO.ConfigMenuOptions.SetConfig:
                        //implement
                        break;
                case BO.ConfigMenuOptions.Exit:
                    back = true;
                    break;
                default:
                    throw new BO.BlInvalidInputException($"Invalid option: {choice}");

            }

        }
    }
}


