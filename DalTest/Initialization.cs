//Initialization of DAL tests
namespace DalTest;
using DalApi;
using DO;

public static class Initialization
{
    private static ICourier? s_dalCourier; //stage 1
    private static IOrder? s_dalOrder; //stage 1
    private static IDelivery? s_dalDelivery; //stage 1
    private static IConfig? s_dalConfig; //stage 1

    /// <summary>
    /// Random number generator to initialize test data
    /// </summary>
    private static readonly Random s_rand = new();
}
