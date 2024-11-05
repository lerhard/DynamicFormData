using Dapper;
using DynamicFormValidator.Presentation.Models.Enums;

namespace DynamicFormValidator.Presentation.Models.Entities.Forms;

public class FormInterpreterData
{
    public string Query { get; set; }
    public DynamicParameters Parameters { get; set; }
    public QueryType QueryType { get; set; }
}