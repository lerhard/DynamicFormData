using System.Data;
using Dapper;
using DynamicFormValidator.Presentation.Models.Entities.Forms;

namespace DynamicFormValidator.Presentation.Repositories.Forms;

public class FormsRepository : IFormsRepository
{
    public async Task<Form?> GetForm(int id, IDbConnection conn, IDbTransaction transaction = null)
    {
        string query = @"SELECT id
                                ,name
                                ,created_at
                                ,updated_at
                                ,description
                                ,fields
                                ,database_info as databaseinfo
                                ,validation_info as validationinfo
                           FROM forms WHERE id = @id";
        return await conn.QueryFirstOrDefaultAsync<Form>(query, new { id }, transaction);
    }

    public async Task<bool> FormHasSubForm(int id, IDbConnection conn, IDbTransaction transaction = null)
    {
        string query = @"SELECT COUNT(*) > 0 FROM forms WHERE parent_id = @id";
        return await conn.ExecuteScalarAsync<bool>(query, new { id }, transaction);
    }

    public async Task<List<int>> GetSubForms(int id, IDbConnection conn, IDbTransaction transaction = null)
    {
        string query = @"SELECT id FROM forms WHERE parent_id = @id";
        return (await conn.QueryAsync<int>(query, new { id }, transaction)).ToList();
    }
}