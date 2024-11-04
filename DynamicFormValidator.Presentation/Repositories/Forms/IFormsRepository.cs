using System.Data;
using DynamicFormValidator.Presentation.Models;
using DynamicFormValidator.Presentation.Models.Entities.Forms;

namespace DynamicFormValidator.Presentation.Repositories.Forms;

public interface IFormsRepository
{
    Task<Form?> GetForm(int id,IDbConnection conn, IDbTransaction transaction = null);
}