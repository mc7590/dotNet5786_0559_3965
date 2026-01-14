namespace Helpers;
//using BO;
using DalApi;
using System;
using System.Net.Http.Json;
//using DO;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Threading.Tasks;
using System.Globalization;
using System.ComponentModel;
using System.Text.Encodings.Web;
using BO;

internal static class OrderManager
{
    private static readonly IDal s_dal = Factory.Get; //stage 4
    internal static ObserverManager Observers = new(); //stage 5 


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
        ExpectedDeliveryTime = AdminManager.Now + CalculateExpectedDeliveryTime(doOrder.Id),
        MaxDeliveryTime = doOrder.OrderCreationTime + AdminManager.GetConfig().GetMaxDeliveryTime,
        OrderStatus = DeliveryManager.CalculateOrderStatus(doOrder.Id),
        ScheduleStatus = DeliveryManager.GetScheduleStatus(doOrder),
        RemainingTime = GetRemainingTime(doOrder),
        OrderDelivHist = DeliveryManager.GetListDeliveryPerOrderInList(doOrder.Id)

        //OrderDelivHist = null // DeliveryManager.GetListDeliveryPerOrderInList(doOrder.Id)
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
        (double lat, double lon) = Tools.GetLatiAndLong(boOrder.Address!).Result;
        DO.Order doOrder = new DO.Order()
        {
            Id = boOrder.Id,
            OrderType = (DO.EnumOrderType)boOrder.OrderType,
            Description = boOrder.Description,
            Address = boOrder.Address!,
            Latitude = lat,
            Longitude = lon,
            CustomerName = boOrder.CustomerName!,
            CustomerPhone = boOrder.CustomerPhone!,
            Weight = boOrder.Weight,
            Fragile = boOrder.Fragile,
            OrderCreationTime = boOrder.CreationTime,
        };

        s_dal.Order.Create(doOrder);

        Observers.NotifyListUpdated(); //stage 5 //no need to notify item updated as it is a new item


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
        s_dal.Order.Delete(orderId);
        Observers.NotifyListUpdated();
        Observers.NotifyItemUpdated(orderId);
    }

    /// <summary>
    /// Return a list of orders in a summarized format, with optional sorting and filtering.
    /// </summary>
    /// <param name="id">id of the person asking the data</param>
    /// <param name="filterBy">if null, no filter, else: filter by: if(enumFilter == filterValue)</param>
    /// <param name="filterValue">the value to use for filter, if null, sort by OrderStatus </param>
    /// <param name="sortBy">if (value!=null), sort by the Enum option for sort</param>
    /// <param name="sortValue">if null, sort by OrderStatus</param>
    internal static IEnumerable<BO.OrderInList> GetOrderInList(int id, BO.EnumOrderFieldFilter? filterBy = null, object? filterValue = null, BO.EnumOrderFieldSort? sortBy = null, object? sortValue = null)
    {
        Tools.IsManager(id);

        var doQuery = s_dal.Order.ReadAll();
        IEnumerable<BO.OrderInList> query = doQuery.Select(doOrder => doOrderToBoOrderInList(doOrder)); //make list of BO.OrderInList

        //filter
        if (filterBy != null && filterValue != null)
        {
            string filterValString = filterValue.ToString()!; //convert filterValue once //won't be null bc after the 'if'
            query = filterBy.Value switch
            {
                BO.EnumOrderFieldFilter.OrderType => query.Where(o => o.OrderType.ToString().Contains(filterValString, StringComparison.OrdinalIgnoreCase)),
                BO.EnumOrderFieldFilter.OrderStatus => query.Where(o => o.OrderStatus.ToString().Contains(filterValString, StringComparison.OrdinalIgnoreCase)),
                _ => query //otherwise no filter
            };
        }

        //sort
        if (sortBy != null && sortValue != null)
        {
            query = sortBy.Value switch
            {
                BO.EnumOrderFieldSort.CourierId => query.OrderBy(o => o.CourierId),
                BO.EnumOrderFieldSort.OrderId => query.OrderBy(o => o.OrderId),
                BO.EnumOrderFieldSort.OrderType => query.OrderBy(o => o.OrderType.ToString()),
                BO.EnumOrderFieldSort.AerialDistance => query.OrderBy(o => o.AerialDistance),
                BO.EnumOrderFieldSort.OrderStatus => query.OrderBy(o => o.OrderStatus.ToString()),
                BO.EnumOrderFieldSort.ScheduleStatus => query.OrderBy(o => o.ScheduleStatus.ToString()),
                BO.EnumOrderFieldSort.RemainingTime => query.OrderBy(o => o.RemainingTime),
                BO.EnumOrderFieldSort.TotalDeliveryTime => query.OrderBy(o => o.TotalDeliveryTime),
                BO.EnumOrderFieldSort.TotalDeliveries => query.OrderBy(o => o.TotalDeliveries),
                _ => query.OrderBy(order => order.OrderStatus.ToString()) //otherwise sort by orderStatus
            };
        }
        else
        {
            //default sort in case no sort input
            query = query.OrderBy(o => o.OrderStatus.ToString());
        }

        return query; //return the final query
    }

    /// <summary>
    /// helpers: converts a list of DO.Orders to a list of BO.OrderInList
    /// </summary>
    /// <param name="doOrders"></param>
    /// <returns></returns>
    internal static BO.OrderInList doOrderToBoOrderInList(DO.Order doOrder)
    {
        //return doOrders.Select(doOrder => DoOrderToBoOrder(doOrder)) //XXX
        //               .Select(boOrder => BoOrderToBoOrderInList(boOrder));
        return new BO.OrderInList
        {
            CourierId = DeliveryManager.GetCourierIdToDoOrder(doOrder),
            OrderId = doOrder.Id,
            OrderType = (BO.EnumOrderType)doOrder.OrderType,
            AerialDistance = Tools.CalculateAerialDistance(doOrder.Longitude, doOrder.Latitude),
            OrderStatus = DeliveryManager.CalculateOrderStatus(doOrder.Id),
            ScheduleStatus = DeliveryManager.GetScheduleStatus(doOrder),
            RemainingTime = OrderManager.GetRemainingTime(doOrder),
            TotalDeliveryTime = DeliveryManager.GetListDeliveryPerOrderInList(doOrder.Id).Count() > 0 ?
                    DeliveryManager.GetListDeliveryPerOrderInList(doOrder.Id).Last().DelCreationTime - doOrder.OrderCreationTime :
                    TimeSpan.Zero,
            TotalDeliveries = DeliveryManager.GetListDeliveryPerOrderInList(doOrder.Id).Count()
        };
    }

    /// <summary>
    /// converts a BO Order to a BO OrderInList
    /// </summary>
    internal static BO.OrderInList BoOrderToBoOrderInList(BO.Order boOrder)
    {
        return new BO.OrderInList
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

            Observers.NotifyListUpdated();  //stage 5
            Observers.NotifyItemUpdated(orderId);

        }
        else if (boOrder.OrderStatus == BO.EnumOrderStatus.InProgress)
        {
            /////connect to deliveryManager to update delivery
            DO.Delivery? delivery = s_dal.Delivery.Read(d => d.OrderId == orderId && d.EndDeliveryStatus == null) ?? throw new BO.BlDoesNotExistException($"Delivery with Order ID={orderId} does Not exist");
            DeliveryManager.UpdateDelivery(delivery, BO.EnumEndDeliveryStatus.Canceled, AdminManager.Now);
        }
        else throw new BO.BlInvalidOperationException($"Order with ID={orderId} cannot be canceled as it is already {boOrder.OrderStatus}.");
    }

    /// <summary>
    /// Selects an order for delivery by associating it with a specific courier.
    /// </summary>
    /// <param name="id">The person asking the date - manager / courier</param>
    /// <param name="courierId">The courier id assigned to deliver the order.</param>
    /// <param name="orderId">The order id to be delivered.</param>
    internal static async Task CreateDeliveryForOrder(int id, int courierId, int orderId)
    {
        //check if the person asking is a manager or the courier assigned to the delivery
        Tools.IsManagerOrCourier(id, courierId);

        //check if order exists
        DO.Order doOrder = s_dal.Order.Read(orderId) ?? throw new BO.BlDoesNotExistException($"Order with ID={orderId} does Not exist");

        //check if a delivery for this order already exists
        if (s_dal.Delivery.Read(d => d.OrderId == orderId) != null)
            throw new BO.BlInvalidOperationException($"Order with ID={orderId} is already assigned to a delivery.");

        //check if courier exists
        DO.Courier doCourier = s_dal.Courier.Read(courierId) ?? throw new BO.BlDoesNotExistException($"Courier with ID={courierId} does Not exist");

        BO.Order boOrder = DoOrderToBoOrder(doOrder);
        if (boOrder.OrderStatus == BO.EnumOrderStatus.Open)
        {
            //create a new delivery for this order and courier
            DO.Delivery delivery = new DO.Delivery()
            {
                Id = 0, //will be set by DAL
                OrderId = orderId,
                CourierId = courierId,
                DeliveryMethod = doCourier.DeliveryMethod,
                DeliveryStartTime = AdminManager.Now,
                DistanceInKm = await Tools.CalculateDistanceInKm(doOrder.Longitude, doOrder.Latitude),
                EndDeliveryStatus = null,
                EndDeliveryTime = null
            };
            s_dal.Delivery.Create(delivery);

            Observers.NotifyListUpdated();  //stage 5
            Observers.NotifyItemUpdated(orderId);
        }
        else
        {
            throw new BO.BlInvalidOperationException($"Order with ID={orderId} cannot be assigned to delivery as its status is {boOrder.OrderStatus}.");
        }
    }


    /// <summary>
    /// gets list of open orders that a courier can chose from, with optional filtering and sorting
    /// </summary>
    internal static async Task<IEnumerable<BO.OpenOrderInList>> GetListOfOpenOrderToChoose(int id, int courierId, BO.EnumOrderType? typeFilter = null, BO.EnumOpenOrderInListField? sortBy = null)
    {
        Tools.IsManagerOrCourier(id, courierId);
        DO.Courier courier = s_dal.Courier.Read(courierId) ?? throw new BO.BlDoesNotExistException($"Courier with ID={courierId} not found");
        if (!courier.Active)
            throw new BO.BlInvalidInputException($"Courier with ID={courierId} is not active");
        IEnumerable<DO.Order> orders = s_dal.Order.ReadAll(or => Tools.CalculateAerialDistance(or.Longitude, or.Latitude) <= courier.MaxPersonalDistance);
        var openOrder = from o in orders
                        let BoOrder = DoOrderToBoOrder(o)
                        where BoOrder.OrderStatus == BO.EnumOrderStatus.Open
                        select o;
        if (typeFilter != null)
        {
            openOrder = openOrder.Where(or => s_dal.Order.Read(or.Id)!.OrderType == (DO.EnumOrderType)typeFilter);
        }

        var tasksOpenOIL= openOrder.Select(async o =>
        {
            double distance = await Tools.CalculateDistanceInKm(o.Longitude, o.Latitude);

            var maxDeliveryTime = AdminManager.Now + AdminManager.GetConfig().GetMaxDeliveryTime;

            return new BO.OpenOrderInList
            {
                CourierId = 0,
                OrderId = o.Id,
                OrderType = (BO.EnumOrderType)o.OrderType,
                Weight = o.Weight,
                Fragile = o.Fragile,
                Address = o.Address,
                AerialDistance = Tools.CalculateAerialDistance(o.Longitude, o.Latitude),
                DistanceInKm = distance,
                EstimatedArrivalTime = DeliveryManager.CalculateEstimatedDeliveryTime(courier.DeliveryMethod, distance),
                ScheduleStatus = DeliveryManager.GetScheduleStatus(o),
                RemainingTime = Tools.CalculateTimeDifference(AdminManager.Now, maxDeliveryTime),
                MaxDeliveryTime = maxDeliveryTime,
            };
        });

        // במקום List, נשתמש ב-IEnumerable
        IEnumerable<BO.OpenOrderInList> result = new List<BO.OpenOrderInList>();

        // הלופ שלך נשאר אותו דבר
        foreach (var openTask in tasksOpenOIL)
        {
            BO.OpenOrderInList openO = await openTask;
            ((List<BO.OpenOrderInList>)result).Add(openO);
        }


        //var result =
        //    from o in openOrder
        //    let distance = await Tools.CalculateDistanceInKm(o.Longitude, o.Latitude)
        //    let maxDeliveryTime = AdminManager.Now + AdminManager.GetConfig().GetMaxDeliveryTime
        //    select new BO.OpenOrderInList
        //    {
        //        CourierId = 0,
        //        OrderId = o.Id,
        //        OrderType = (BO.EnumOrderType)o.OrderType,
        //        Weight = o.Weight,
        //        Fragile = o.Fragile,
        //        Address = o.Address,
        //        AerialDistance = Tools.CalculateAerialDistance(o.Longitude, o.Latitude),
        //        DistanceInKm = distance,
        //        EstimatedArrivalTime = DeliveryManager.CalculateEstimatedDeliveryTime(courier.DeliveryMethod, distance),
        //        ScheduleStatus = DeliveryManager.GetScheduleStatus(o),
        //        RemainingTime = Tools.CalculateTimeDifference(AdminManager.Now, maxDeliveryTime),
        //        MaxDeliveryTime = maxDeliveryTime,
        //    };
        result = sortBy == null ? result.OrderBy(r => r.ScheduleStatus)
            : sortBy switch
            {
                BO.EnumOpenOrderInListField.CourierId => result.OrderBy(r => r.CourierId),
                BO.EnumOpenOrderInListField.OrderId => result.OrderBy(r => r.OrderId),
                BO.EnumOpenOrderInListField.OrderType => result.OrderBy(r => r.OrderType),
                BO.EnumOpenOrderInListField.Address => result.OrderBy(r => r.Address),
                BO.EnumOpenOrderInListField.DistanceInKm => result.OrderBy(r => r.DistanceInKm),
                BO.EnumOpenOrderInListField.RemainingTime => result.OrderBy(r => r.RemainingTime),
                _ => result
            };

        return result;
    }

    private static TimeSpan GetRemainingTime(DO.Order doOrder)
    {
        DateTime maxDeliveryTime = doOrder.OrderCreationTime + AdminManager.GetConfig().GetMaxDeliveryTime;
        TimeSpan remainingTime = Tools.CalculateTimeDifference(AdminManager.Now, maxDeliveryTime);
        if (remainingTime <= TimeSpan.Zero)
            return TimeSpan.Zero;
        return Tools.CalculateTimeDifference(AdminManager.Now, maxDeliveryTime);
    }
    private static TimeSpan CalculateExpectedDeliveryTime(int orderId)
    {
        DO.Order doOrder = s_dal.Order.Read(orderId) ?? throw new BO.BlDoesNotExistException($"Order with ID={orderId} does Not exist");
        DO.Delivery? delivery = s_dal.Delivery.Read(d => d.OrderId == orderId && d.EndDeliveryStatus == null);
        if (delivery == null || delivery.DistanceInKm == null)
            return TimeSpan.Zero;
        return DeliveryManager.CalculateEstimatedDeliveryTime(delivery.DeliveryMethod, Tools.CalculateDistanceInKm(doOrder.Longitude, doOrder.Latitude));
    }

    /// <summary>
    /// updates an order in DAL
    /// </summary>
    internal static void UpdateOrder(int id, BO.Order boOrder)
    {
        Tools.IsManager(id);

        if (boOrder != null)
        {
            //check if order exists
            if (s_dal.Order.Read(boOrder.Id) == null)
                throw new BO.BlDoesNotExistException($"Order with ID={boOrder.Id} does Not exist");

            DO.Order doOrder = BoOrderToDoOrder(boOrder);
            s_dal.Order.Update(doOrder);

            Observers.NotifyItemUpdated(boOrder.Id); //stage 5
            Observers.NotifyListUpdated();  //stage 5

        }

    }

    
}