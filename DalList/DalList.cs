namespace Dal;
using DalApi;

sealed public class DalList : IDal
{
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
