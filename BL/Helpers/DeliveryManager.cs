//using BO;
using DalApi;
//using DO;
namespace Helpers;

internal static class DeliveryManager
{
    private static IDal s_dal = Factory.Get; //stage 4
    internal static ObserverManager Observers = new(); //stage 5 


    /// <summary>
    /// calculte the estimated delivery time based on delivery method and distance
    /// </summary>
    public static TimeSpan CalculateEstimatedDeliveryTime(DO.EnumDeliveryMethod method, double distanceInKm)
    {
        TimeSpan estimatedTime = method switch
        {
            DO.EnumDeliveryMethod.Car => TimeSpan.FromMinutes(distanceInKm / AdminManager.GetConfig().AveCarSpeedKmH),
            DO.EnumDeliveryMethod.Motorcycle => TimeSpan.FromMinutes(distanceInKm / AdminManager.GetConfig().AveMotorcycleSpeedKmH),
            DO.EnumDeliveryMethod.Bicycle => TimeSpan.FromMinutes(distanceInKm / AdminManager.GetConfig().AveBicycleSpeedKmH),
            DO.EnumDeliveryMethod.Foot => TimeSpan.FromMinutes(distanceInKm / AdminManager.GetConfig().AveWalkingSpeedKmH),
            _ => throw new BO.BlInvalidInputException("Invalid delivery method")
        };
        return estimatedTime;
    }
    ///// <summary>
    ///// calculates the order status based on deliveries associated with the order
    ///// </summary>
    //internal static BO.EnumOrderStatus CalculateOrderStatus(int OrderId)
    //{


    //    throw new NotImplementedException();
    //}

    /// <summary>
    /// Calculates the order status based on deliveries associated with the order
    /// </summary>
    /// <param name="id">The ID of the order.</param>
    /// <returns>The calculated BO.EnumOrderStatus.</returns>
    /// <exception cref="BO.ObjectNotFoundException">Thrown if the order ID is not found.</exception>
    internal static BO.EnumOrderStatus CalculateOrderStatus(int id)
    {
        // get the order from DAL
        IDal dal = Factory.Get;
        DO.Order order = dal.Order.Read(id) ?? throw new BO.BlDoesNotExistException($"Order with ID {id} does not exist.");

        IEnumerable<DO.Delivery> deliveries = dal.Delivery.ReadAll(d => d.OrderId == id);

        //case: finished delivery
        //if last delivery found that ended, that determines the order status
        DO.Delivery? lastDelivery = deliveries.OrderByDescending(d => d.EndDeliveryTime).FirstOrDefault(d => d.EndDeliveryTime != null);

        if (lastDelivery != null)
        {
            //order is closed, status determined by last delivery end status
            switch (lastDelivery.EndDeliveryStatus)
            {
                case DO.EnumEndDeliveryStatus.Delivered:
                    return BO.EnumOrderStatus.Delivered;

                case DO.EnumEndDeliveryStatus.RefusedToReceive:
                    return BO.EnumOrderStatus.CustomerRefused;

                case DO.EnumEndDeliveryStatus.Canceled:
                    return BO.EnumOrderStatus.Canceled;

                default:
                    return BO.EnumOrderStatus.Canceled;
            }
        }

        //check for active delivery (not ended yet)

        //if no finished delivery found, check for active delivery (not yet ended)
        DO.Delivery? activeDelivery = deliveries.FirstOrDefault(d => d.EndDeliveryTime == null);

        if (activeDelivery != null)
        {
            //in progress - there is an active delivery that has not yet ended
            return BO.EnumOrderStatus.InProgress;
        }

        //default: opened

        //no finished delivery and no active delivery: the order is open
        return BO.EnumOrderStatus.Open;
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
        return s_dal.Delivery.ReadAll(d => d.OrderId == orderId)/*.Where(delivery => delivery.OrderId == orderId)*/
            .Select(delivery => new BO.DeliveryPerOrderInList()
            {
                DeliveryId = delivery.Id,
                CourierId = delivery.CourierId,
                CourierName = s_dal.Courier.Read(delivery.CourierId)?.Name ?? null,
                DeliveryMethod = (BO.EnumDeliveryMethod)delivery.DeliveryMethod,
                DelCreationTime = delivery.DeliveryStartTime,
                EndDeliveryStatus = (BO.EnumEndDeliveryStatus?)delivery.EndDeliveryStatus,
                EndDeliveryTime = delivery.EndDeliveryTime
            }).ToList(); //leave the "tolist" because there in no use of foreach when func is called
    }


    /// <summary>
    /// get the schedule status of the given DO order
    /// </summary>
    internal static BO.EnumScheduleStatus GetScheduleStatus(DO.Order doOrder)
    {
        TimeSpan timePass = doOrder.OrderCreationTime - AdminManager.Now;
        if (timePass > AdminManager.GetConfig().GetMaxDeliveryTime)
            return BO.EnumScheduleStatus.Late;
        if (timePass > AdminManager.GetConfig().RiskRange)
            return BO.EnumScheduleStatus.InRisk;
        return BO.EnumScheduleStatus.OnTime;
    }
    
    internal static TimeSpan GetTotalDeliveryTime(int orderId, DateTime? end)
    {
        DO.Order order = s_dal.Order.Read(orderId) ?? throw new BO.BlDoesNotExistException($"Order with ID={orderId} does Not exist");
        return order.OrderCreationTime - end ?? throw new BO.BlInvalidInputException("Delivery is not yet closed");
    }
    /// <summary>
    /// checks if the given delivery was delivered on time or not
    /// </summary>
    private static bool IsDeliveredOnTime(DO.Delivery d)
    {
        TimeSpan totalTime = (d.EndDeliveryTime ?? DateTime.Now) - d.DeliveryStartTime;
        TimeSpan maxTime = AdminManager.GetConfig().GetMaxDeliveryTime;
        return totalTime <= maxTime;
    }
    /// <summary>
    /// gets the number of late deliveries for a specific courier
    /// </summary>
    public static int GetDeliverierLateForCourier(int id, int courierId)
    {
        Tools.IsManagerOrCourier(id, courierId);
        IEnumerable<DO.Delivery> deliveries = s_dal.Delivery.ReadAll(d => d.CourierId == courierId);
        return deliveries.Count(d => IsDeliveredOnTime(d));
    }
    /// <summary>
    /// gets the number of on-time deliveries for a specific courier
    /// </summary>
    public static int GetDeliverierOnTimeForCourier(int id, int courierId)
    {
        Tools.IsManagerOrCourier(id, courierId);
        IEnumerable<DO.Delivery> deliveries = s_dal.Delivery.ReadAll(d => d.CourierId == courierId);
        return deliveries.Count(d => !IsDeliveredOnTime(d));
    }

    /// <summary>
    /// get the closed deliveries in list for a specific courier with optional filtering and sorting
    /// </summary>
    internal static IEnumerable<BO.ClosedDeliveryInList> GetClosedDeliveriesInListsToCourier(int id ,int courierId ,BO.EnumOrderType? typeFilter = null ,BO.EnumClosedDeliveryInListField? sortBy = null)
    {
        //check if the person asking is a manager or the courier assigned to the delivery
        Tools.IsManagerOrCourier(id, courierId);

        //get all closed deliveries for the given courier 
        IEnumerable<DO.Delivery> deliveries = s_dal.Delivery.ReadAll(d => d.CourierId == courierId && d.EndDeliveryTime != null) ?? throw new BO.BlDoesNotExistException($"No closed deliveries found for courier with ID={courierId}");

        //filter
        if (typeFilter != null)
        {
            deliveries = deliveries.Where(d => s_dal.Order.Read(d.OrderId)!.OrderType == (DO.EnumOrderType)typeFilter);
        }

        //sort
        deliveries = sortBy switch
        {
            BO.EnumClosedDeliveryInListField.DeliveryId => deliveries.OrderBy(d => d.Id),
            BO.EnumClosedDeliveryInListField.OrderId => deliveries.OrderBy(d => d.OrderId),
            BO.EnumClosedDeliveryInListField.OrderType => deliveries.OrderBy(d => s_dal.Order.Read(d.OrderId)!.OrderType),
            BO.EnumClosedDeliveryInListField.Address => deliveries.OrderBy(d => s_dal.Order.Read(d.OrderId)!.Address),
            BO.EnumClosedDeliveryInListField.DeliveryMethod => deliveries.OrderBy(d => d.DeliveryMethod),
            BO.EnumClosedDeliveryInListField.DistanceInKm => deliveries.OrderBy(d => Tools.CalculateDistanceInKm(s_dal.Order.Read(d.OrderId)!.Longitude, s_dal.Order.Read(d.OrderId)!.Latitude)),
            BO.EnumClosedDeliveryInListField.TotalDeliveryTime => deliveries.OrderBy(d => d.EndDeliveryTime - d.DeliveryStartTime),
            _ => deliveries.OrderBy(d => d.EndDeliveryStatus) //default sorting by end delivery status (all deliveries here are closed)
        };

        return deliveries.Select(d => new BO.ClosedDeliveryInList
        {
            DeliveryId = d.Id,
            OrderId = d.OrderId,
            OrderType = (BO.EnumOrderType)s_dal.Order.Read(d.OrderId)!.OrderType,
            Address = s_dal.Order.Read(d.OrderId)!.Address ?? null,
            DeliveryMethod = (BO.EnumDeliveryMethod)d.DeliveryMethod,
            DistanceInKm = Tools.CalculateDistanceInKm(s_dal.Order.Read(d.OrderId)!.Longitude, s_dal.Order.Read(d.OrderId)!.Latitude),
            TotalDeliveryTime = GetTotalDeliveryTime(d.OrderId, d.EndDeliveryTime),
            EndDeliveryStatus = (BO.EnumEndDeliveryStatus)d.EndDeliveryStatus!
        });
    }

    /// <summary>
    /// make delivery end for given order and delivery IDs
    /// </summary>
    /// <param name="id">id of person asking data</param>
    /// <param name="courierId">order id</param>
    /// <param name="deliveryId">delivery id to be ended</param>
    internal static void EndOrderStatus(int id, int courierId, int deliveryId)
    {
        //check if the person asking is a manager or the courier assigned to the delivery
            //int? nullableCourierId = s_dal.Delivery.Read(deliveryId)!.CourierId;
            //int courierId = nullableCourierId  ?? throw new BO.BlDoesNotExistException($"Delivery with ID={deliveryId} does Not exist");
        Tools.IsManagerOrCourier(id, s_dal.Delivery.Read(deliveryId)!.CourierId);

        //check if the order exists
        if (s_dal.Courier.Read(courierId) == null)
            throw new BO.BlDoesNotExistException($"Order with ID={courierId} does Not exist");

        //try to update the delivery end status and time
        DO.Delivery? delivery = s_dal.Delivery.Read(d => d.Id == deliveryId && d.EndDeliveryStatus == null) ?? throw new BO.BlDoesNotExistException($"Delivery with ID={deliveryId} does Not exist");
        DeliveryManager.UpdateDelivery(delivery, BO.EnumEndDeliveryStatus.Delivered, AdminManager.Now);

    }

    /// <summary>
    /// updates the delivery with given ID to new status and end time
    /// </summary>
    internal static void UpdateDelivery(DO.Delivery? delivery, BO.EnumEndDeliveryStatus newStatus, DateTime endTime)
    {
        if (delivery != null)
        {
            var updatedDelivery = delivery with //create delivery copy by updated delivery
            {
                EndDeliveryStatus = (DO.EnumEndDeliveryStatus)newStatus,
                EndDeliveryTime = endTime
            };
            s_dal.Delivery.Update(updatedDelivery);

            Observers.NotifyItemUpdated(updatedDelivery.Id); //stage 5
            Observers.NotifyListUpdated();  //stage 5

        }
    }


}
