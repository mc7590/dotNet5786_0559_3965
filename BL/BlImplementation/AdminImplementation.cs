using BlApi;
using Helpers;

namespace BlImplementation;

internal class AdminImplementation : IAdmin
{
    public void ForwardClock(BO.EnumTimeUnit unit)
    {
       switch (unit)
        {
            case BO.EnumTimeUnit.Minute:
                AdminManager.UpdateClock(AdminManager.Now.AddMinutes(1));
                break;
            case BO.EnumTimeUnit.Hour:
                AdminManager.UpdateClock(AdminManager.Now.AddHours(1));
                break;
            case BO.EnumTimeUnit.Day:
                AdminManager.UpdateClock(AdminManager.Now.AddDays(1));
                break;
            case BO.EnumTimeUnit.Month:
                AdminManager.UpdateClock(AdminManager.Now.AddMonths(1));
                break;
            case BO.EnumTimeUnit.Year:
                AdminManager.UpdateClock(AdminManager.Now.AddYears(1));
                break;
            default:
                throw new BO.BlInvalidInputException($"Invalid time unit: {unit}");
        }
    }

    public DateTime GetClock()=> AdminManager.Now;

    public BO.Config GetConfig()=> AdminManager.GetConfig();

    public void InitializeDB() => AdminManager.InitializeDB();

    public void ResetDB() => AdminManager.ResetDB();

    public void SetConfig(BO.Config config) => AdminManager.SetConfig(config);
}
