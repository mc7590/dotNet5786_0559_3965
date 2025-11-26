
namespace BO;

/// <summary>
/// Exception thrown when a requested BL entity does not exist.
/// </summary>
[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
}

/// <summary>
/// Exception thrown when a user attempts to perform an action they are not authorized for.
/// </summary>
public class UnauthorizedAccessException : Exception
{
    public UnauthorizedAccessException(string? message) : base(message) { }
}
