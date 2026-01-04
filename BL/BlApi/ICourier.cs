namespace BlApi;

public interface ICourier : IObservable //stage 5
{
    BO.EnumUserRole Login(string username, string password);    
    IEnumerable<BO.CourierInList>? GetCouriersInList(int id, BO.EnumActiveCourier? activeFilter = null, BO.EnumCourierFieldSort? sortBy = null);
    BO.Courier? Read(int id, int courierId);
    void Update(int id, BO.Courier boCourier);    
    void Delete(int id, int courierId);
    void Create(int id, BO.Courier boCourier);
    int GetNumOfDeliveryOnTimeForCourier(int id, int courierId);
    int GetNumOfDeliveryLateForCourier(int id, int courierId);
    void AssignOrderToCourier(int courierId, int orderId);
    IEnumerable<BO.ClosedDeliveryInList> GetCloseDeliveriesForCourier(int id, int courierId);
}
