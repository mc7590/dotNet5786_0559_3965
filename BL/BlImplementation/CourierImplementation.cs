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
    public IEnumerable<BO.CourierInList>? GetCouriersInList(int id, BO.EnumActiveCourier? activeFilter = null, BO.EnumCourierFieldSort? sortBy = null)
    {
        return CourierManager.GetCouriersInList(id, activeFilter, sortBy);
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
        return DeliveryManager.GetDeliveriesOnTimeForCourier(id, courierId);
    }
    public int GetNumOfDeliveryLateForCourier(int id, int courierId)
    {
        return DeliveryManager.GetDeliveriesLateForCourier(id, courierId);
    }
    public async Task AssignOrderToCourier(int courierId, int orderId)
    {
        await OrderManager.CreateDeliveryForOrder(courierId, courierId, orderId);
    }
    public IEnumerable<BO.ClosedDeliveryInList> GetCloseDeliveriesForCourier(int id, int courierId)
    {
        return DeliveryManager.GetClosedDeliveriesInListsToCourier(id, courierId);
    }

    #region Stage 5
    public void AddObserver(Action listObserver) =>
    CourierManager.Observers.AddListObserver(listObserver); //stage 5
    public void AddObserver(int id, Action observer) =>
    CourierManager.Observers.AddObserver(id, observer); //stage 5
    public void RemoveObserver(Action listObserver) =>
    CourierManager.Observers.RemoveListObserver(listObserver); //stage 5
    public void RemoveObserver(int id, Action observer) =>
    CourierManager.Observers.RemoveObserver(id, observer); //stage 5
    #endregion Stage 5

}
