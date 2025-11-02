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

    /// <summary>
    /// Creates initial data for couriers in the DAL
    /// </summary>
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

    /// <summary>
    /// Creates initial data for orders in the DAL
    /// </summary>
    private static void createOrders()
    {
        string[] customerNames = {
    "Dana Levi", "Eli Cohen", "Noa Friedman", "Itay Mor", "Rina Katz", "Gadi Azulay",
    "Hadas Haim", "Miri Shaked", "Avi Weis", "Gadi Shapiro", "Meni Chazan", "Gil Berger",
    "Tzila Haimov", "Avichai Shay", "Gili Wert", "Batya David",
    "Lior Ben Ami", "Orly Tal", "Yael Nissim", "Eitan Gold", "Tamar Ben Haim", "Nadav Peretz",
    "Sharon Azulai", "Hila Dahan", "Yoni Levi", "Maya Green", "Rafi Malka", "Noam Hadad",
    "Roni Meir", "Galit Cohen" };//30 customer names

        (string address, double latitude, double longitude)[] addresses = new (string, double, double)[]{
    ("Herzl 12, Ramat Gan",32.08140468378999, 34.81846994729164),
    ("Jabotinsky 45, Ramat Gan",32.08462812649096, 34.80891584729142),
    ("Bialik 7, Ramat Gan",32.079904161610465, 34.814772647291655),
    ("Arlozorov 28, Ramat Gan", 32.08058627022238, 34.81255700496421),
    ("David Ben Gurion 65, Bnei Brak", 32.08691883312563, 34.82261950496387),
    ("Haroe 3, Ramat Gan", 32.08405363711667, 34.815957204964114),
    ("HaYarden 14, Ramat Gan", 32.06880762534448, 34.82837354729227),
    ("Avishai 3, Ramat Gan", 32.07185417121981, 34.823270562637234),
    ("Krinitsi 19, Ramat Gan", 32.079423624950415, 34.81589141845542),
    ("Etsel 50, Ramat Gan", 32.06904704146371, 34.83745017798244),
    ("Moshe Sharet 20, Ramat Gan", 32.08564253490335, 34.81832257427386),
    ("Struma 9, Ramat Gan", 32.09082351991918, 34.81605910295885),
    ("Zabotinsky 101, Ramat Gan", 32.08941279874944, 34.81519096263632),
    ("Truman 30, Ramat Gan", 32.06820334280617, 34.82599864729232),
    ("Tel Hai 90, Ramat Gan", 32.063884680730965, 34.82634966078368),
    ("Rabbi Akiva 90, Bnei Brak", 32.08611587414736, 34.831141189618805),
    ("HaShomer 12, Bnei Brak", 32.081261659989735, 34.822311847291665),
    ("Chazon Ish 23, Bnei Brak", 32.08289859666585, 34.83566446263663),
    ("Avnei Nezer 7, Bnei Brak", 32.08369969868215, 34.841965933800296),
    ("Rashi 5, Bnei Brak", 32.085374145178065, 34.835374647291395),
    ("Yerushalayim 8, Bnei Brak", 32.08689017337688, 34.829995418455134),
    ("Ben Ya'akov 31, Bnei Brak", 32.07717658312238, 34.84103852030936),
    ("Rabbi Akiva 102, Bnei Brak", 32.08595061456101, 34.83275906263643),
    ("Hazon Ish 47, Bnei Brak", 32.080594241935124, 34.8347143779818),
    ("Mivtza Kadesh 64, Bnei Brak", 32.10103608222269, 34.83050512030826),
    ("Hashomer 13, Bnei Brak", 32.08150664066434, 34.82259351845536),
    ("David Hamelech 2, Bnei Brak", 32.08356507949177, 34.82652508961899),
    ("Daniel 13, Bnei Brak", 32.081340040896386, 34.82687240496421),
    ("Shma'aya 4, Bnei Brak", 32.088972920006405, 34.832547504963806),
    ("HaRav Desler 19, Bnei Brak", 32.082221820496564, 34.83390117798174) };//50 addresses + coordinates

        string[] phones = {
    "0501234567", "0529876543", "0531112233", "0544445555", "0557778888",
    "0507654321", "0523456789", "0532223344", "0545566778", "0551122334",
    "0509988776", "0526677889", "0534455667", "0542233445", "0558899001",
    "0503216549", "0527891234", "0538904567", "0545670987", "0553456780",
    "0506789012", "0521011121", "0537654321", "0549998887", "0554321098",
    "0505556667", "0524443332", "0531110099", "0548080706", "0559090807" }; //30 phone numbers

        string[] descriptions = {"Online order through Burgeranch app",
    "Delivery to office during lunch hours", "Pickup order for two burgers and fries",
    "Late-night delivery request", "Customer asked for extra sauces",
    "Family meal ordered for dinner", "Corporate lunch catering",
    "Special order for birthday event", "Repeat customer – weekly order",
    "Express delivery to nearby office", "Customer requested no onions",
    "Phone order confirmed by manager", "New customer – first time order",
    "Should be quick", "Extra napkins requested",
    "Delivery with contactless option", "Regular customer – same address",
    "Office team order – 4 meals", "Prepaid order via credit card",
    "Customer requested extra ketchup", "Gift meal for a friend",
    "Takeaway order from the counter", "App order with discount coupon",
    "Customer added a special note", "Delivery to school event",
    "Quick lunch order for employee", "Late evening order – high priority",
    "Requested gluten-free bun", "Customer ordered using loyalty points",
    "Repeat Friday lunch order", "Pickup order – ready in 15 minutes",
    "Delivery to 3rd floor apartment", "Express delivery before closing time",
    "Meal ordered via phone call", "Customer requested a call before delivery",
    "Special order with side salad", "Delivery for office meeting",
    "Burgeranch club member order", "Customer asked to add utensils",
    "Order confirmed via SMS", "Family order for movie night",
    "Delivery to coworking space", "Lunch break order for two",
    "No-contact delivery requested", "Customer used birthday coupon",
    "Pickup by courier service", "Small lunch order – single meal",
    "Evening order for three friends", "Slippery road at the address",
    "VIP customer – monthly order", "Delivery with cold drink request" }; //50 descriptions


        int totalOrders = 50; //number of orders to create
        for (int i = 0; i < totalOrders; i++)
        {
            string name = customerNames[s_rand.Next(customerNames.Length)];

            int index = s_rand.Next(addresses.Length);
            string address = addresses[index].address;
            double latitude = addresses[index].latitude;
            double longitude = addresses[index].longitude;

            string phone = phones[s_rand.Next(phones.Length)];
            string description = descriptions[s_rand.Next(descriptions.Length)];
            EnumOrderType type = (EnumOrderType)s_rand.Next(0, 3); //random 0,1,2

            //order times according to wanted statuses
            DateTime now = s_dalConfig!.Clock;
            DateTime orderCreation;
            int chance = s_rand.Next(100);
            if (chance < 40) //40% opened orders
                orderCreation = now.AddMinutes(-s_rand.Next(0, 120)); //order opened in prior range of 2 hours
            else if (chance < 60) //20% currently handled orders
                orderCreation = now.AddHours(-s_rand.Next(2, 6)); //order opened in prior range of 2-6 hours
            else //40% closed orders
                orderCreation = now.AddDays(-s_rand.Next(1, 60)); //order made in the past 2 months

            double? weight = Math.Round(s_rand.NextDouble() * 5 + 0.5, 2); //order weight 0.5-5.5 kg.(Round 2 digits after decimal point)
            bool? fragile = s_rand.Next(100) < 15 ? true : false; //15% chance to be fragile


            s_dalOrder!.Create(new(0, type, description, address, latitude, longitude, name, phone, orderCreation, weight, fragile));
        }

    }

    /// <summary>
    /// Creates initial data for deliveries in the DAL
    /// </summary>
    private static void createDeliveries()
    {
        var allCouriers = s_dalCourier!.ReadAll().ToList();
        var availableOrders = s_dalOrder!.ReadAll().ToList();

        int NumOfDeliveries = 50;
        for (int i = 0; i < NumOfDeliveries && availableOrders.Count > 0; i++)
        {

            var orderIndex = s_rand.Next(availableOrders.Count);
            var order = availableOrders[orderIndex];
            availableOrders.RemoveAt(orderIndex);
            var courier = allCouriers[s_rand.Next(allCouriers.Count)];

            //var DeliveryStartTime = GenerateDeliveryStart(***);
            //double? distanceInKm = 

            EnumEndDeliveryStatus endStatus = (EnumEndDeliveryStatus)s_rand.Next(0, 3);//maybe not random bc depens on starting date?
            DateTime endDeliveryTime = s_dalConfig!.Clock.AddMinutes(s_rand.Next(30, 180));
            s_dalDelivery!.Create(new(0, order.Id, courier.Id, courier.DeliveryMethod, DeliveryStartTime, , endStatus, endDeliveryTime));
        }
    }


    ///// <summary>
    ///// generate non-overlapping delivery start time
    ///// </summary>
    //private static DateTime GenerateDeliveryStart(ICourier courier, Order order, int estimatedMinutes)
    //{
    //    DateTime earliestStart = order.OrderCreationTime.AddMinutes(5);
    //    DateTime latestStart = s_dalConfig!.Clock;
    //    DateTime start;

    //    do
    //    {
    //        start = earliestStart.AddMinutes(s_rand.Next((int)(latestStart - earliestStart).TotalMinutes));

    //    } while (s_dalDelivery!.ReadAll()
    //                .Where(d => d.CourierId == courier.Id)
    //                .Any(d => start < d.DeliveryEndTime && start.AddMinutes(estimatedMinutes) > d.DeliveryStartTime));

    //    return start;
    //}

}

