
namespace BlApi;

public interface IOrder
{
    void Create(BO.Order boOrder);
    BO.Order? Read(int id);

    IEnumerable<BO.OrderInList> ReadAll(BO.OrderFieldSort? sort = null, BO.OrderFieldFilter? filter = null, object? value = null);
    void Update(BO.Order boOrder);
    void Delete(int id);

}
