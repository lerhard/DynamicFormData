using System.Data;
using Npgsql;

namespace DynamicFormValidator.Presentation.Services.Connection;

public class ConnectionService: IConnectionService
{
    private readonly IConfiguration _configuration;

    public ConnectionService(IConfiguration conf)
    {
       _configuration = conf; 
    }
    public IDbConnection GetConnection()
    {
        string connectionString = _configuration.GetConnectionString("Default");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
           throw new NullReferenceException("Connection string is null or empty"); 
        }
        
        var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        return connection;
    }
}