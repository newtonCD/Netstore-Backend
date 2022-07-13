namespace Netstore.Common.Results;

public interface IResultError
{
    string Error { get; }
    string Code { get; }
}