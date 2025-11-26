
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
public class BlUnauthorizedAccessException : Exception
{
    public BlUnauthorizedAccessException(string? message) : base(message) { }
}

/// <summary>
/// Exception thrown when invalid input is provided to a BL method.
/// </summary>
public class BlInvalidInputException : Exception
{
    public BlInvalidInputException(string? message) : base(message) { }
}
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
}
public class BlInvalidOperationException : Exception
{
    public BlInvalidOperationException(string? message) : base(message) { }
}