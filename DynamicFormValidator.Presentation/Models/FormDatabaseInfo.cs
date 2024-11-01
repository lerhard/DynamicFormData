namespace DynamicFormValidator.Presentation.Models;

public class FormDatabaseInfo
{
   public int FormId { get; set; } 
   public string TableName { get; set; }
   public List<FormDatabaseKey> Columns { get; set; }
}

public class FormDatabaseKey
{
   public int KeyId { get; set; }
   public int KeyType { get; set; }
   public string KeyColumnName { get; set; }
}