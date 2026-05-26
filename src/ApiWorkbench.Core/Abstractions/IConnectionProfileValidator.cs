using ApiWorkbench.Core.Models;

namespace ApiWorkbench.Core.Abstractions;

public interface IConnectionProfileValidator
{
    ConnectionProfileValidationResult Validate(ConnectionProfile profile);
}
