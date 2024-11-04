using System.Data;
using DynamicFormValidator.Presentation.Models.Entities.Forms;
using DynamicFormValidator.Presentation.Repositories.Forms;
using DynamicFormValidator.Presentation.Services.Connection;

namespace DynamicFormValidator.Presentation.Services.Forms;

public  class FormsService: IFormsService
{
    private readonly IConnectionService _conn;
    private readonly IFormsRepository _formsRepository;

    public FormsService(IConnectionService connectionService, IFormsRepository formsRepository)
    {
        _conn = connectionService;
        _formsRepository = formsRepository;
    }
    public async Task<Form> GetForm(int id)
    {
        IDbConnection conn = null;
        
        try
        {
            conn = _conn.GetConnection();
            var form = await _formsRepository.GetForm(id, conn);
            return form;
        }
        catch
        {
            return null;
        }
        finally
        {
           conn?.Close(); 
        }
    }
}