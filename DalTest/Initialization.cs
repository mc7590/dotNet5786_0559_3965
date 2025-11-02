//Initialization of DAL tests
namespace DalTest;
using DalApi;
using DO;

public static class Initialization
{
    private static ICourier? s_dalCourier; //stage 1
    private static IOrder? s_dalOrder; //stage 1
    private static IDelivery? s_dalDelivery; //stage 1
    private static IConfig? s_dalConfig; //stage 1

    /// <summary>
    /// Random number generator to initialize test data
    /// </summary>
    private static readonly Random s_rand = new();

    private static void createCouriers()
    {
        string[] courierNames =
    { "Dani Levy", "Eli Amar", "Yair Cohen", "Ariela Levin", "Dina Klein", "Shira Israelof", "Gigi Sadin", "Noa Tishbi", "Shimi Avner", "Neli Buhbot",
    "Avi Tirmon", "Kobi Maoz", "Mimi Asulin", "Shimshon Gibor", "Tirza Cohen", "Malca Bek", "Yael Lulu", "Erez G", "Gabi Gabot", "Ana Zak", "Dana Frider"};

        foreach (var name in courierNames)
        {
            int id;
            do
                id = s_rand.Next(200000000, 400000000);
            while (s_dalCourier!.Read(id) != null);

            string phone = $"+9725{s_rand.Next(10000000, 99999999)}";
            string email = $"{name.Replace(" ", "").ToLower()}@gmail.com";
            string password = $"password{id % 10000}";
            bool isActive = s_rand.NextDouble() < 0.8;
            EnumDeliveryMethod method = (EnumDeliveryMethod)s_rand.Next(0, 4);

            DateTime start = new DateTime(2018, 1, 1);
            int range = (s_dalConfig!.Clock - start).Days;
            DateTime startedworking = start.AddDays(s_rand.Next(range));

            double maxDistance = method switch
            {
                EnumDeliveryMethod.Foot => s_rand.Next(1, 2),  
                EnumDeliveryMethod.Bicycle => s_rand.Next(2, 5),    
                EnumDeliveryMethod.Motorcycle => s_rand.Next(3, 10), 
                EnumDeliveryMethod.Car => s_rand.Next(5, 25),    
                _ => s_rand.Next(2, 10)
            };

            s_dalCourier!.Create(new(id, name, phone, email, password, isActive, method, startedworking, maxDistance));
        }
    }
    private static void createOrders() { }
    private static void createDeliveries() {
        var allCouriers = s_dalCourier!.ReadAll().ToList();
        var availableOrders = s_dalOrder!.ReadAll().ToList();
        int NumOfDeliveries = 50;
        for (int i = 0; i < NumOfDeliveries && availableOrders.Count > 0; i++)
        {
            var orderIndex = s_rand.Next(availableOrders.Count);
            var order = availableOrders[orderIndex];
            availableOrders.RemoveAt(orderIndex);
            var courier = allCouriers[s_rand.Next(allCouriers.Count)];



            EnumEndDeliveryStatus endStatus = (EnumEndDeliveryStatus)s_rand.Next(0, 3);
            DateTime endDeliveryTime = s_dalConfig!.Clock.AddMinutes(s_rand.Next(30, 180));
            s_dalDelivery!.Create(new(0, order.Id, courier.Id, courier.DeliveryMethod, , , endStatus, endDeliveryTime));
        }
    }
}
