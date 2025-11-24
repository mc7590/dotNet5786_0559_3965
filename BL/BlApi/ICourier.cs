
namespace BlApi;

public interface ICourier
{
    void Create(BO.Courier boStudent);
    BO.Courier? Read(int id);

    IEnumerable<BO.CourierInList> ReadAll(BO.CourierFieldSort? sort = null, BO.StudentFieldFilter? filter = null, object? value = null);
    void Update(BO.Courier boStudent);
    void Delete(int id);

}
