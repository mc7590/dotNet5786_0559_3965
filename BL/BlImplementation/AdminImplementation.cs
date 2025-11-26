using BlApi;
using BO;
using Helpers;

namespace BlImplementation;

internal class AdminImplementation : IAdmin
{
    public void ForwardClock(EnumTimeUnit unit)
    {
       switch (unit)
        {
            case EnumTimeUnit.Minute:
                AdminManager.UpdateClock(AdminManager.Now.AddMinutes(1));
                break;
            case EnumTimeUnit.Hour:
                AdminManager.UpdateClock(AdminManager.Now.AddHours(1));
                break;
            case EnumTimeUnit.Day:
                AdminManager.UpdateClock(AdminManager.Now.AddDays(1));
                break;
            case EnumTimeUnit.Month:
                AdminManager.UpdateClock(AdminManager.Now.AddMonths(1));
                break;
            case EnumTimeUnit.Year:
                AdminManager.UpdateClock(AdminManager.Now.AddYears(1));
                break;
            default:
                throw new BlInvalidInputException($"Invalid time unit: {unit}");
        }
    }

    public DateTime GetClock()=> AdminManager.Now;

    public Config GetConfig()=> AdminManager.GetConfig();

    public void InitializeDB() => AdminManager.InitializeDB();

    public void ResetDB() => AdminManager.ResetDB();

    public void SetConfig(Config config) => AdminManager.SetConfig(config);
}
