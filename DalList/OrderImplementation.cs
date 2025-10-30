//OrderImplementation.cs
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class OrderImplementation : IOrder
{
    /// <summary>
    /// Adds a new order to the data source with a unique identifier.
    /// </summary>
    /// <remarks>The method assigns a unique identifier to the order before adding it to the data source. 
    /// Ensure that the order does not already exist in the data source to prevent duplicates.</remarks>
    /// <param name="item">The order to be added. The order must not be null and should not have an existing identifier.</param>
    public void Create(Order item)
    {
        int nextId = Config.NextOrderId;
        Order newItem = item with { Id = nextId };
        DataSource.Orders.Add(newItem);
    }

    /// <summary>
    /// Retrieves an order by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order to retrieve.</param>
    public Order? Read(int id)
    {
        foreach (var order in DataSource.Orders)
        {
            if (order.Id == id)
                return order;
        }
        return null;
    }

    /// <summary>
    /// Retrieves all orders from the data source.
    /// </summary>
    /// <returns>A list of objects representing all orders currently stored in the data source.</returns>
    public List<Order> ReadAll()
    {
        return new List<DO.Order>(DataSource.Orders);
    }

    /// <summary>
    /// Updates an existing order in the data source with the specified order details.
    /// </summary>
    /// <param name="item">The order containing updated details. The order must have a valid ID that matches an existing order in the data
    /// source.</param>
    /// <exception cref="NotImplementedException">Thrown if an order with the specified ID does not exist in the data source.</exception>
    public void Update(Order item)
    {
        foreach (var order in DataSource.Orders)
        {
            if (order.Id == item.Id)
            {
                DataSource.Orders.Remove(order);
                DataSource.Orders.Add(item);
                return;
            }
        }
        throw new NotImplementedException("An object of type Order with such ID does not exist.");
    }

    /// <summary>
    /// Deletes an order with the specified identifier from the data source.
    /// </summary>
    /// <param name="id">The unique identifier of the order to be deleted.</param>
    /// <exception cref="NotImplementedException">Thrown if no order with the specified <paramref name="id"/> exists in the data source.</exception>
    public void Delete(int id)
    {
        foreach (var order in DataSource.Orders)
        {
            if (order.Id == id)
            {
                DataSource.Orders.Remove(order);
                return;
            }
        }
        throw new NotImplementedException("An object of type Order with such ID does not exist.");
    }

    /// <summary>
    /// Deletes all orders from the data source.
    /// </summary>
    /// <remarks>This method clears the entire collection of orders, removing all existing entries.</remarks>
    public void DeleteAll()
    {
        DataSource.Orders.Clear();
    }


}
