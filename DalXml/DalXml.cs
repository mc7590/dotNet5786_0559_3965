using DalApi;
using System.Diagnostics;
namespace Dal;

sealed internal class DalXml : IDal //sealed - prevents inheritance to maintain the Singleton principle.
{
    /// <summary>
    ///Private field using Lazy<T> to defer DalXml object creation until first request.
    /// </summary>
    private static readonly Lazy<DalXml> lazyInstance = new Lazy<DalXml>(() => new DalXml());//stage 4 //Lazy Initialization
    /// <summary>
    ///Public static access point. Gets the instance, creating it safely on first access (Lazy and Thread Safe).
    /// </summary>
    public static IDal Instance => lazyInstance.Value;//stage 4 //Thread Safe
    private DalXml() { }//stage 4

    public ICourier Courier { get; } = new CourierImplementation();
    public IOrder Order { get; } = new OrderImplementation();
    public IDelivery Delivery { get; } = new DeliveryImplementation();
    public IConfig Config { get; } = new ConfigImplementation();

    public void ResetDB()
    {
        
        Courier.DeleteAll();
        Console.WriteLine("Deleting Couriers...");
        Order.DeleteAll();
        Console.WriteLine("Deleting Orders...");
        Delivery.DeleteAll();
        Console.WriteLine("Deleting Deliveries...");
        Config.Reset();
        Console.WriteLine("Resetting Config...");
    }
}
