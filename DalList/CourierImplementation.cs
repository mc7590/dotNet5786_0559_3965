///CourierImplementation.cs
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
/// <summary>
/// CourierImplementation class implements ICourier interface for managing Courier entities in the data source  
/// </summary>
internal class CourierImplementation : ICourier
{

    public void Create(Courier item)
    {
        if (Read(item.Id) != null)
            throw new NotImplementedException("An object of type Courier with such ID already exists.");

        DataSource.Couriers.Add(item);
    }
    public Courier? Read(int id)
    {
        //return DataSource.Couriers.Find(item => item.Id == id); //stage 1
        return DataSource.Couriers.FirstOrDefault(item => item.Id == id); //stage 2

    }

    /// <summary>
    /// Retrieves a courier by a specified filter.
    /// </summary>
    public Courier? Read(Func<Courier, bool> filter) // stage 2
    {
        return DataSource.Couriers.FirstOrDefault(item => filter(item));
    }

    ///// <summary>
    ///// Retrieves all Couriers from the data source.
    ///// </summary>
    //public List<Courier> ReadAll()
    //{
    //    return new List<DO.Courier>(DataSource.Couriers);
    //}

    /// <summary>
    /// Retrieves filtered couriers from the data source.
    /// </summary>
    public IEnumerable<Courier> ReadAll(Func<Courier, bool>? filter = null) //stage 2
    {
        return filter != null
            ? from item in DataSource.Couriers
              where filter(item)
              select item
            : from item in DataSource.Couriers
              select item;
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
