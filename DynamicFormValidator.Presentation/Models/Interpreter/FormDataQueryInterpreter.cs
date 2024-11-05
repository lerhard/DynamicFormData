using System.Data;
using System.Text;
using Dapper;
using DynamicFormValidator.Presentation.Models.DTOs;
using DynamicFormValidator.Presentation.Models.Entities.Forms;
using DynamicFormValidator.Presentation.Models.Enums;
using DynamicFormValidator.Presentation.Services.Connection;

namespace DynamicFormValidator.Presentation.Models.Interpreter;

public class FormDataQueryInterpreter : IFormDataQueryInterpreter
{
    private readonly IConnectionService _conn;

    public FormDataQueryInterpreter(IConnectionService conn)
    {
        _conn = conn;
    }

    public async Task<bool> InsertFormData(FormDto formDto, Form form)
    {
        var formDataBaseInfo = new FormFieldsDatabaseInfo(form);
        StringBuilder query = new StringBuilder();
        query.Append("INSERT INTO ");
        query.Append(formDataBaseInfo.TableName);
        query.Append(" (");
        int index = 0;
        foreach (var fieldKey in formDto.Fields.Keys)
        {
            query.Append(formDataBaseInfo.Columns[fieldKey]);
            if (index < formDto.Fields.Count - 1)
            {
                query.Append(", ");
            }

            index++;
        }

        query.Append(") VALUES(");

        index = 0;
        DynamicParameters parameters = new DynamicParameters();
        foreach (var fieldKey in formDto.Fields.Keys)
        {
            query.Append($"@{index}");
            parameters.Add($"@{index}", GetFieldValueType(formDto.Fields[fieldKey], formDataBaseInfo.Types[fieldKey]));
            if (index < formDto.Fields.Count - 1)
            {
                query.Append(", ");
            }

            index++;
        }

        query.Append(") RETURNING ");
        query.Append(formDataBaseInfo.PrimaryKeyName);
        query.Append(" ;");

        IDbConnection conn = null;
        IDbTransaction transaction = null;

        try
        {
            conn = _conn.GetConnection();
            transaction = conn.BeginTransaction();
            int rowsAffected = await conn.ExecuteAsync(query.ToString(), parameters, transaction);
            if (rowsAffected == 0)
            {
                transaction.Rollback();
                return false;
            }


            transaction.Commit();
            return true;
        }
        catch
        {
            transaction?.Rollback();
            return false;
        }
        finally
        {
            conn?.Close();
        }
    }

    public async Task<bool> DeleteFormData(string entityId, Form form)
    {
        var formDatabaseInfo = new FormFieldsDatabaseInfo(form);
        string query = string.Format("DELETE FROM {0} WHERE {1}=@id"
            , formDatabaseInfo.TableName, formDatabaseInfo.PrimaryKeyName);
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@id", GetFieldValueType(entityId, formDatabaseInfo.Types[formDatabaseInfo.PrimaryKeyColumnId]));

        IDbConnection conn = null;
        IDbTransaction transaction = null;

        try
        {
            conn = _conn.GetConnection();
            transaction = conn.BeginTransaction(IsolationLevel.Snapshot);
            int rowsAffected = await conn.ExecuteAsync(query, parameters, transaction);
            if (rowsAffected == 0)
            {
                transaction.Rollback();
                return false;
            }

            transaction.Commit();
            return true;
        }
        catch
        {
            transaction?.Rollback();
            return false;
        }
        finally
        {
            conn?.Close();
        }
    }

    private object GetFieldValueType(string value, DataType type)
    {
        switch (type)
        {
            case DataType.INT:
                return int.Parse(value);
            case DataType.DATE:
                return DateTime.Parse(value).Date;
            case DataType.UUID:
                return Guid.Parse(value);
            case DataType.FLOAT:
                return float.Parse(value);
            case DataType.DOUBLE:
                return double.Parse(value);
            case DataType.BOOLEAN:
                return bool.Parse(value);
            case DataType.DECIMAL:
                return decimal.Parse(value);
            case DataType.DATETIME:
                return DateTime.Parse(value);
            case DataType.STRING:
            default:
                return value;
        }
    }
}