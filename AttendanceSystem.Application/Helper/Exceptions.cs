namespace AttendanceSystem.Application;

public abstract class BaseException : Exception
{
    protected BaseException(string message)
        : base(message)
    {
    }
}


public class NotFoundException : BaseException
{
    public NotFoundException(string message)
        : base(message)
    {
    }
}


public class ValidationException : BaseException
{
    public ValidationException(string message)
        : base(message)
    {
    }
}


public class UnauthorizedException : BaseException
{
    public UnauthorizedException(string message)
        : base(message)
    {
    }
}


public class ForbiddenException : BaseException
{
    public ForbiddenException(string message)
        : base(message)
    {
    }
}
