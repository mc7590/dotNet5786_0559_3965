
namespace Dal;
using DalApi;
using DO;
using System.Runtime.CompilerServices;

internal class DeliveryImplementation : IDelivery
{
    //working by 1st method

    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public void Create(Delivery item)
    {
        int nextId = Config.NextDeliveryId;
        Delivery newItem = item with { Id = nextId };
        List<Delivery> Deliveries = XMLTools.LoadListFromXMLSerializer<Delivery>(Config.s_deliveries_xml);
        Deliveries.Add(newItem);
        XMLTools.SaveListToXMLSerializer(Deliveries, Config.s_deliveries_xml);
    }

    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public void Delete(int id)
    {
        List<Delivery> Deliveries = XMLTools.LoadListFromXMLSerializer<Delivery>(Config.s_deliveries_xml);
        if (Deliveries.RemoveAll(it => it.Id == id) == 0)
            throw new DalDoesNotExistException($"Delivery with ID={id} does Not exist");
        XMLTools.SaveListToXMLSerializer(Deliveries, Config.s_deliveries_xml);
    }

    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public void DeleteAll()
    {
        XMLTools.SaveListToXMLSerializer(new List<Delivery>(), Config.s_deliveries_xml);
    }

    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public Delivery? Read(int id)
    {
        List<Delivery> Deliveries = XMLTools.LoadListFromXMLSerializer<Delivery>(Config.s_deliveries_xml);
        return Deliveries.FirstOrDefault(it => it.Id == id);
    }

    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public Delivery? Read(Func<Delivery, bool> filter)
    {
        List<Delivery> Deliveries = XMLTools.LoadListFromXMLSerializer<Delivery>(Config.s_deliveries_xml);
        return Deliveries.FirstOrDefault(item => filter(item));

    }

    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public IEnumerable<Delivery> ReadAll(Func<Delivery, bool>? filter = null)
    {
        List<Delivery> Deliveries = XMLTools.LoadListFromXMLSerializer<Delivery>(Config.s_deliveries_xml);
        return filter != null
            ? from item in Deliveries
              where filter(item)
              select item
            : from item in Deliveries
              select item;
    }

    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public void Update(Delivery item)
    {
        List<Delivery> Deliveries = XMLTools.LoadListFromXMLSerializer<Delivery>(Config.s_deliveries_xml);
        if (Deliveries.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistException($"Delivery with ID={item.Id} does Not exist");
        Deliveries.Add(item);
        XMLTools.SaveListToXMLSerializer(Deliveries, Config.s_deliveries_xml);
    }

}
