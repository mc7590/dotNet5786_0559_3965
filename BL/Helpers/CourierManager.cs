
using DalApi;
using System.Data;
using System.Runtime.CompilerServices;

namespace Helpers;

internal static class CourierManager
{

    private static IDal s_dal = Factory.Get; //stage 4

    private static Enumerable <int, Enumerable<ClosedDeliveryInList>> GetCloseDeliveryToCouriers()
    {
        return 
            from c in s_dal.Courier.ReadAll()
            select c.Id;

    }
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

    public static void AssignDeliveryToCourier(int courierId, int deliveryId)
    {
        if (s_dal.Courier.Read(courierId) == null)
            throw new BO.BlDoesNotExistException($"Courier with ID={courierId} does Not exist");
        BO.Courier courier = GetCourierById(courierId, courierId)!;
        if (!courier.Active)
            throw new BO.BlInvalidOperationException($"Courier with ID={courierId} is not active");
        if (courier.ActiveDeliveryOrder != null)
            throw new BO.BlInvalidOperationException($"Courier with ID={courierId} already has an active delivery");
        DO.Delivery? delivery = s_dal.Delivery.Read(deliveryId)
            ?? throw new BO.BlDoesNotExistException($"Delivery with ID={deliveryId} does not exist.");

        courier.ActiveDeliveryOrder = new BO.OrderInProgress
        {
            DeliveryId = deliveryId,
            OrderId = delivery.OrderId,
            OrderType = (BO.EnumOrderType)s_dal.Order.Read(delivery.OrderId)!.OrderType,
            Description = s_dal.Order.Read(delivery.OrderId)!.Description ?? null,
            Address = s_dal.Order.Read(delivery.OrderId)!.Address ?? null,
            AerialDistance = Tools.CalculateAerialDistance(s_dal.Order.Read(delivery.OrderId)!.Longitude, s_dal.Order.Read(delivery.OrderId)!.Latitude),
            ActualDistance = Tools.CalculateDistanceInKm(s_dal.Order.Read(delivery.OrderId)!.Longitude, s_dal.Order.Read(delivery.OrderId)!.Latitude),
            CustomerName = s_dal.Order.Read(delivery.OrderId)!.CustomerName ?? null,
            CustomerPhone = s_dal.Order.Read(delivery.OrderId)!.CustomerPhone ?? null,
            ExpectedDeliveryTime = delivery.DeliveryStartTime.Add(AdminManager.GetConfig().GetMaxDeliveryTime),
            MaxDeliveryTime = delivery.DeliveryStartTime.Add(AdminManager.GetConfig().GetMaxDeliveryTime).Add(AdminManager.GetConfig().RiskRange),
            OrderStatus = BO.EnumOrderStatus.InProgress,
            ScheduleStatus = BO.EnumScheduleStatus.OnTime,
            RemainingTime = Tools.CalculateTimeDifference(AdminManager.Now, delivery.DeliveryStartTime.Add(AdminManager.GetConfig().GetMaxDeliveryTime))
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
 

}
     
    //לממש!!!!!!
    private static BO.OrderInProgress? GetActiveDeliveryOrderForCourier(int id, int courierId)
    {
        return null;
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
    public static BO.EnumDeliveryMethod GetDeliveryMethod()
    {
        return null;
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
    public static BO.EnumEndDeliveryStatus ChekStatusToDelivery(TimeSpan totalDeliveryTime, DO.EnumEndDeliveryStatus status)
    {
        return switch (status) {

        }
 
    }
}