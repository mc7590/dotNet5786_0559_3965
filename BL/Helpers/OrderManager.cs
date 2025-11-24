using DalApi;

namespace Helpers;

internal static class OrderManager
{
    private static IDal s_dal = Factory.Get; //stage 4

    internal static BO.Order? GetOrder(int id)
    {
        DO.Order doOrder;
        doOrder = s_dal.Order.Read(id) ?? throw new BO.BlDoesNotExistException($"Order with ID={id} does Not exist");

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

