namespace BO;

/// <summary>
/// Exception thrown when a requested BL entity does not exist.
/// </summary>
[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }

}

/// <summary>
/// Represents an exception that is thrown when an attempt is made to create a BL entity that already exists.
/// </summary>
[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string message, Exception innerException)
                : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when invalid input is provided to a BL method.
/// </summary>
[Serializable]
public class BlInvalidInputException : Exception
{
    public BlInvalidInputException(string? message) : base(message) { }
    public BlInvalidInputException(string message, Exception innerException)
                : base(message, innerException) { }
}


/// <summary>
/// Represents an exception that is thrown when an invalid operation is attempted in the BL layer.
/// </summary>
[Serializable]
public class BlInvalidOperationException : Exception
{
    public BlInvalidOperationException(string? message) : base(message) { }
    public BlInvalidOperationException(string message, Exception innerException)
                : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when a user attempts to perform an action they are unauthorized for.
/// </summary>
[Serializable]
public class BlUnauthorizedException : Exception
{
    public BlUnauthorizedException(string? message) : base(message) { }
    public BlUnauthorizedException(string message, Exception innerException)
                : base(message, innerException) { }
}