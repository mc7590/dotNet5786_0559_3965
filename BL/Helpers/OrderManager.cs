using DalApi;


namespace Helpers;

internal static class OrderManager
{
    private static IDal s_dal = Factory.Get; //stage 4

    internal static void createOrder(int id, BO.Order boOrder)
    {
        Tools.isManager(id);
        Tools.isValidAddress(boOrder.Address); // Check if Address is valid
        //Tools.Location? location = Tools.GetLocationOfAddressSync(boOrder.Address);

        DO.Order doOrder = new DO.Order()//????
        {
            Id = boOrder.Id,
            OrderType = (DO.EnumOrderType)boOrder.OrderType,
            Description = boOrder.Description,
            Address = boOrder.Address??"",
            Latitude = boOrder.Latitude,
            Longitude = boOrder.Longitude,
            CustomerName = boOrder.CustomerName ?? "",
            CustomerPhone = boOrder.CustomerPhone ?? "",
            Weight = boOrder.Weight,
            Fragile = boOrder.Fragile,
            OrderCreationTime = boOrder.CreationTime,
        };

        s_dal.Order.Create(doOrder);
    }

    internal static BO.Order? GetOrder(int id, int orderId)
    {
        isManager(id);

        DO.Order doOrder;
        doOrder = s_dal.Order.Read(orderId) ?? throw new BO.BlDoesNotExistException($"Order with ID={orderId} does Not exist");

        return new BO.Order()
        {
            Id = id,
            OrderType = (BO.EnumOrderType)doOrder.OrderType,
            Description = doOrder.Description,
            Address = doOrder.Address,
            Latitude = doOrder.Latitude,
            Longitude = doOrder.Longitude,
            AerialDistance = GetOrderAerialDistance(doOrder.Address),
            CustomerName = doOrder.CustomerName,
            CustomerPhone = doOrder.CustomerPhone,
            Weight = doOrder.Weight,
            Fragile = doOrder.Fragile,
            CreationTime = doOrder.OrderCreationTime,
            ExpectedDeliveryTime = GetOrderExpectedDeliveryTime(doOrder.OrderCreationTime),

        };
    }

}

