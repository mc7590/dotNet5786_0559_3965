using DalApi;
namespace Dal
{
    public sealed class DalXml : IDal //added "sealed" to prevent inheritance
    {
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
}
