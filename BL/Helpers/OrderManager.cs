using DalApi;


namespace Helpers;

internal static class OrderManager
{
    private static IDal s_dal = Factory.Get; //stage 4

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
        ExpectedDeliveryTime = CalculateExpectedDeliveryTime(doOrder.Id),//in ManagerDelivery
        MaxDeliveryTime = doOrder.OrderCreationTime + AdminManager.GetConfig().GetMaxDeliveryTime,
        OrderStatus = CalculateOrderStatus(doOrder.Id),
        ScheduleStatus = GetScheduleStatus(doOrder),
        RemainingTime = GetRemainingTime(doOrder),
        OrderDelivHist = DeliveryManager.GetDeliveryPerOrderInList(doOrder.Id)
    };



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

    internal static BO.Order? GetOrder(int id, int orderId)
    {
        Tools.IsManager(id);

        DO.Order doOrder;
        doOrder = s_dal.Order.Read(orderId) ?? throw new BO.BlDoesNotExistException($"Order with ID={orderId} does Not exist");
        return DoOrderToBoOrder(doOrder);
    }

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

        var query = s_dal.Order.ReadAll().Select(DoOrderToBoOrder);
        
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
                    _ => true
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
        return query.Select(order => new BO.OrderInList
        {
            CourierId 
            OrderId
            OrderType 
            AerialDistance = order.AerialDistance,
            OrderStatus = order.OrderStatus,
            ScheduleStatus = order.ScheduleStatus,
            RemainingTime = order.RemainingTime, // הנחה שזה חושב ב-BO
            TotalDeliveryTime = DeliveryManager.GetTotalDeliveryTime(order.Id),
            TotalDeliveries
        }).ToList();
    }

}

