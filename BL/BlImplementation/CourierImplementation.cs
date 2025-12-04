using BlApi;
using Helpers;

namespace BlImplementation;

internal class CourierImplementation : ICourier
{
    public BO.EnumUserRole Login(string username, string password) => CourierManager.Login(username, password);
    public void Create(int id, BO.Courier boCourier)
    {
        CourierManager.CreateCourier(id,boCourier);
    }
    public BO.Courier? Read(int id, int courierId)
    {
        return CourierManager.GetCourierById(id,courierId);
    }
    public IEnumerable<BO.CourierInList>? GetCouriersInList(int id, bool? active = null, BO.EnumCourierFieldSort? sort = null, BO.EnumCourierFieldFilter? filter = null, object? value = null)
    {
        return CourierManager.GetCouriersInList(id, active, sort, filter, value);
    }
    public void Update(int id, BO.Courier boCourier)
    {
        CourierManager.UpdateCourier(id, boCourier);
    }
    public void Delete(int id, int courierId)
    {
        CourierManager.DeleteCourier(id,courierId);
    }    
    public int GetNumOfDeliveryOnTimeForCourier(int id, int courierId)
    {
        return DeliveryManager.GetDeliverierOnTimeForCourier(id, courierId);
    }
    public int GetNumOfDeliveryLateForCourier(int id, int courierId)
    {
        return DeliveryManager.GetDeliverierLateForCourier(id, courierId);
    }
    public void AssignOrderToCourier(int courierId, int orderId)
    {
        OrderManager.CreateDeliveryForOrder(courierId, courierId, orderId); //?right?
    }
    public IEnumerable<BO.ClosedDeliveryInList> GetCloseDeliveriesForCourier(int id, int courierId)
    {
        return DeliveryManager.GetClosedDeliveriesInListsToCourier(id, courierId);
    }
}
