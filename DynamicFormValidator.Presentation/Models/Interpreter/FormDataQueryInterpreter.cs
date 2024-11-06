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
        IDbConnection conn = null;
        IDbTransaction transaction = null;
        try
        {
            conn = _conn.GetConnection();
            transaction = conn.BeginTransaction(IsolationLevel.Snapshot);

            var result = await InsertFormData(formDto, form, conn, transaction);
            if (!result)
            {
                transaction.Rollback();
            }
            else
            {
                transaction.Commit();
            }


            return result;
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

    public async Task<bool> InsertFormData(FormDto formDto, Form form, IDbConnection conn,
        IDbTransaction transaction = null)
    {
        var formDataBaseInfo = new FormFieldsDatabaseInfo(form);
        StringBuilder query = new StringBuilder();
        query.Append("INSERT INTO ");
        query.Append(formDataBaseInfo.TableName);
        query.Append(" (");
        int index = 0;
        int parametersAdded = 0;
        foreach (var fieldKey in formDto.Fields.Keys)
        {
            if(formDataBaseInfo.IgnoreOnInsert[fieldKey])
            {
                continue;
            }
            
            if (parametersAdded > 0)
            {
                query.Append(", ");
            }
            query.Append(formDataBaseInfo.Columns[fieldKey]);
            parametersAdded++;
            index++;
        }

        query.Append(") VALUES(");

        index = 0;
        parametersAdded = 0;
        
        DynamicParameters parameters = new DynamicParameters();
        foreach (var fieldKey in formDto.Fields.Keys)
        {
            if(formDataBaseInfo.IgnoreOnInsert[fieldKey])
            {
                continue;
            }
            
            parameters.Add($"@{index}", GetFieldValueType(formDto.Fields[fieldKey], formDataBaseInfo.Types[fieldKey]));
            if (parametersAdded > 0)
            {
                query.Append(", ");
            }
            
            parametersAdded++;
            query.Append($"@{index}");

            index++;
        }

        query.Append(") RETURNING ");
        query.Append(formDataBaseInfo.PrimaryKeyName);
        query.Append(" ;");

        int rowsAffected = await conn.ExecuteAsync(query.ToString(), parameters, transaction);
        return rowsAffected != 0;
    }

    public async Task<bool> UpdateFormData(FormDto formDto, Form form)
    {
        IDbConnection conn = null;
        IDbTransaction transaction = null;

        try
        {
            conn = _conn.GetConnection();
            transaction = conn.BeginTransaction(IsolationLevel.Snapshot);
            var result = await UpdateFormData(formDto, form, conn, transaction);
            if (result)
            {
                transaction.Commit();
            }
            else
            {
                transaction.Rollback();
            }

            return result;
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

    private async Task<bool> UpdateFormData(FormDto formDto, Form form, IDbConnection conn,
        IDbTransaction transaction = null)
    {
        var formDataBaseInfo = new FormFieldsDatabaseInfo(form);
        if (!formDto.Fields.ContainsKey(formDataBaseInfo.PrimaryKeyColumnId))
        {
            return false;
        }

        StringBuilder query = new StringBuilder();
        query.Append("UPDATE ");
        query.Append(formDataBaseInfo.TableName);
        query.Append(" SET ");
        DynamicParameters parameters = new DynamicParameters();
        
        int index = 0;
        int parametersAdded = 0;
        
        foreach (var key in formDto.Fields.Keys)
        {
            if (key == formDataBaseInfo.PrimaryKeyColumnId)
            {
                continue;
            }

            if (formDataBaseInfo.IgnoreOnUpdate[key])
            {
                continue;
            }


            query.Append(formDataBaseInfo.Columns[key]);
            parameters.Add($"@{index}", GetFieldValueType(formDto.Fields[key], formDataBaseInfo.Types[key]));
            
            parametersAdded++;
            
            if(parametersAdded > 1)
            {
                query.Append(", ");
            }
            
            query.Append($"=@{index}");

            index++;
        }

        query.Append(" WHERE ");
        query.Append(formDataBaseInfo.PrimaryKeyName);
        query.Append("=@id");
        parameters.Add("@id", GetFieldValueType(formDto.Fields[formDataBaseInfo.PrimaryKeyColumnId]
            , formDataBaseInfo.Types[formDataBaseInfo.PrimaryKeyColumnId]));

        int rowsAffected = await conn.ExecuteAsync(query.ToString(), parameters, transaction);
        return rowsAffected != 0;
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

    public async Task<object> SelectFormData(string entityId, Form form)
    {
        if (string.IsNullOrWhiteSpace(entityId))
        {
            throw new NullReferenceException("EntityId is null or empty");
        }

        if (form is null)
        {
            throw new NullReferenceException("Form is null");
        }

        IDbConnection conn = null;
        IDbTransaction transaction = null;
        try
        {
            conn = _conn.GetConnection();
            transaction = conn.BeginTransaction(IsolationLevel.Snapshot);
            object result = await GetFormData(entityId, form, conn);
            transaction.Commit();

            return result;
        }
        catch
        {
            transaction?.Rollback();
            return null;
        }
        finally
        {
            conn?.Close();
        }
    }

    private async Task<object> GetFormData(string entityId, Form form, IDbConnection conn)
    {
        var formDatabaseInfo = new FormFieldsDatabaseInfo(form);
        StringBuilder columns = new StringBuilder();
        string query = string.Format("SELECT {0} FROM {1} WHERE {2}=@id",
            string.Join(',', formDatabaseInfo.Columns.Values), formDatabaseInfo.TableName, formDatabaseInfo.PrimaryKeyName);
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@id", GetFieldValueType(entityId, formDatabaseInfo.Types[formDatabaseInfo.PrimaryKeyColumnId]));

        object result = await conn.QueryFirstOrDefaultAsync(query, parameters);
        return result;
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