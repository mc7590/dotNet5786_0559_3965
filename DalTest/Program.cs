using Dal;
using DalApi;

namespace DalTest;


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
           //main...




        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    

    
}