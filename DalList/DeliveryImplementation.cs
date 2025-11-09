//DeliveryImplementation.cs
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

/// <summary>
/// DeliveryImplementation class implements IDelivery interface for managing Delivery entities in the data source  
/// </summary>
internal class DeliveryImplementation : IDelivery
{
    /// <summary>
    /// Adds a new delivery to the data source with a unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the delivery to retrieve.</param>
    public void Create(Delivery item)
    {
        int nextId = Config.NextDeliveryId;
        Delivery newItem = item with { Id = nextId };
        DataSource.Deliverys.Add(newItem);
    }

    /// <summary>
    /// Retrieves a delivery by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the delivery to retrieve.</param>
    public Delivery? Read(int id)
    {
        //return DataSource.Deliverys.Find(item => item.Id == id); //stage 1
        return DataSource.Deliverys.FirstOrDefault(item => item.Id == id); //stage 2
    }

    /// <summary>
    /// Retrieves a delivery by a specified filter.
    /// </summary>
    public Delivery? Read(Func<Delivery, bool> filter) // stage 2
    {
        return DataSource.Deliverys.FirstOrDefault(item => filter(item));
    }

    ///// <summary>
    ///// Retrieves all deliveries from the data source.
    ///// </summary>
    //public List<Delivery> ReadAll()
    //{
    //    return new List<DO.Delivery>(DataSource.Deliverys);
    //}

    /// <summary>
    /// Retrieves filtered deliveries from the data source.
    /// </summary>
    public IEnumerable<Delivery> ReadAll(Func<Delivery, bool>? filter = null) //stage 2
    {
        return filter != null
            ? from item in DataSource.Deliverys
              where filter(item)
              select item
            : from item in DataSource.Deliverys
              select item;
    }

    /// <summary>
    /// Updates an existing delivery in the data source with the specified delivery details.
    /// </summary>
    /// <param name="id">The unique identifier of the delivery to be deleted.</param>
    /// <exception cref="DalDoesNotExistException">In case Delivery with the specified ID doesn't exist.</exception>
    public void Update(Delivery item)
    {
        foreach (var delivery in DataSource.Deliverys)
        {
            if (delivery.Id == item.Id)
            {
                DataSource.Deliverys.Remove(delivery);
                DataSource.Deliverys.Add(item);
                return;
            }
        }
        throw new DalDoesNotExistException($"Delivery with ID={item.Id} doesn't exist");
    }

    /// <summary>
    /// Deletes a delivery from the data source by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the delivery to be deleted.</param>
    /// <exception cref="DalDoesNotExistException">In case Delivery with the specified ID doesn't exist.</exception>
    public void Delete(int id)
    {
        foreach (var delivery in DataSource.Deliverys)
        {
            if (delivery.Id == id)
            {
                DataSource.Deliverys.Remove(delivery);
                return;
            }
        }
        throw new DalDoesNotExistException($"Delivery with ID={id} doesn't exist");
    }

    /// <summary>
    /// Deletes all deliveries from the data source.
    /// </summary>
    public void DeleteAll()
    {
        DataSource.Deliverys.Clear();
    }

}
