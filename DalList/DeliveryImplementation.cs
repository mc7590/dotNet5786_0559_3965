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
        foreach (var delivery in DataSource.Deliverys)
        {
            if (delivery.Id == id)
                return delivery;
        }
        return null;
    }

    /// <summary>
    /// Retrieves all deliveries from the data source.
    /// </summary>
    /// <returns>A list of objects representing all deliverys currently stored in the data source.</returns>
    public List<Delivery> ReadAll()
    {
        return new List<DO.Delivery>(DataSource.Deliverys);
    }

    /// <summary>
    /// Updates an existing delivery in the data source with the specified delivery details.
    /// </summary>
    /// <param name="id">The unique identifier of the delivery to be deleted.</param>
    /// <exception cref="NotImplementedException">Thrown if no delivery with the specified <paramref name="id"/> exists in the data source.</exception>
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
        throw new NotImplementedException("An object of type Delivery with such ID does not exist.");
    }

    /// <summary>
    /// Deletes a delivery from the data source by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the delivery to be deleted.</param>
    /// <exception cref="NotImplementedException">Thrown if no delivery with the specified <paramref name="id"/> exists in the data source.</exception>
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
        throw new NotImplementedException("An object of type Delivery with such ID does not exist.");
    }

    /// <summary>
    /// Deletes all deliveries from the data source.
    /// </summary>
    public void DeleteAll()
    {
        DataSource.Deliverys.Clear();
    }



}
