//check if new exceptions are okay!
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

/// <summary>
/// Represents an exception that is thrown when an attempt is made to create a BL entity that already exists.
/// </summary>
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
}

/// <summary>
/// Represents an exception that is thrown when an invalid operation is attempted in the BL layer.
/// </summary>
public class BlInvalidOperationException : Exception
{
    public BlInvalidOperationException(string? message) : base(message) { }
}
/// <summary>
/// Represents an exception that is thrown when a user is not authorized to perform a specific action in the BL layer.
/// </summary>
public class BlUnauthorizedException : Exception
{
    public BlUnauthorizedException(string? message) : base(message) { }
}