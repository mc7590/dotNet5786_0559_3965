using DalApi;
using System.Data;
using System.Runtime.CompilerServices;

namespace Helpers;

internal static class CourierManager
{

    private static IDal s_dal = Factory.Get; //stage 4
    internal static ObserverManager Observers = new(); //stage 5 


    /// <summary>
    /// Creates a new courier in DAL
    /// </summary>
    internal static void CreateCourier(int id, BO.Courier boCourier)
    {
        Tools.IsManager(id);

        lock (AdminManager.BlMutex) //stage 7
        {

            if (s_dal.Courier.Read(boCourier.Id) != null)
                throw new BO.BlAlreadyExistsException($"Courier with ID={boCourier.Id} already exists");
            Tools.IsValidId(boCourier.Id);
            Tools.IsValidName(boCourier.Name!);
            Tools.IsValidPhone(boCourier.CourierPhone!);
            Tools.IsValidEmail(boCourier.Email!);
            if (!Tools.IsStrongPassword(boCourier.Password!))
                throw new BO.BlInvalidInputException("Password is not strong enough");

            DO.Courier? doCourier = new()
            {
                Id = boCourier.Id,
                Name = boCourier.Name!,
                CourierPhone = boCourier.CourierPhone!,
                Email = boCourier.Email!,
                Password = boCourier.Password!,
                Active = boCourier.Active,
                DeliveryMethod = (DO.EnumDeliveryMethod)boCourier.DeliveryMethod,
                StartedWorking = DateTime.Now,
                MaxPersonalDistance = boCourier.MaxPersonalDistance
            };
            s_dal.Courier.Create(doCourier);
        }

        Observers.NotifyListUpdated(); //stage 5 /no need to notify item updated as it is a new item
    }


    /// <summary>
    /// Gets a courier by ID from DAL 
    /// </summary>
    internal static BO.Courier? GetCourierById(int id, int courierId)
    {
        DO.Courier doCourier;

        lock (AdminManager.BlMutex) //stage 7
            doCourier = s_dal.Courier.Read(courierId) ?? throw new BO.BlDoesNotExistException($"Courier with ID={id} does Not exist");
        
        return new BO.Courier
        {
            Id = doCourier.Id,
            Name = doCourier.Name,
            CourierPhone = doCourier.CourierPhone,
            Email = doCourier.Email,
            Password = doCourier.Password,
            Active = doCourier.Active,
            DeliveryMethod = (BO.EnumDeliveryMethod)doCourier.DeliveryMethod,
            StartedWorking = doCourier.StartedWorking,
            MaxPersonalDistance = doCourier.MaxPersonalDistance,
            TotalOnTimeDeliveries = DeliveryManager.GetDeliveriesOnTimeForCourier(id, doCourier.Id),
            TotalLateDeliveries = DeliveryManager.GetDeliveriesLateForCourier(id, doCourier.Id),
            ActiveDeliveryOrder = GetActiveDeliveryOrderForCourier(id, doCourier.Id)
        };
    }

    /// <summary>
    /// Gets list of couriers from DAL with optional filtering and sorting
    /// <param name="id">id of the person asking the data</param>"
    /// <param name="activeFilter">If not null, filters couriers by active status</param>
    /// <param name="sortBy">If not null, sorts couriers by the specified field</param>
    /// </summary>
    internal static IEnumerable<BO.CourierInList> GetCouriersInList(int id, BO.EnumActiveCourier? activeFilter, BO.EnumCourierFieldSort? sortBy = null)
    {
        Tools.IsManager(id);
        IEnumerable<DO.Courier> doCouriers;

        lock (AdminManager.BlMutex) //stage 7
            doCouriers = s_dal.Courier.ReadAll();

        // FILTER by active status
        if (activeFilter != null && activeFilter != BO.EnumActiveCourier.None)
        {
            bool active = activeFilter == BO.EnumActiveCourier.Active;
            doCouriers = doCouriers.Where(c => c.Active == active);
        }

        // convert doCouriers to BO.CourierInList
        IEnumerable<BO.CourierInList> boCouriersInList = from c in doCouriers
                                                   let activeOrderId = GetActiveDeliveryIdForCourier(id, c.Id)
                                                   select new BO.CourierInList
                                                   {
                                                       Id = c.Id,
                                                       Name = c.Name,
                                                       Active = c.Active,
                                                       DeliveryMethod = (BO.EnumDeliveryMethod)c.DeliveryMethod,
                                                       StartedWorking = c.StartedWorking,
                                                       TotalOnTimeDeliveries = DeliveryManager.GetDeliveriesOnTimeForCourier(id, c.Id),
                                                       TotalLateDeliveries = DeliveryManager.GetDeliveriesLateForCourier(id, c.Id),
                                                       OrderInProgressId = activeOrderId != null ? activeOrderId.Value : 0
                                                   };
        // SORT
        if (sortBy != null)
        {
            boCouriersInList = sortBy switch
            {
                BO.EnumCourierFieldSort.Id => boCouriersInList.OrderBy(c => c.Id),
                BO.EnumCourierFieldSort.Name => boCouriersInList.OrderBy(c => c.Name),
                BO.EnumCourierFieldSort.StartedWorking => boCouriersInList.OrderBy(c => c.StartedWorking),
                BO.EnumCourierFieldSort.TotalOnTimeDeliveries => boCouriersInList.OrderBy(c => c.TotalOnTimeDeliveries),
                BO.EnumCourierFieldSort.TotalLateDeliveries => boCouriersInList.OrderBy(c => c.TotalLateDeliveries),
                _ => boCouriersInList
            };
        }

        return boCouriersInList;
    }

    /// <summary>
    /// Updates a courier in DAL
    /// </summary>
    internal static void UpdateCourier(int id, BO.Courier boCourier)
    {
        Tools.IsManagerOrCourier(id, boCourier.Id);

        lock (AdminManager.BlMutex) //stage 7
        {
            //check if courier exists
            if (s_dal.Courier.Read(boCourier.Id) == null)
                throw new BO.BlDoesNotExistException($"Courier with ID={boCourier.Id} does Not exist");
            //check if password is strong enough
            if (!Tools.IsStrongPassword(boCourier.Password!))
                throw new BO.BlInvalidInputException($"Password {boCourier.Password} is not strong enough");
            if (boCourier.ActiveDeliveryOrder != null && boCourier.Active == false)
                throw new BO.BlInvalidInputException("Cannot set courier to inactive while having an active delivery order.");

            DO.Courier doCourier = BoCourierToDoCourier(boCourier)!;

            s_dal.Courier.Update(doCourier);
        }

        Observers.NotifyItemUpdated(boCourier.Id); //stage 5
        Observers.NotifyListUpdated();  //stage 5
    }

    /// <summary>
    /// deletes a courier from DAL
    /// </summary>
    internal static void DeleteCourier(int id, int courierId)
    {
        Tools.IsManager(id);

        lock (AdminManager.BlMutex) //stage 7
        {
            DO.Courier? existingCourier = s_dal.Courier.Read(courierId) ?? throw new BO.BlDoesNotExistException($"Courier with ID={courierId} does Not exist");
            s_dal.Courier.Delete(courierId);
        }

        Observers.NotifyItemUpdated(courierId); //stage 5
        Observers.NotifyListUpdated();  //stage 5
    }

    /// <summary>
    /// converts a BO.Courier to DO.Courier
    /// </summary>
    internal static DO.Courier? BoCourierToDoCourier(BO.Courier boCourier)
    {
        return new DO.Courier
        {
            Id = boCourier.Id,
            Name = boCourier.Name!,
            CourierPhone = boCourier.CourierPhone!,
            Email = boCourier.Email!,
            Password = boCourier.Password!,
            Active = boCourier.Active,
            DeliveryMethod = (DO.EnumDeliveryMethod)boCourier.DeliveryMethod,
            StartedWorking = boCourier.StartedWorking,
            MaxPersonalDistance = boCourier.MaxPersonalDistance
        };
    }

    /// <summary>
    /// Logs in a user (manager or courier) and returns their role 
    /// </summary>
    internal static BO.EnumUserRole Login(string userName, string password)
    {
        int id = int.Parse(userName);
        int managerId = AdminManager.GetConfig().ManagerId;
        if (managerId == id)
        {
            if (!Tools.VerifyPassword(password, AdminManager.GetConfig().ManagerPassword))
                throw new BO.BlUnauthorizedException("Incorrect password for admin.");

            return BO.EnumUserRole.Manager;
        }

        DO.Courier? courier;

        lock (AdminManager.BlMutex) //stage 7
            courier = s_dal.Courier.Read(id)

            ?? throw new BO.BlDoesNotExistException($"Courier with ID={id} does not exist");
        if (!Tools.VerifyPassword(password, courier.Password))
            throw new BO.BlUnauthorizedException("Incorrect password.");
        return BO.EnumUserRole.Courier;
    }

    /// <summary>
    /// returns the ID of the active delivery assigned to the specified courier.
    /// </summary>
    private static int? GetActiveDeliveryIdForCourier(int id, int courierId)
    {
        lock (AdminManager.BlMutex) //stage 7
            return s_dal.Delivery.ReadAll(d => d.CourierId == courierId && d.EndDeliveryTime == null).FirstOrDefault()?.Id;
    }

    /// <summary>
    /// gets the active delivery order for a specific courier
    /// </summary>
    private static BO.OrderInProgress? GetActiveDeliveryOrderForCourier(int id, int courierId)
    {
        Tools.IsManagerOrCourier(id, courierId);

        lock (AdminManager.BlMutex) //stage 7
        {
            var activeDelivery = s_dal.Delivery.ReadAll(d => d.CourierId == courierId && d.EndDeliveryTime == null).FirstOrDefault();
            if (activeDelivery == null)
                return null;

            var order = s_dal.Order.Read(activeDelivery.OrderId)!;
            return new BO.OrderInProgress
            {
                DeliveryId = activeDelivery.Id,
                OrderId = order.Id,
                OrderType = (BO.EnumOrderType)order.OrderType,
                Description = order.Description,
                Address = order.Address,
                AerialDistance = Tools.CalculateAerialDistance(order.Longitude, order.Latitude),
                ActualDistance = activeDelivery.DistanceInKm,
                CustomerName = order.CustomerName,
                CustomerPhone = order.CustomerPhone,
                ExpectedDeliveryTime = activeDelivery.DeliveryStartTime.Add(AdminManager.GetConfig().GetMaxDeliveryTime),
                MaxDeliveryTime = activeDelivery.DeliveryStartTime.Add(AdminManager.GetConfig().GetMaxDeliveryTime).Add(AdminManager.GetConfig().RiskRange),
                OrderStatus = BO.EnumOrderStatus.InProgress,
                ScheduleStatus = DeliveryManager.GetScheduleStatus(order.OrderCreationTime),
                RemainingTime = Tools.CalculateTimeDifference(AdminManager.Now, activeDelivery.DeliveryStartTime.Add(AdminManager.GetConfig().GetMaxDeliveryTime))
            };
        }
    }



    //the mutex for periodic tasks
    private static readonly AsyncMutex s_periodicMutex = new(); //stage 7

    /// <summary>
    /// functions to update time
    /// </summary>
    public static void PeriodicCouriersUpdates(DateTime oldClock, DateTime newClock)
    {
        // If the previous simulation is still in progress, exit immediately
        if (s_periodicMutex.CheckAndSetInProgress()) //stage 7
            return;


        DateTime now = newClock;
        TimeSpan maxInactivity = AdminManager.GetConfig().InactivityThreshold;
        IEnumerable<DO.Courier> inactiveCouriers;

        lock (AdminManager.BlMutex) //stage 7
        {
            var deliveries = s_dal.Delivery.ReadAll();
            var couriers = s_dal.Courier.ReadAll();

            // find to all courier his last inactivity
            var couriersWithLastActivity =
                from courier in couriers
                let lastDelivery =
                    (from delivery in deliveries
                     where delivery.CourierId == courier.Id
                     orderby (delivery.EndDeliveryTime ?? delivery.DeliveryStartTime) descending
                     select delivery)
                    .FirstOrDefault()
                let lastActivityTime =
                    lastDelivery != null
                        ? (lastDelivery.EndDeliveryTime ?? lastDelivery.DeliveryStartTime)
                        : courier.StartedWorking
                select new
                {
                    Courier = courier,
                    LastActivity = lastActivityTime
                };

            // filter the courier which need update
            inactiveCouriers =
                from item in couriersWithLastActivity
                where item.Courier.Active == true
                where (now - item.LastActivity) > maxInactivity
                select item.Courier;

            // update the couriers
            inactiveCouriers
                .Select(c => c with { Active = false })
                .ToList() //must ToList bc cannot use ForEach on IEnumerable
                .ForEach(s_dal.Courier.Update);
        }

        foreach (var courier in inactiveCouriers)
            Observers.NotifyItemUpdated(courier.Id); //stage 5

        Observers.NotifyListUpdated();  //stage 5

        s_periodicMutex.UnsetInProgress(); //stage 7
    }


    /// <summary>
    /// Random number generator to simulation data
    /// </summary>
    private static readonly Random s_rand = new();

    private static readonly AsyncMutex s_simulationMutex = new(); //stage 7


    internal static async Task SimulateActivityOfCouriers()
    {
        // If the previous simulation is still in progress, exit immediately
        if (s_simulationMutex.CheckAndSetInProgress())
            return;


        List<DO.Courier> activeCouriers;
        lock (AdminManager.BlMutex)
        {
            activeCouriers = s_dal.Courier.ReadAll(c => c.Active == true).ToList();
        }

        foreach (var courier in activeCouriers)
        {
            // does courier has active order
            BO.OrderInProgress? activeOrder = CourierManager.GetActiveDeliveryOrderForCourier(courier.Id, courier.Id);

            //A case: courier has no order in progress
            if (activeOrder == null)
            {
                // 35% chance for un-busy courier to choose order
                if (s_rand.NextDouble() < 0.35)
                {
                    var openOrders = await OrderManager.GetListOfOpenOrderToChoose(courier.Id, courier.Id);

                    if (openOrders.Any() && s_rand.NextDouble() < 0.50) // הסתברות של 50% שיבחר אחת
                    {
                        var selectedOrder = openOrders.ElementAt(s_rand.Next(openOrders.Count()));

                        await OrderManager.CreateDeliveryForOrder(courier.Id, courier.Id, selectedOrder.OrderId);
                    }
                }
            }
            //B case: courier has order in progress
            else
            {
                //calc if enough time passed
                if (activeOrder.ActualDistance == null) //if net func for actual distance failed
                    continue;

                if (AdminManager.Now >= activeOrder.ExpectedDeliveryTime)
                {
                    //end the delivery, random final status
                    BO.EnumEndDeliveryStatus finalStatus = s_rand.NextDouble() switch
                    {
                        < 0.80 => BO.EnumEndDeliveryStatus.Delivered,       // 80% הצלחה
                        < 0.87 => BO.EnumEndDeliveryStatus.RefusedToReceive, // 7% סירבו לקבל
                        < 0.92 => BO.EnumEndDeliveryStatus.CustomerNotFound, // 5% לא נמצא הלקוח
                        < 0.97 => BO.EnumEndDeliveryStatus.Failed,           // 5% נכשל
                        _ => BO.EnumEndDeliveryStatus.Canceled              // 3% השאר בוטל
                    };

                    //close delivery
                        DeliveryManager.EndOrderStatus(courier.Id, courier.Id, activeOrder.DeliveryId, finalStatus);
                }
                else //if not enough time passed
                {
                    //10% manager cancel the order
                    if (s_rand.NextDouble() < 0.10)
                    {
                            OrderManager.CancelOrder(AdminManager.GetConfig().ManagerId, activeOrder.OrderId);
                    }
                }
            }
        }

        s_simulationMutex.UnsetInProgress();
    }

}