

//using BO;
//using DO;

using BO;

namespace BlTest;

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
                        throw new BlInvalidInputException($"Invalid option: {choice}");
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
                    break;
                case BO.CourierMenuOption.AddCourier:
                    // Implement Add Courier functionality
                    break;
                case BO.CourierMenuOption.ShowCourierById:
                    // Implement Show Courier by ID functionality
                    break;
                case BO.CourierMenuOption.ShowListOfCouriers:
                    // Implement Show List of Couriers functionality
                    break;
                case BO.CourierMenuOption.UpdateCourier:
                    // Implement Update Courier functionality
                    break;
                case BO.CourierMenuOption.DeleteCourier:
                    // Implement Delete Courier functionality
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
            //        MoveClock = 1,
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


