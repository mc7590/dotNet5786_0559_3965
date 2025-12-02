
namespace DO;

/// <summary>
/// Exception thrown when a requested data-access-layer(DAL) entity does not exist.
/// </summary>
[Serializable]
public class DalDoesNotExistException : Exception
{
    public DalDoesNotExistException(string? message) : base(message) { }
}

/// <summary>
/// Exception thrown when attempting to create a data-access-layer(DAL) entity that already exists.
/// </summary>
[Serializable]
public class DalAlreadyExistsException : Exception
{
    public DalAlreadyExistsException(string? message) : base(message) { }
}

/// <summary>
/// Exception thrown when invalid input is provided to a data-access-layer(DAL) method (in DalTest).
/// </summary>
[Serializable]
public class DalInvalidInputException : Exception
{
    public DalInvalidInputException(string? message) : base(message) { }
}

[Serializable]
public class DalXMLFileLoadCreateException : Exception
{   
    public DalXMLFileLoadCreateException(string? message) : base(message) { }
}