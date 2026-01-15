
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

    #region Stage 5
    void AddConfigObserver(Action configObserver);
    void RemoveConfigObserver(Action configObserver);
    void AddClockObserver(Action clockObserver);
    void RemoveClockObserver(Action clockObserver);
    #endregion Stage 5


    #region stage 7
    void StartSimulator(int interval); //stage 7
    void StopSimulator(); //stage 7

    #endregion stage 7
}
