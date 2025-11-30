using DalApi;
namespace Helpers;

internal static class DeliveryManager
{
    private static IDal s_dal = Factory.Get; //stage 4

    internal static DateTime? CalculateExpectedDeliveryTime(int id)
    {
        throw new NotImplementedException();
    }

    internal static BO.EnumOrderStatus CalculateOrderStatus(int id)
    {
        throw new NotImplementedException();
    }

    internal static DO.Delivery CreateDemeDelivery(int orderId)
    {
        return new DO.Delivery
        {
            Id = 0,
            OrderId = orderId,
            CourierId = 0,
            DeliveryMethod = DO.EnumDeliveryMethod.Car, //default value
            DeliveryStartTime = AdminManager.Now,
            EndDeliveryStatus = DO.EnumEndDeliveryStatus.Canceled,
            EndDeliveryTime = AdminManager.Now
        };
    }


    /// <summary>
    /// returns the courier ID that delivered the given BO order
    /// </summary>
    internal static int? GetCourierIdToBoOrder(BO.Order boOrder)
    {
        //get list of deliveries for the given order ID
        var delList = GetListDeliveryPerOrderInList(boOrder.Id);

        // Find the object where the time difference (boOrder.CreationTime - delivery.DelCreationTime) is minimal
        // OrderBy-> calculates the time difference for each item and sorts from smallest to largest
        // FirstOrDefault-> returns the first item in the sorted list (the item with the min time difference)
        var closestDelivery = delList.OrderBy(delivery => boOrder.CreationTime - delivery.DelCreationTime)
                                     .FirstOrDefault();

        // Return the CourierId of the found delivery (or null if no delivery was found)
        return closestDelivery?.CourierId;
    }

    /// <summary>
    /// returns a list of DeliveryPerOrderInList for the given BO order ID
    /// </summary>
    internal static IEnumerable<BO.DeliveryPerOrderInList> GetListDeliveryPerOrderInList(int orderId)
    {
        return s_dal.Delivery.ReadAll().Where(delivery => delivery.OrderId == orderId)
            .Select(delivery => new BO.DeliveryPerOrderInList()
            {
                DeliveryId = delivery.Id,
                CourierId = delivery.CourierId,
                CourierName = s_dal.Courier.Read(delivery.CourierId)!.Name,
                DeliveryMethod = (BO.EnumDeliveryMethod)delivery.DeliveryMethod,
                DelCreationTime = delivery.DeliveryStartTime,
                EndDeliveryStatus = (BO.EnumEndDeliveryStatus)delivery.EndDeliveryStatus!,
                EndDeliveryTime = delivery.EndDeliveryTime
            }).ToList();
    }


    internal static BO.EnumScheduleStatus GetScheduleStatus(DO.Order doOrder)
    {
        throw new NotImplementedException();
    }

    internal static TimeSpan GetTotalDeliveryTime(int orderId)
    {
        // look at function OrderManager.BoOrderToBoOrderInList
    }
    public static int GetDeliverierLateForCourier(int id, int courierId)
    {
        Tools.IsManagerOrCourier(id, courierId);
        IEnumerable<DO.Delivery> deliveries = s_dal.Delivery.ReadAll(d => d.CourierId == courierId);
        return deliveries.Count(d => (d.DeliveryStartTime - d.EndDeliveryTime) > AdminManager.GetConfig().GetMaxDeliveryTime);
    }

    public static int GetDeliverierOnTimeForCourier(int id, int courierId)
    {
        Tools.IsManagerOrCourier(id, courierId);
        IEnumerable<DO.Delivery> deliveries = s_dal.Delivery.ReadAll(d => d.CourierId == courierId);
        return deliveries.Count(d => (d.DeliveryStartTime - d.EndDeliveryTime) <= AdminManager.GetConfig().GetMaxDeliveryTime);
    }
    /// <summary>
    /// gets the closed deliveries in list for a specific courier with optional filtering and sorting
    /// </summary>
    public static IEnumerable<BO.ClosedDeliveryInList> GetClosedDeliveriesInListsToCourier(int Id ,int courierId ,BO.EnumOrderType? typeFilter = null ,BO.EnumClosedDeliveryInListField? sortBy = null)
    {
        Tools.IsManagerOrCourier(Id, courierId);
        var deliveries = s_dal.Delivery.ReadAll(d => d.CourierId == courierId && d.EndDeliveryTime != null);
        if (typeFilter != null)
        {
            deliveries = deliveries.Where(d => s_dal.Order.Read(d.OrderId)!.OrderType == (DO.EnumOrderType)typeFilter);
        }
        deliveries = sortBy switch
        {
            BO.EnumClosedDeliveryInListField.DeliveryId => deliveries.OrderBy(d => d.Id),
            BO.EnumClosedDeliveryInListField.OrderId => deliveries.OrderBy(d => d.OrderId),
            BO.EnumClosedDeliveryInListField.Address => deliveries.OrderBy(d => s_dal.Order.Read(d.OrderId)!.Address),
            BO.EnumClosedDeliveryInListField.DistanceInKm => deliveries.OrderBy(d => Tools.CalculateDistanceInKm(s_dal.Order.Read(d.OrderId)!.Longitude, s_dal.Order.Read(d.OrderId)!.Latitude)),
            BO.EnumClosedDeliveryInListField.TotalDeliveryTime => deliveries.OrderBy(d => d.EndDeliveryTime - d.DeliveryStartTime),
            _ => deliveries
        };
        return deliveries.Select(d => new BO.ClosedDeliveryInList
        {
            DeliveryId = d.Id,
            OrderId = d.OrderId,
            OrderType = (BO.EnumOrderType)s_dal.Order.Read(d.OrderId)!.OrderType,
            Address = s_dal.Order.Read(d.OrderId)!.Address ?? null,
            DeliveryMethod = (BO.EnumDeliveryMethod)d.DeliveryMethod,
            DistanceInKm = Tools.CalculateDistanceInKm(s_dal.Order.Read(d.OrderId)!.Longitude, s_dal.Order.Read(d.OrderId)!.Latitude),
            TotalDeliveryTime = GetTotalDeliveryTime(d.OrderId),
            EndDeliveryStatus = 
        });
    }
}
