using BlApi;
using BO;
using Helpers;

namespace BlImplementation;

internal class CourierImplementation : ICourier
{       
    public EnumUserRole Login(string username, string password)
    {
        throw new NotImplementedException();
    }
    public void Create(int id, Courier boCourier)
    {
        CourierManager.CreateCourier(id,boCourier);
    }
    public Courier? Read(int id, int courierId)
    {
        return CourierManager.GetCourierById(id,courierId);
    }
    public IEnumerable<CourierInList>? GetCourierInList(int id, bool? active = null, EnumCourierFieldSort? sort = null, BO.EnumCourierFieldSort? filter = null)
    {
        return CourierManager.GetCourierInList(id, active, sort, filter);
    }
    public void Update(int id, Courier boCourier)
    {
        CourierManager.UpdateCourier(id, boCourier);
    }
    public void Delete(int id, int courierId)
    {
        CourierManager.DeleteCourier(courierId);
    }    
    public int GetNumOfDeliveryOnTimeForCourier(int id, int courierId)
    {
        return CourierManager.GetDeliverierOnTime(id, courierId);
    }
    public int GetNumOfDeliveryLateForCourier(int id, int courierId)
    {
        return CourierManager.GetDeliverierLate(id, courierId);
    }
    public void AssignDeliveryToCourier(int courierId, int deliveryId)
    {
        CourierManager.AssignDeliveryToCourier(courierId, deliveryId);
    }
}
