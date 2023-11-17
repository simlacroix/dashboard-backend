namespace Dashboard.Exceptions;

/*
 * Custom exception used by validators.
 * Should be used for listing invalid parameter from requests.
 */
public class InvalidParameterException : ApplicationException
{
    public InvalidParameterException(List<string> exceptionMessages) : base(String.Join(" | ", exceptionMessages.ToArray()))
    {

    }
}