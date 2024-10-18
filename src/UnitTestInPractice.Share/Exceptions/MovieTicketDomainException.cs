namespace UnitTestInPractice.Shared.Exceptions;
  
public class MovieTicketDomainException : Exception
{
    public MovieTicketDomainException()
    { }

    public MovieTicketDomainException(string message)
        : base(message)
    { }

    public MovieTicketDomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}

