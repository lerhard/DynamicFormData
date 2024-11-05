using System.Data;
using DynamicFormValidator.Presentation.Models.DTOs;
using DynamicFormValidator.Presentation.Models.Entities.Forms;
using DynamicFormValidator.Presentation.Models.Interpreter;
using DynamicFormValidator.Presentation.Models.Validators;
using DynamicFormValidator.Presentation.Repositories.Forms;
using DynamicFormValidator.Presentation.Services.Connection;
using FluentValidation.Results;

namespace DynamicFormValidator.Presentation.Services.Forms;

public class FormsService : IFormsService
{
    private readonly IConnectionService _conn;
    private readonly IFormsRepository _formsRepository;
    private readonly IFormDataQueryInterpreter _queryInterpreter;

    public FormsService(IConnectionService connectionService, IFormsRepository formsRepository,
        IFormDataQueryInterpreter queryInterpreter)
    {
        _conn = connectionService;
        _formsRepository = formsRepository;
        _queryInterpreter = queryInterpreter;
    }

    public async Task<Form> GetForm(int id)
    {
        IDbConnection conn = null;

        try
        {
            conn = _conn.GetConnection();
            var form = await GetForm(id, conn);
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

    private async Task<Form> GetForm(int id, IDbConnection conn, IDbTransaction transaction = null)
    {
        var form = await _formsRepository.GetForm(id, conn);
        if (form is null)
        {
            return null;
        }

        bool formHasSubForm = await _formsRepository.FormHasSubForm(id, conn);
        if (!formHasSubForm)
        {
            return form;
        }

        form.SubForms = new List<Form>();
        var subFormsIds = await _formsRepository.GetSubForms(id, conn);
        foreach (var subFormId in subFormsIds)
        {
            Form? subForm = await GetForm(subFormId, conn);
            if (subForm is not null)
            {
                form.SubForms.Add(subForm);
            }
        }

        return form;
    }

    public async Task<ValidationResult> ValidateForm(FormDto formDto)
    {
        var form = await GetForm(formDto.Id);
        if (form is null)
        {
            throw new DataException("Form not found");
        }

        FormValidator validator = new FormValidator(form);
        ValidationResult result = await validator.ValidateAsync(formDto);

        return result;
    }

    public async Task<ValidationResult> ValidateEntityId(string entityId)
    {
        var valresult = new ValidationResult();
        if (string.IsNullOrWhiteSpace(entityId))
        {
            var failure = new ValidationFailure("EntityId"
                , "Id is required to complete this operation.");
            
            valresult.Errors = new List<ValidationFailure>();
            valresult.Errors.Add(failure);
        }

        return valresult;
    }

    public async Task DeleteForm(int formId, string entityId)
    {
        var form = await GetForm(formId);
        if (form is null)
        {
            throw new DataException("Form not found");
        }
        
        bool deleteResult = await _queryInterpreter.DeleteFormData(entityId, form);
        if(!deleteResult)
        {
            throw new DataException("Failed to delete form data");
        }
        
    }

    public async Task SaveForm(FormDto formDto)
    {
        var form = await GetForm(formDto.Id);
        var insertResult = await _queryInterpreter.InsertFormData(formDto, form);
        if(!insertResult)
        {
            throw new DataException("Failed to save form data");
        }
        
    }
}