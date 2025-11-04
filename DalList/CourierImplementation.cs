///CourierImplementation.cs
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
/// <summary>
/// CourierImplementation class implements ICourier interface for managing Courier entities in the data source  
/// </summary>
public class CourierImplementation : ICourier
{
    
    public void Create(Courier item)
    {
        if (Read(item.Id) != null)
            throw new NotImplementedException("An object of type Courier with such ID already exists.");

        DataSource.Couriers.Add(item);
    }
    public Courier? Read(int id)
    {
        foreach (var courier in DataSource.Couriers) {
            if (courier.Id == id) 
                return courier;
        }
        return null;
    } 
    public List<Courier> ReadAll()
    {
        return new List<DO.Courier>(DataSource.Couriers);
    }

    public void Delete(int id)
    {
        foreach (var courier in DataSource.Couriers)
        {
            if (courier.Id == id)
            {
                DataSource.Couriers.Remove(courier);
                return;
            }
        }
        throw new Exception("An object of type Courier with such ID doesn't exist");
    }
    public void DeleteAll()
    {
        DataSource.Couriers.Clear();
    }

    
    public void Update(Courier item)
    {
        foreach (var courier in DataSource.Couriers)
        {
            if (courier.Id ==  item.Id) 
            { 
                DataSource.Couriers.Remove(courier);
                DataSource.Couriers.Add(item);
                return;
            }
        }

        throw new NotImplementedException("An object of type Courier with such ID isnt exists.");
    }

}
