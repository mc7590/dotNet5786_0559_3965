using System.Collections;
using System.Collections.Generic;
namespace PL;
 
public class MethodDeliveryCollection : IEnumerable<BO.EnumDeliveryMethod>
{
    static readonly IEnumerable<BO.EnumDeliveryMethod> s_enums =
        (IEnumerable<BO.EnumDeliveryMethod>)Enum.GetValues(typeof(BO.EnumDeliveryMethod));

    public IEnumerator<BO.EnumDeliveryMethod> GetEnumerator() => s_enums.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}