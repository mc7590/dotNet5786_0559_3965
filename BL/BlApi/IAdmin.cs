
using Helpers;

namespace BlApi;

public interface IAdmin
{

    /// <summary>
    /// Reset all configuration data to its initial value
    /// </summary>
    void ResetDB();

    /// <summary>
    /// Initialize database: reset, then initialize
    /// </summary>
    void InitializeDB();

    /// <summary>
    /// Returns the system clock
    /// </summary>
    DateTime GetClock();

    /// <summary>
    /// Advance the system clock by the appropriate time unit (minute, hour, day, month, year)
    /// </summary>
    void ForwardClock(BO.EnumTimeUnit unit);

    /// <summary>
    /// Returns the values ​​of BO.Config
    /// </summary>
    BO.Config GetConfig();

    /// <summary>
    /// Updates all configuration variables
    /// </summary>
    void SetConfig(BO.Config config);

}
