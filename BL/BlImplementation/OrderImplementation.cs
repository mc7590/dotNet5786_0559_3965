
namespace BlImplementation;
using BlApi;
using Helpers;

internal class OrderImplementation : IOrder
{
    public void Create(int id, BO.Order boOrder)
    {
        OrderManager.createOrder(id, boOrder); 
    }

    public BO.Order? Read(int id, int orderId)
    {
        return OrderManager.GetOrder(id, orderId);
    }

    public void Delete(int id, int orderId)
    {
        throw new NotImplementedException();
    }


    public IEnumerable<BO.OrderInList> ReadAll(int id, bool? status = null, BO.EnumOrderFieldSort? sort = null, BO.EnumOrderFieldSort? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(int id, Order boOrder)
    {
        throw new NotImplementedException();
    }
}
