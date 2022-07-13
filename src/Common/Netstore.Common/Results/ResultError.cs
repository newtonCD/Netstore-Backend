namespace Netstore.Common.Results;

public class ResultError : IResultError
{
    public ResultError()
    {
    }

    public ResultError(string error)
    {
        Error = error;
    }

    public ResultError(string error, string code)
        : this(error)
    {
        Code = code;
    }

    public string Error { get; private set; }

    public string Code { get; private set; }

    public override string ToString()
    {
        return $"Error[{Code}]: {Error}";
    }
}