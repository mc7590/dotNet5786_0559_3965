using DalApi;

namespace Helpers;

internal static class DeliveryManager
{
    private static IDal s_dal = Factory.Get; //stage 4

    //GetDelivery ?
    internal static BO.DeliveryPerOrderInList GetDeliveryPerOrderInList(int orderId)
    {
        
    }

}
