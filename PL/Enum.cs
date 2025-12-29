using System.Collections;
using System.Collections.Generic;
namespace PL;

/// <summary>
/// for binding EnumDeliveryMethod in UI (ComboBox)
/// </summary>
public class MethodDeliveryCollection : IEnumerable<BO.EnumDeliveryMethod>
{
    static readonly IEnumerable<BO.EnumDeliveryMethod> s_enums =
        (IEnumerable<BO.EnumDeliveryMethod>)Enum.GetValues(typeof(BO.EnumDeliveryMethod));

    public IEnumerator<BO.EnumDeliveryMethod> GetEnumerator() => s_enums.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <summary>
/// for binding EnumOrderType in UI (ComboBox)
/// </summary>
public class OrderTypeCollection : IEnumerable<BO.EnumOrderType>
{
    static readonly IEnumerable<BO.EnumOrderType> s_enums =
        (IEnumerable<BO.EnumOrderType>)Enum.GetValues(typeof(BO.EnumOrderType));
    public IEnumerator<BO.EnumOrderType> GetEnumerator() => s_enums.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <summary>
/// for binding EnumOrderStatus in UI (ComboBox)
/// </summary>
public class OrderStatusCollection : IEnumerable<BO.EnumOrderStatus>
{
    static readonly IEnumerable<BO.EnumOrderStatus> s_enums =
        (IEnumerable<BO.EnumOrderStatus>)Enum.GetValues(typeof(BO.EnumOrderStatus));
    public IEnumerator<BO.EnumOrderStatus> GetEnumerator() => s_enums.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
/// <summary>
/// for binding EnumEndDeliveryStatus in UI (ComboBox)
/// </summary>
public class EndStatusCollection : IEnumerable<BO.EnumEndDeliveryStatus>
{
    static readonly IEnumerable<BO.EnumEndDeliveryStatus> s_enums =
        (IEnumerable<BO.EnumEndDeliveryStatus>)Enum.GetValues(typeof(BO.EnumEndDeliveryStatus));
    public IEnumerator<BO.EnumEndDeliveryStatus> GetEnumerator() => s_enums.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
