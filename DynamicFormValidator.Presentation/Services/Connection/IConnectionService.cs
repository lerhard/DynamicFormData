using System.Data;

namespace DynamicFormValidator.Presentation.Services.Connection;

public interface IConnectionService
{
    IDbConnection GetConnection();
}