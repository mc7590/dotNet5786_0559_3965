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

        Observers.NotifyListUpdated(); //stage 5 /no need to notify item updated as it is a new item
    }


    /// <summary>
    /// Gets a courier by ID from DAL 
    /// </summary>
    internal static BO.Courier? GetCourierById(int id, int courierId)
    {
        DO.Courier doCourier;
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
        IEnumerable<DO.Courier> doCouriers = s_dal.Courier.ReadAll();

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
        //check if courier exists
        if(s_dal.Courier.Read(boCourier.Id)==null)
            throw new BO.BlDoesNotExistException($"Courier with ID={boCourier.Id} does Not exist");
        //check if password is strong enough
        if (!Tools.IsStrongPassword(boCourier.Password!))
            throw new BO.BlInvalidInputException($"Password {boCourier.Password} is not strong enough");
        if(boCourier.ActiveDeliveryOrder != null && boCourier.Active == false)    
            throw new BO.BlInvalidInputException("Cannot set courier to inactive while having an active delivery order.");
        
        DO.Courier doCourier = CourierBoToDo(boCourier)!;

        s_dal.Courier.Update(doCourier);

        Observers.NotifyItemUpdated(boCourier.Id); //stage 5
        Observers.NotifyListUpdated();  //stage 5
    } 
    internal static void DeleteCourier(int id, int courierId)
    {
        Tools.IsManager(id);
        DO.Courier? existingCourier = s_dal.Courier.Read(courierId) ?? throw new BO.BlDoesNotExistException($"Courier with ID={courierId} does Not exist");
        s_dal.Courier.Delete(courierId);

        Observers.NotifyItemUpdated(courierId); //stage 5
        Observers.NotifyListUpdated();  //stage 5
    }
    internal static DO.Courier? CourierBoToDo(BO.Courier boCourier)
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
        DO.Courier? courier = s_dal.Courier.Read(id)
    ?? throw new BO.BlDoesNotExistException($"Courier with ID={id} does not exist");
        if (!Tools.VerifyPassword(password, courier.Password))
            throw new BO.BlUnauthorizedException("Incorrect password.");
        return BO.EnumUserRole.Courier;
    }
    private static int? GetActiveDeliveryIdForCourier(int id, int courierId)
    {
        var activeDeliveryId = s_dal.Delivery.ReadAll(d => d.CourierId == courierId && d.EndDeliveryTime == null).FirstOrDefault()?.Id;
        return activeDeliveryId;
    }
    /// <summary>
    /// gets the active delivery order for a specific courier
    /// </summary>
    private static BO.OrderInProgress? GetActiveDeliveryOrderForCourier(int id, int courierId)
    {
        Tools.IsManagerOrCourier(id, courierId);

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
            ScheduleStatus = DeliveryManager.GetScheduleStatus(order),
            RemainingTime = Tools.CalculateTimeDifference(AdminManager.Now, activeDelivery.DeliveryStartTime.Add(AdminManager.GetConfig().GetMaxDeliveryTime))
        };
    }
 

    /// <summary>
    /// functions to update time
    /// </summary>
    public static void PeriodicCouriersUpdates(DateTime oldClock, DateTime newClock)
    {
        DateTime now = newClock;
        TimeSpan maxInactivity = AdminManager.GetConfig().InactivityThreshold;

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
        var inactiveCouriers =
            from item in couriersWithLastActivity
            where item.Courier.Active == true
            where (now - item.LastActivity) > maxInactivity
            select item.Courier;

        // update the couriers
        inactiveCouriers
            .Select(c => c with { Active = false })
            .ToList()
            .ForEach(c =>
            {
                s_dal.Courier.Update(c);
                Observers.NotifyItemUpdated(c.Id);  //stage 5
            });

        Observers.NotifyListUpdated();  //stage 5

    }


    public static void SimulateCourierInactivity() 
        => PeriodicCouriersUpdates(s_dal.Config.Clock.AddMinutes(-1), s_dal.Config.Clock);
 
}