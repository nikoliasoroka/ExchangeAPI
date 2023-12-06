namespace ExchangeAPI.Shared.Common.Models.Result.Implementations;

public class ErrorInfo
{
    private readonly List<string> _messagesList = new();

    public string Error { get; set; }

    public IReadOnlyCollection<string> Messages => _messagesList;

    public string? StackTrace { get; set; }

    public ErrorInfo() { }

    public ErrorInfo(string message, string? stackTrace)
    {
        Error = message;
        StackTrace = stackTrace;
        _messagesList.Add(message);
    }

    public void AddError(string message)
    {
        _messagesList.Add(message);
    }

    public void AddErrors(IEnumerable<string> collection)
    {
        _messagesList.AddRange(collection);
    }
}