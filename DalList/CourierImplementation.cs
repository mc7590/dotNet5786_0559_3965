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
    /// <summary>
    /// Adds a new courier to the data source with a unique identifier.
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalAlreadyExistsException">In case a courier with the same ID already exists.</exception>
    public void Create(Courier item)
    {
        if (Read(item.Id) != null)
            throw new DalAlreadyExistsException($"Courier with ID={item.Id} already exists");

        DataSource.Couriers.Add(item);
    }

    /// <summary>
    /// Retrieves a courier by its unique identifier.
    /// </summary>
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

    /// <summary>
    /// Deletes a courier by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException">In case a courier with the specified ID doesn't exist.</exception>
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
        throw new DalDoesNotExistException($"Courier with ID={id} doesn't exist");
    }

    /// <summary>
    /// Deletes all couriers from the data source.
    /// </summary>
    public void DeleteAll()
    {
        DataSource.Couriers.Clear();
    }

    /// <summary>
    /// Updates an existing courier in the data source with the specified courier details.
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException">In case courier with the specified ID doesn't exist.</exception>
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
        throw new DalDoesNotExistException($"Courier with ID={item.Id} doesn't exist");
    }

}
