using DalApi;
using System.Data;
using System.Runtime.CompilerServices;

namespace Helpers;

internal static class CourierManager
{

    private static IDal s_dal = Factory.Get; //stage 4
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
    }
    internal static BO.Courier? GetCourierById(int id, int courierId)
    {
        DO.Courier doCourier;
        doCourier = s_dal.Courier.Read(id) ?? throw new BO.BlDoesNotExistException($"Courier with ID={id} does Not exist");
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
            TotalOnTimeDeliveries = GetDeliverierOnTime(id, doCourier.Id),
            TotalLateDeliveries = GetDeliverierLate(id, doCourier.Id),
            ActiveDeliveryOrder = GetActiveDeliveryOrderForCourier(id, doCourier.Id)
        };
    }
    internal static IEnumerable<BO.CourierInList> GetCouriersInList(int id, bool? active, BO.EnumCourierFieldSort? sort, BO.EnumCourierFieldFilter? filter, object? value)
    {
        IEnumerable<DO.Courier> doCouriers = s_dal.Courier.ReadAll();
        if (active != null)
            doCouriers = doCouriers.Where(c => c.Active == active);

        IEnumerable<BO.CourierInList> boCouriers = from c in doCouriers
                                                   select new BO.CourierInList
                                                   {
                                                       Id = c.Id,
                                                       Name = c.Name,
                                                       Active = c.Active,
                                                       DeliveryMethod = (BO.EnumDeliveryMethod)c.DeliveryMethod,
                                                       StartedWorking = c.StartedWorking,
                                                       TotalOnTimeDeliveries = GetDeliverierOnTime(id, c.Id),
                                                       TotalLateDeliveries = GetDeliverierLate(id, c.Id),
                                                       OrdersInProgressId = GetActiveDeliveryOrderForCourier(id, c.Id) != null ? GetActiveDeliveryOrderForCourier(id, c.Id)!.OrderId : -1
                                                   };
        if (sort != null)
        {
            boCouriers = sort switch
            {
                BO.EnumCourierFieldSort.Id => boCouriers.OrderBy(c => c.Id),
                BO.EnumCourierFieldSort.Name => boCouriers.OrderBy(c => c.Name),
                BO.EnumCourierFieldSort.StartedWorking => boCouriers.OrderBy(c => c.StartedWorking),
                BO.EnumCourierFieldSort.TotalOnTimeDeliveries => boCouriers.OrderBy(c => c.TotalOnTimeDeliveries),
                BO.EnumCourierFieldSort.TotalLateDeliveries => boCouriers.OrderBy(c => c.TotalLateDeliveries),
                BO.EnumCourierFieldSort.MaxPersonalDistance => boCouriers.OrderBy(c => c.MaxPersonalDistance),
                _ => boCouriers
            };
        }
        if (filter != null && value != null)
        {
            boCouriers = filter switch
            {
                BO.EnumCourierFieldFilter.DeliveryMethod => boCouriers.Where(c => c.DeliveryMethod == (BO.EnumDeliveryMethod)value!),
                BO.EnumCourierFieldFilter.MaxPersonalDistance => boCouriers.Where(c => c.MaxPersonalDistance != null && c.MaxPersonalDistance >= (double)value!),
                _ => doCouriers
            };
        }
        return boCouriers;
    }

    internal static void UpdateCourier(int id, BO.Courier boCourier)
    {
        Tools.IsManagerOrCourier(id, boCourier.Id);
        DO.Courier? existingCourier = s_dal.Courier.Read(boCourier.Id) ?? throw new BO.BlDoesNotExistException($"Courier with ID={boCourier.Id} does Not exist");
        DO.Courier doCourier = courierBoToDo(boCourier)!;
        s_dal.Courier.Update(doCourier);
    }
    internal static void DeleteCourier(int courierId)
    {
        DO.Courier? existingCourier = s_dal.Courier.Read(courierId) ?? throw new BO.BlDoesNotExistException($"Courier with ID={courierId} does Not exist");
        s_dal.Courier.Delete(courierId);
    }
    internal static DO.Courier? courierBoToDo(BO.Courier boCourier)
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

    public static BO.EnumUserRole Login(int id, string password)
    {
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
            ActualDistance = Tools.CalculateDistanceInKm(order.Longitude, order.Latitude),
            CustomerName = order.CustomerName,
            CustomerPhone = order.CustomerPhone,
            ExpectedDeliveryTime = activeDelivery.DeliveryStartTime.Add(AdminManager.GetConfig().GetMaxDeliveryTime),
            MaxDeliveryTime = activeDelivery.DeliveryStartTime.Add(AdminManager.GetConfig().GetMaxDeliveryTime).Add(AdminManager.GetConfig().RiskRange),
            OrderStatus = BO.EnumOrderStatus.InProgress,
            ScheduleStatus = /*BO.EnumScheduleStatus.OnTime*/,
            RemainingTime = Tools.CalculateTimeDifference(AdminManager.Now, activeDelivery.DeliveryStartTime.Add(AdminManager.GetConfig().GetMaxDeliveryTime))
        };
    }
    public static IEnumerable<BO.ClosedDeliveryInList> CloseDeliveriesForCourier(int id, int courierId, BO.EnumDeliveryMethod deliveryMethod)
    {
        Tools.IsManagerOrCourier(id, courierId);
        IEnumerable<DO.Delivery> deliveries = s_dal.Delivery.ReadAll(d => d.CourierId == courierId);
        return from d in deliveries
               let order = s_dal.Order.Read(d.OrderId)!
               select new BO.ClosedDeliveryInList
               {
                   DeliveryId = d.Id,
                   OrderId = d.OrderId,
                   OrderType = (BO.EnumOrderType)order.OrderType,
                   Address = order.Address,
                   DeliveryMethod = (BO.EnumDeliveryMethod)s_dal.Courier.Read(courierId)!.DeliveryMethod,
                   DistanceInKm = Tools.CalculateDistanceInKm(order.Longitude, order.Latitude),
                   TotalDeliveryTime = Tools.CalculateTimeDifference(d.DeliveryStartTime, d.EndDeliveryTime!.Value),
                   EndDeliveryStatus = (BO.EnumEndDeliveryStatus)d.EndDeliveryStatus!
               };
    }
      
    
    public static int GetDeliverierLate(int id, int courierId)
    {
        Tools.IsManagerOrCourier(id, courierId);
        IEnumerable<DO.Delivery> deliveries = s_dal.Delivery.ReadAll(d => d.CourierId == courierId);
        return deliveries.Count(d => (d.DeliveryStartTime - d.EndDeliveryTime) > AdminManager.GetConfig().GetMaxDeliveryTime);
    }

    public static int GetDeliverierOnTime(int id, int courierId)
    {
        Tools.IsManagerOrCourier(id, courierId);
        IEnumerable<DO.Delivery> deliveries = s_dal.Delivery.ReadAll(d => d.CourierId == courierId);
        return deliveries.Count(d => (d.DeliveryStartTime - d.EndDeliveryTime) <= AdminManager.GetConfig().GetMaxDeliveryTime);
    }
    private static BO.ClosedDeliveryInList ConvertToClosedDeliveryInList(DO.Delivery doDelivery)
    {
        var order = s_dal.Order.Read(doDelivery.OrderId)
            ?? throw new BO.BlDoesNotExistException($"Order {doDelivery.OrderId} not found");
        TimeSpan totalTime = DateTime.Now - order.OrderCreationTime;
        return new BO.ClosedDeliveryInList
        {
            DeliveryId = doDelivery.Id,
            OrderId = doDelivery.OrderId,
            OrderType = (BO.EnumOrderType)order.OrderType,
            Address = order.Address,
            DeliveryMethod = (BO.EnumDeliveryMethod)doDelivery.DeliveryMethod,
            DistanceInKm = Tools.CalculateDistanceInKm(order.Longitude, order.Latitude),
            TotalDeliveryTime = totalTime,
            EndDeliveryStatus = ChekStatusToDelivery(totalTime, doDelivery.EndDeliveryStatus)
        };
    }
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
            .ForEach(c => s_dal.Courier.Update(c));
    }
    public static void SimulateCourseRegistrationAndGrade() => PeriodicCouriersUpdates(s_dal.Config.Clock.AddMinutes(-1), s_dal.Config.Clock);
}