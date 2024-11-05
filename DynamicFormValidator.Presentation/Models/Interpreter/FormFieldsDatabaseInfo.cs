using DynamicFormValidator.Presentation.Models.Entities.Forms;
using DynamicFormValidator.Presentation.Models.Enums;

namespace DynamicFormValidator.Presentation.Models.Interpreter;

public class FormFieldsDatabaseInfo
{
    public string PrimaryKeyName { get; set; }
    public int PrimaryKeyColumnId { get; set; }
    public Dictionary<int,DataType> Types { get; init; }
    public Dictionary<int,string> Columns { get; init; }
    public string TableName { get; init; }
    
    
    private FormFieldsDatabaseInfo()
    {
        
    }

    public FormFieldsDatabaseInfo(Form form)
    {
        Types = form.DatabaseInfo.ToDictionary(f => f.Id, f => (DataType)f.Type);
        Columns = form.DatabaseInfo.ToDictionary(f => f.Id, f => f.ColumnName);
        TableName = form.DatabaseInfo.First().Table;
        var primaryKey = form.DatabaseInfo.First(f => f.IsPrimaryKey);
        PrimaryKeyName = primaryKey.ColumnName;
        PrimaryKeyColumnId = primaryKey.Id;
    }
}