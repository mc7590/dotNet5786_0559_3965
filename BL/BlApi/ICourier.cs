using BO;

namespace BlApi;

public interface ICourier
{
    BO.EnumUserRole Login(string username, string password);    
    IEnumerable<BO.CourierInList>? GetCourierInList(int id, bool? active = null, BO.EnumCourierFieldSort? sort = null, BO.EnumCourierFieldSort? filter = null);
    BO.Courier? Read(int id, int courierId);
    void Update(int id, BO.Courier boCourier);    
    void Delete(int id, int courierId);
    void Create(int id, BO.Courier boCourier);
    int GetNumOfDeliveryOnTimeForCourier(int id, int courierId);
    int GetNumOfDeliveryLateForCourier(int id, int courierId);
    void AssignDeliveryToCourier(int courierId, int deliveryId);
}
