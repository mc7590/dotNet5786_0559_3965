namespace BlApi;

public interface ICourier
{
    void Create(int id, BO.Courier boStudent);
    BO.Courier? Read(int id, int courierId);
    IEnumerable<BO.CourierInList> ReadAll(int id, bool? status = null, BO.EnumCourierFieldSort? sort = null, BO.EnumCourierFieldSort? filter = null);
    void Update(int id, BO.Courier boCourier);
    void Delete(int id, int courierId);
    IEnumerable<BO.OrderInList> GetRegisteredOrdersForCourier(int courierId);
    //IEnumerable<BO.OrderInList> GetUnRegisteredOrdersForCourier(int courierId);
    void AssignDeliveryToCourier(int courierId, int deliveryId);
    void UnAssignDeliveryFromCourier(int courierId, int deliveryId);
    //OrderInProgress GetOrderInProgressOfCourier(int courierId);
}
