using DalApi;

namespace Helpers;

internal static class CourierManager
{
    private static IDal s_dal = Factory.Get; //stage 4

    internal static BO.Courier? GetCourier(int id)
    {
        DO.Courier doCourier;
        doCourier = s_dal.Courier.Read(id) ?? throw new BO.BlDoesNotExistException($"Courier with ID={id} does Not exist");

        return new BO.Courier()
        {
            Id = id,
            Name = doCourier.Name,
            CourierPhone = doCourier.CourierPhone,
            Email = doCourier.Email,
            Password = doCourier.Password,
            Active = doCourier.Active,
            DeliveryMethod = (BO.EnumDeliveryMethod)doCourier.DeliveryMethod,
            StartedWorking = doCourier.StartedWorking,
            MaxPersonalDistance = doCourier.MaxPersonalDistance,
            TotalOnTimeDeliveries = GetCourierTotalOnTimeDeliveries(doCourier.Id),
            TotalLateDeliveries = GetCourierTotalLateDeliveries(doCourier.Id),
            ActiveDeliveryOrder = GetCourierActiveDeliveryOrder(doCourier.Id)
        };
    }

}
