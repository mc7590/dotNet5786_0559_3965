
namespace BlApi;

public interface IOrder
{
    /// <summary>
    /// Returns an array of order quantities by all status types.
    /// </summary>
    /// <param name="id">the id of the person asking the date</param>
    int[] GetAmountOfOrdersByStatus(int id);

    /// <summary>
    /// returns a BO.OrderInList filtered and sorted
    /// </summary>
    IEnumerable<BO.OrderInList> GetOrderInList(int id, BO.EnumOrderFieldSort? sort = null, object? value = null, BO.EnumOrderFieldSort? filter = null);

    BO.Order? Read(int id, int orderId);

    void Update(int id, BO.Order boOrder);

    /// <summary>
    /// Checks that the order is open or in processing but not yet fulfilled, otherwise the request is invalid.
    /// </summary>
    void CancelOrder(int id, int orderId);

    void Delete(int id, int orderId);

    void Create(int id, BO.Order boOrder);

    /// <summary>
    /// Attempting to request an update of a DO.Delivery from a data layer
    /// This function can be done by: The courier who makes the delivery
    /// </summary>
    void EndOrderStatus(int id, int orderId, int deliveryId);

    /// <summary>
    /// Attempting to add (Create) a new DO.Delivery from a data layer
    /// </summary>
    void ChooseOrderForDelivery(int id, int orderId, int deliveryId);

    /// <summary>
    /// Returns a sorted collection BO.ClosedDeliveryInList
    /// </summary>
    IEnumerable<BO.ClosedDeliveryInList> GetClosedDeliveriesInListsToCourier(int Id, int courierId, BO.EnumOrderType? typeFilter = null, BO.EnumClosedDeliveryInListField? sortBy = null)

    /// <summary>
    /// Returns a sorted collection BO.OpenOrderInList
    /// </summary>
    IEnumerable<BO.OpenOrderInList> GetListOfOpenOrderToChose(int id, int courierId, BO.EnumOrderType? typeFilter = null, BO.EnumOpenOrderInListField? sortBy = null);


}
