//OrderImplementation.cs
namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class OrderImplementation : IOrder
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
        //return DataSource.Orders.Find(item => item.Id == id); //stage 1
        return DataSource.Orders.FirstOrDefault(item => item.Id == id); //stage 2
    }

    /// <summary>
    /// Retrieves an order by a specified filter.
    /// </summary>
    public Order? Read(Func<Order, bool> filter) // stage 2
    {
        return DataSource.Orders.FirstOrDefault(item => filter(item));
    }

    ///// <summary>
    ///// Retrieves all Orders from the data source.
    ///// </summary>
    //public List<Order> ReadAll()
    //{
    //    return new List<DO.Order>(DataSource.Orders);
    //}

    /// <summary>
    /// Retrieves filtered orders from the data source.
    /// </summary>
    public IEnumerable<Order> ReadAll(Func<Order, bool>? filter = null) //stage 2
    {
        return filter != null
            ? from item in DataSource.Orders
              where filter(item)
              select item
            : from item in DataSource.Orders
              select item;
    }

    /// <summary>
    /// Updates an existing order in the data source with the specified order details.
    /// </summary>
    /// <param name="item">The order containing updated details. The order must have a valid ID that matches an existing order in the data
    /// source.</param>
    /// <exception cref="DalDoesNotExistException">In case Order with the specified ID doesn't exist.</exception>
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
        throw new DalDoesNotExistException($"Order with ID={item.Id} doesn't exist");
    }

    /// <summary>
    /// Deletes an order with the specified identifier from the data source.
    /// </summary>
    /// <param name="id">The unique identifier of the order to be deleted.</param>
    /// <exception cref="DalDoesNotExistException">In case Order with the specified ID doesn't exist.</exception>
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
        throw new DalDoesNotExistException($"Order with ID={id} doesn't exist");
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
