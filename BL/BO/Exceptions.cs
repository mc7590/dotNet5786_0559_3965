
namespace BO;

/// <summary>
/// Exception thrown when a requested BL entity does not exist.
/// </summary>
[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
}
