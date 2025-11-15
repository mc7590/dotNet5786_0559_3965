using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;
using DalApi;
using DO;
using System.Xml.Linq;

/// <summary>
/// Courier Implementation for DalXml
/// </summary>

internal class CourierImplementation : ICourier
{
    //working by 2nd method

    private static Courier getCourier(XElement c)
    {
        return new Courier()
        {
            Id = c.ToIntNullable("Id") ?? throw new FormatException("can't convert id"),
            Name = (string?)c.Element("Name") ?? "",
            CourierPhone = (string?)c.Element("CourierPhone") ?? "",
            Email = (string?)c.Element("Email") ?? "",
            Password = (string?)c.Element("Password") ?? "",
            Active = (bool?)c.Element("Active") ?? false,
            DeliveryMethod = c.ToEnumNullable<EnumDeliveryMethod>("DeliveryMethod") ?? EnumDeliveryMethod.Car,
            StartedWorking = c.ToDateTimeNullable("StartedWorking") ?? DateTime.Now,
            MaxPersonalDistance = c.ToDoubleNullable("MaxPersonalDistance")
        };
    }
    private static IEnumerable<XElement> createCourierElement(Courier c) // Converts Courier to XML elements (without <Courier> tag)
    {
        return new XElement[] {
            new XElement("Id", c.Id),
            new XElement("Name", c.Name),
            new XElement("CourierPhone", c.CourierPhone),
            new XElement("Email", c.Email),
            new XElement("Password", c.Password),
            new XElement("Active", c.Active),
            new XElement("DeliveryMethod", c.DeliveryMethod),
            new XElement("StartedWorking", c.StartedWorking),
            new XElement("MaxPersonalDistance", c.MaxPersonalDistance)
        };
    }
    public void Create(Courier item)
    {
        XElement courierRootElem = XMLTools.LoadListFromXMLElement(Config.s_couriers_xml);
        if (courierRootElem.Elements().Any(c => (int?)c.Element("Id") == item.Id))
            throw new DO.DalAlreadyExistsException($"Courier with ID={item.Id} already exists");
        courierRootElem.Add(new XElement("Courier", createCourierElement(item))); //create tag Courier
        XMLTools.SaveListToXMLElement(courierRootElem, Config.s_couriers_xml);
    }

    public void Delete(int id)
    {
        XElement courierRootElem = XMLTools.LoadListFromXMLElement(Config.s_couriers_xml);
        (courierRootElem.Elements().FirstOrDefault(c => (int?)c.Element("Id") == id) ??
            throw new DO.DalDoesNotExistException($"Courier with ID={id} doesn't exist")).Remove();
        XMLTools.SaveListToXMLElement(courierRootElem, Config.s_couriers_xml);
    }

    public void DeleteAll()
    {
        XElement courierRootElem = XMLTools.LoadListFromXMLElement(Config.s_couriers_xml);
        courierRootElem.Elements().Remove();
        XMLTools.SaveListToXMLElement(courierRootElem, Config.s_couriers_xml);
    }

    public Courier? Read(int id)
    {
        XElement? courierElem =
XMLTools.LoadListFromXMLElement(Config.s_couriers_xml).Elements().FirstOrDefault(co => (int?)co.Element("Id") == id);
        return courierElem is null ? null : getCourier(courierElem);
    }

    public Courier? Read(Func<Courier, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(Config.s_couriers_xml).Elements().Select(c => getCourier(c)).FirstOrDefault(filter);
    }

    public IEnumerable<Courier> ReadAll(Func<Courier, bool>? filter = null)
    {
        XElement courierRootElem = XMLTools.LoadListFromXMLElement(Config.s_couriers_xml);
        IEnumerable<Courier> couriers = courierRootElem.Elements().Select(c => getCourier(c));
        return filter == null ? couriers : couriers.Where(filter);
    }

    public void Update(Courier item)
    {
        XElement courierRootElem = XMLTools.LoadListFromXMLElement(Config.s_couriers_xml);
        (courierRootElem.Elements().FirstOrDefault(c => (int?)c.Element("Id") == item.Id) ??
            throw new DO.DalDoesNotExistException($"Courier with ID={item.Id} doesn't exist")).Remove();
        courierRootElem.Add(new XElement("Courier", createCourierElement(item))); //create tag Courier
        XMLTools.SaveListToXMLElement(courierRootElem, Config.s_couriers_xml);
    }
}
