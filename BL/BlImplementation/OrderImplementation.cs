
namespace BlImplementation;
using BlApi;
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
}
