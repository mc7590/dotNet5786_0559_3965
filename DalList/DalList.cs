namespace Dal;
using DalApi;

sealed internal class DalList : IDal //sealed - prevents inheritance to maintain the Singleton principle.
{
    //This Eagerly initializes the single instance upon class loading. (Not in use here)
    //public static IDal Instance { get; } = new DalList();//stage 4

    /// <summary>
    ///Private field using Lazy<T> to defer DalList object creation until first request.
    /// </summary>
    private static readonly Lazy<DalList> lazyInstance = new Lazy<DalList>(() => new DalList());//stage 4 //Lazy Initialization
    /// <summary>
    ///Public static access point. Gets the instance, creating it safely on first access (Lazy and Thread Safe).
    /// </summary>
    public static IDal Instance => lazyInstance.Value;//stage 4 //Thread Safe
    private DalList() { }//stage 4

    public ICourier Courier { get; } = new CourierImplementation();
    public IOrder Order { get; } = new OrderImplementation();
    public IDelivery Delivery { get; } = new DeliveryImplementation();
    public IConfig Config { get; } = new ConfigImplementation();

    public void ResetDB()
    {
        Console.WriteLine("Deleting Couriers...");
        Courier.DeleteAll(); 
        Console.WriteLine("Deleting Orders...");
        Order.DeleteAll();
        Console.WriteLine("Deleting Deliveries...");
        Delivery.DeleteAll();
        Console.WriteLine("Resetting Config...");
        Config.Reset();
    }
}
