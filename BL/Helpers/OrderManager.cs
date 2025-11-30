using BO;
using DalApi;


namespace Helpers;

internal static class OrderManager
{
    private static readonly IDal s_dal = Factory.Get; //stage 4

    /// <summary>
    /// Converts a BO Order to a DO Order
    /// </summary>
    internal static DO.Order BoOrderToDoOrder(BO.Order boOrder)
    {
        return new DO.Order()
        {
            Id = boOrder.Id,
            OrderType = (DO.EnumOrderType)boOrder.OrderType,
            Description = boOrder.Description,
            Address = boOrder.Address!,
            Latitude = boOrder.Latitude,
            Longitude = boOrder.Longitude,
            CustomerName = boOrder.CustomerName!,
            CustomerPhone = boOrder.CustomerPhone!,
            OrderCreationTime = boOrder.CreationTime,
            Weight = boOrder.Weight,
            Fragile = boOrder.Fragile
        };

    }

    /// <summary>
    /// Converts a DO Order to a BO Order
    /// </summary>
    internal static BO.Order DoOrderToBoOrder(DO.Order doOrder) => new BO.Order()
    {
        Id = doOrder.Id,
        OrderType = (BO.EnumOrderType)doOrder.OrderType,
        Description = doOrder.Description,
        Address = doOrder.Address,
        Latitude = doOrder.Latitude,
        Longitude = doOrder.Longitude,
        AerialDistance = Tools.CalculateAerialDistance(doOrder.Longitude, doOrder.Latitude),
        CustomerName = doOrder.CustomerName,
        CustomerPhone = doOrder.CustomerPhone,
        Weight = doOrder.Weight,
        Fragile = doOrder.Fragile,
        CreationTime = doOrder.OrderCreationTime,
        ExpectedDeliveryTime = DeliveryManager.CalculateExpectedDeliveryTime(doOrder.Id),
        MaxDeliveryTime = doOrder.OrderCreationTime + AdminManager.GetConfig().GetMaxDeliveryTime,
        OrderStatus = DeliveryManager.CalculateOrderStatus(doOrder.Id),
        ScheduleStatus = DeliveryManager.GetScheduleStatus(doOrder),
        RemainingTime = DeliveryManager.GetRemainingTime(doOrder),
        OrderDelivHist = DeliveryManager.GetListDeliveryPerOrderInList(doOrder.Id)
    };


    /// <summary>
    /// Creates a new order in DAL
    /// </summary>
    internal static void CreateOrder(int id, BO.Order boOrder)
    {
        Tools.IsManager(id);
        Tools.IsValidAddress(boOrder.Address); // Check if Address is valid
        Tools.IsValidName(boOrder.CustomerName);
        Tools.IsValidPhone(boOrder.CustomerPhone);

        DO.Order doOrder = new DO.Order()
        {
            Id = boOrder.Id,
            OrderType = (DO.EnumOrderType)boOrder.OrderType,
            Description = boOrder.Description,
            Address = boOrder.Address!,
            Latitude = boOrder.Latitude,
            Longitude = boOrder.Longitude,
            CustomerName = boOrder.CustomerName!,
            CustomerPhone = boOrder.CustomerPhone!,
            Weight = boOrder.Weight,
            Fragile = boOrder.Fragile,
            OrderCreationTime = boOrder.CreationTime,
        };

        s_dal.Order.Create(doOrder);
    }

    /// <summary>
    /// Gets an order by its ID
    /// </summary>
    internal static BO.Order? GetOrder(int id, int orderId)
    {
        Tools.IsManager(id);

        DO.Order doOrder;
        doOrder = s_dal.Order.Read(orderId) ?? throw new BO.BlDoesNotExistException($"Order with ID={orderId} does Not exist");
        return DoOrderToBoOrder(doOrder);
    }

    /// <summary>
    /// Throws exception that orders cannot be deleted
    /// </summary>
    internal static void DeleteOrder(int id, int orderId)
    {
        Tools.IsManager(id);
        throw new BO.BlInvalidOperationException("Orders cannot be deleted");
    }

    /// <summary>
    /// Return a list of orders in a summarized format, with optional sorting and filtering.
    /// </summary>
    /// <param name="id">id of the person asking the data</param>
    /// <param name="filter">if null, no filter, else: filter by: if(enumFilter == value)</param>
    /// <param name="value">the value to use for filter, if null, sort by OrderStatus </param>
    /// <param name="sort">if (value!=null), sort by the Enum option for sort</param>
    internal static IEnumerable<BO.OrderInList> GetOrderInList(int id, BO.EnumOrderFieldSort? filter = null, object? value = null, BO.EnumOrderFieldSort? sort = null)
    {
        Tools.IsManager(id);

        var query = s_dal.Order.ReadAll()
            .Select(doOrder => DoOrderToBoOrder(doOrder))
            .Distinct();

        //filter
        if (filter != null)
        {
            query = query.Where(order =>
            {
                if (value == null) return false;

                return filter switch
                {
                    BO.EnumOrderFieldSort.Id => order.Id.Equals(Convert.ToInt32(value)),
                    BO.EnumOrderFieldSort.OrderType => order.OrderType.Equals(value),
                    BO.EnumOrderFieldSort.AerialDistance => order.AerialDistance.Equals(Convert.ToDouble(value)),
                    BO.EnumOrderFieldSort.CustomerName => order.CustomerName != null && order.CustomerName.Contains(value.ToString()!, StringComparison.OrdinalIgnoreCase),
                    BO.EnumOrderFieldSort.Weight => order.Weight.Equals(Convert.ToDouble(value)),
                    BO.EnumOrderFieldSort.Fragile => order.Fragile.Equals(Convert.ToBoolean(value)),
                    BO.EnumOrderFieldSort.CreationTime => order.CreationTime.Date.Equals(Convert.ToDateTime(value).Date),
                    BO.EnumOrderFieldSort.OrderStatus => order.OrderStatus.Equals(value),
                    BO.EnumOrderFieldSort.ScheduleStatus => order.ScheduleStatus.Equals(value),
                    _ => true //otherwise no filter
                };
            });
        }

        //sort
        if (value == null)
        {
            query = query.OrderBy(order => order.OrderStatus);
        }
        else if (sort != null)
        {
            query = sort switch
            {
                BO.EnumOrderFieldSort.Id => query.OrderBy(o => o.Id),
                BO.EnumOrderFieldSort.OrderType => query.OrderBy(o => o.OrderType),
                BO.EnumOrderFieldSort.AerialDistance => query.OrderBy(o => o.AerialDistance),
                BO.EnumOrderFieldSort.CustomerName => query.OrderBy(o => o.CustomerName),
                BO.EnumOrderFieldSort.Weight => query.OrderBy(o => o.Weight),
                BO.EnumOrderFieldSort.Fragile => query.OrderBy(o => o.Fragile),
                BO.EnumOrderFieldSort.CreationTime => query.OrderBy(o => o.CreationTime),
                BO.EnumOrderFieldSort.ExpectedDeliveryTime => query.OrderBy(o => o.ExpectedDeliveryTime),
                BO.EnumOrderFieldSort.OrderStatus => query.OrderBy(o => o.OrderStatus),
                _ => query.OrderBy(o => o.OrderStatus) //default
            };
        }

        //return
        return query.Select(BoOrder => BoOrderToBoOrderInList(BoOrder)).ToList();

    }

    /// <summary>
    /// converts a BO Order to a BO OrderInList
    /// </summary>
    internal static OrderInList BoOrderToBoOrderInList(Order boOrder)
    {
        return new OrderInList
        {
            CourierId = DeliveryManager.GetCourierIdToBoOrder(boOrder),
            OrderId = boOrder.Id,
            OrderType = boOrder.OrderType,
            AerialDistance = boOrder.AerialDistance,
            OrderStatus = boOrder.OrderStatus,
            ScheduleStatus = boOrder.ScheduleStatus,
            RemainingTime = boOrder.RemainingTime,
            TotalDeliveryTime = boOrder.OrderDelivHist != null && boOrder.OrderDelivHist.Count() > 0 ?//check logic again
                                boOrder.OrderDelivHist.Last().DelCreationTime - boOrder.CreationTime :
                                TimeSpan.Zero,
            TotalDeliveries = boOrder.OrderDelivHist != null ? boOrder.OrderDelivHist.Count() : 0
        };
    }

    /// <summary>
    /// Returns an array of order quantities by all status types.
    /// </summary>
    /// <param name="id">id of person asking the data</param>
    internal static int[] GetAmountOfOrdersByStatus(int id)
    {
        Tools.IsManager(id);

        var orders = s_dal.Order.ReadAll().Select(doOrder => DoOrderToBoOrder(doOrder));
        int[] arrayAmountOfOrdersByStatus = new int[Enum.GetValues(typeof(BO.EnumOrderStatus)).Length]; //create array with size of EnumOrderStatus

        // group orders by OrderStatus and count occurrences
        // GroupBy creates groups of orders with the same OrderStatus
        // ToDictionary: convert group to dictionary: Key = OrderStatus, Value = count the orders for the status
        var statusCounts = orders.GroupBy(order => order.OrderStatus) // group orders by their EnumOrderStatus
                                 .ToDictionary
                                 (
                                     g => (int)g.Key, // key in dictionary is the numeric value of EnumOrderStatus
                                     g => g.Count()   // value in dictionary is the count of items in the group (LINQ func)
                                 );

        //fill in arrayAmountOfOrdersByStatus with valued from dictionary
        foreach (var pair in statusCounts)
        {
            arrayAmountOfOrdersByStatus[pair.Key] = pair.Value;
        }

        return arrayAmountOfOrdersByStatus;
    }

    /// <summary>
    /// update delivery status to Canceled if possible
    /// </summary>
    internal static void CancelOrder(int id, int orderId)
    {
        Tools.IsManager(id);

        DO.Order doOrder = s_dal.Order.Read(orderId) ?? throw new BO.BlDoesNotExistException($"Order with ID={orderId} does Not exist");
        BO.Order boOrder = DoOrderToBoOrder(doOrder);
        //Can be canceled if: Order is Open or InProgress but not yet delivered.
        //no need to update order status in DAL, as it is calculated at the conversion "DoOrderToBoOrder"
        if (boOrder.OrderStatus == BO.EnumOrderStatus.Open)
        {
            //create a deme delivery with status Canceled
            DO.Delivery delivery = DeliveryManager.CreateDemeDelivery(orderId);
            s_dal.Delivery.Create(delivery);
        }
        else if (boOrder.OrderStatus == BO.EnumOrderStatus.InProgress)
        {
            DO.Delivery? delivery = s_dal.Delivery.Read(d => d.OrderId == orderId && d.EndDeliveryStatus == null);
            if (delivery != null)
            {
                var updatedDelivery = delivery with //create delivery copy by updated delivery
                {
                    EndDeliveryStatus = DO.EnumEndDeliveryStatus.Canceled,
                    EndDeliveryTime = AdminManager.Now
                };
                s_dal.Delivery.Update(updatedDelivery);
            }
        }
        else throw new BO.BlInvalidOperationException($"Order with ID={orderId} cannot be canceled as it is already {boOrder.OrderStatus}.");
    }

    internal static void EndOrderStatus(int id, int orderId, int deliveryId)
    {
        throw new NotImplementedException();
    }
}

