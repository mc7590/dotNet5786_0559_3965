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
                        Console.WriteLine("Database and config reset successfully!");
                        break;

                    case MainMenuOption.Exit:
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
                private static void ResetAll()
                {
        if (s_dalCourier == null || s_dalOrder == null || s_dalDelivery == null || s_dalConfig == null)
        {
            throw new Exception ("Error: DAL not initialized yet!");
        }
         s_dalCourier.DeleteAll(); //stage 1
                    s_dalOrder.DeleteAll(); //stage 1
                    s_dalDelivery.DeleteAll(); //stage 1                
                    s_dalConfig.Reset(); //stage 1
                    Console.WriteLine("All data and config reset successfully!");

    }
}









