using BO;
using DalApi;
using System.Data;
using System.Runtime.CompilerServices;

namespace Helpers;

internal static class CourierManager
{
    public static int sumOfOnTimeDeliveries = 0;
    public static int sumOfLateDeliveries = 0;

    private static IDal s_dal = Factory.Get; //stage 4
    internal static void CreateCourier(int id, BO.Courier boCourier)
    {
      Tools.IsManager(id);
      if(s_dal.Courier.Read(boCourier.Id) != null)
            throw new BO.BlAlreadyExistsException($"Courier with ID={boCourier.Id} already exists");
      Tools.IsValidId(boCourier.Id);
      Tools.IsValidPhone(boCourier.CourierPhone!);
      Tools.IsValidEmail(boCourier.Email!);
      DO.Courier? doCourier = new DO.Courier
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
    internal static IEnumerable<BO.CourierInList> GetCourierInList(int id, bool? active, BO.EnumCourierFieldSort? sort, BO.EnumCourierFieldSort? filter)
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
                BO.EnumCourierFieldSort.DeliveryMethod => boCouriers.OrderBy(c => c.DeliveryMethod),
                BO.EnumCourierFieldSort.StartedWorking => boCouriers.OrderBy(c => c.StartedWorking),
                BO.EnumCourierFieldSort.TotalOnTimeDeliveries => boCouriers.OrderBy(c => c.TotalOnTimeDeliveries),
                BO.EnumCourierFieldSort.TotalLateDeliveries => boCouriers.OrderBy(c => c.TotalLateDeliveries),
                _ => boCouriers
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
     
    //לממש!!!!!!
    private static BO.OrderInProgress? GetActiveDeliveryOrderForCourier(int id, int courierId)
    {
        return null;
    }
    public static int GetDeliverierLate(int id, int courierId)
    {
        return sumOfLateDeliveries;
    }

    public static int GetDeliverierOnTime(int id, int courierId)
    {
        return sumOfOnTimeDeliveries;
    }
    public static void AssignDeliveryToCourier(int courierId, int deliveryId)
    {
        if (s_dal.Courier.Read(courierId) == null)
            throw new BO.BlDoesNotExistException($"Courier with ID={courierId} does Not exist");
        BO.Courier courier = GetCourierById(courierId, courierId)!;
        if(!courier.Active)
            throw new BO.BlInvalidOperationException($"Courier with ID={courierId} is not active");
        if(courier.ActiveDeliveryOrder != null)
            throw new BO.BlInvalidOperationException($"Courier with ID={courierId} already has an active delivery");
        courier.ActiveDeliveryOrder = OrderInProgress.Create(deliveryId);
    }
}
