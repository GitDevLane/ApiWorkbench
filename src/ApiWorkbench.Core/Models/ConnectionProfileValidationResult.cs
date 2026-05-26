namespace ApiWorkbench.Core.Models;

public sealed class ConnectionProfileValidationResult
{
    public IReadOnlyList<string> Errors { get; }

    public bool IsValid => Errors.Count == 0;

    private ConnectionProfileValidationResult(IReadOnlyList<string> errors)
    {
        Errors = errors;
    }

    public static ConnectionProfileValidationResult Success()
    {
        return new ConnectionProfileValidationResult(Array.Empty<string>());
    }

    public static ConnectionProfileValidationResult Failure(IEnumerable<string> errors)
    {
        return new ConnectionProfileValidationResult(errors.ToArray());
    }
}
