
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

internal class OrderImplementation : IOrder
{
    //working by 1st method

    public void Create(Order item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        List<Order> Orders = XMLTools.LoadListFromXMLSerializer<Order>(Config.s_orders_xml);
        if (Orders.RemoveAll(it => it.Id == id) == 0)
            throw new DalDoesNotExistException($"Order with ID={id} does Not exist");
        XMLTools.SaveListToXMLSerializer(Orders, Config.s_orders_xml);
    }


    public void DeleteAll()
    {
        XMLTools.SaveListToXMLSerializer(new List<Order>(), Config.s_orders_xml);
    }


    public Order? Read(int id)
    {
        List<Order> Orders = XMLTools.LoadListFromXMLSerializer<Order>(Config.s_orders_xml);
        return Orders.FirstOrDefault(it => it.Id == id);
    }

    public Order? Read(Func<Order, bool> filter)
    {
        List<Order> Orders = XMLTools.LoadListFromXMLSerializer<Order>(Config.s_orders_xml);
        return Orders.FirstOrDefault(item => filter(item));

    }

    public IEnumerable<Order> ReadAll(Func<Order, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Order item)
    {
        List<Order> Orders = XMLTools.LoadListFromXMLSerializer<Order>(Config.s_orders_xml);
        if (Orders.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistException($"Order with ID={item.Id} does Not exist");
        Orders.Add(item);
        XMLTools.SaveListToXMLSerializer(Orders, Config.s_orders_xml);
    }

}
