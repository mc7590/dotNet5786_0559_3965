
namespace BlImplementation;
using BlApi;
//using BO;
using Helpers;

internal class OrderImplementation : IOrder
{
    public void Create(int id, BO.Order boOrder)
    {
        OrderManager.CreateOrder(id, boOrder);
    }

    public BO.Order? Read(int id, int orderId)
    {
        return OrderManager.GetOrder(id, orderId);
    }

    public void Delete(int id, int orderId)
    {
        OrderManager.DeleteOrder(id, orderId);
    }
    public IEnumerable<BO.OrderInList> GetOrderInList(int id, BO.EnumOrderFieldSort? sort = null, object? value = null, BO.EnumOrderFieldSort? filter = null)
    {
        return OrderManager.GetOrderInList(id, sort, value, filter);
    }

    public void Update(int id, BO.Order boOrder)
    {
        OrderManager.UpdateOrder(id, boOrder);
    }

    public int[] GetAmountOfOrdersByStatus(int id)
    {
        return OrderManager.GetAmountOfOrdersByStatus(id);
    }

    public void CancelOrder(int id, int orderId)
    {
        OrderManager.CancelOrder(id, orderId);
    }

    public void EndOrderStatus(int id, int courierId, int deliveryId)
    {
        DeliveryManager.EndOrderStatus(id, courierId, deliveryId);
    }

    public void CreateDeliveryForOrder(int id, int courierId, int orderId)
    {
        OrderManager.CreateDeliveryForOrder(id, courierId, orderId);
    }

    public IEnumerable<BO.ClosedDeliveryInList> GetClosedDeliveriesInListsToCourier(int id, int courierId, BO.EnumOrderType? typeFilter = null, BO.EnumClosedDeliveryInListField? sortBy = null)
    {
        return DeliveryManager.GetClosedDeliveriesInListsToCourier(id, courierId, typeFilter, sortBy);
    }

    public IEnumerable<BO.OpenOrderInList> GetListOfOpenOrderToChoose(int id, int courierId, BO.EnumOrderType? typeFilter = null, BO.EnumOpenOrderInListField? sortBy = null)
    {
        return OrderManager.GetListOfOpenOrderToChoose(id, courierId, typeFilter, sortBy);
    }

    #region Stage 5
    public void AddObserver(Action listObserver) =>
    OrderManager.Observers.AddListObserver(listObserver); //stage 5
    public void AddObserver(int id, Action observer) =>
    OrderManager.Observers.AddObserver(id, observer); //stage 5
    public void RemoveObserver(Action listObserver) =>
    OrderManager.Observers.RemoveListObserver(listObserver); //stage 5
    public void RemoveObserver(int id, Action observer) =>
    OrderManager.Observers.RemoveObserver(id, observer); //stage 5
    #endregion Stage 5
}
