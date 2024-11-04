using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicFormValidator.Presentation.Models.Entities.Forms;

[Table("forms")]
public class Form
{
   [Column("id")]
   public int Id { get; set; } 
   
   [Column("name")]
   public string Name { get; set; }
   
   [Column("description")]
   public string Description { get; set; }
   
   [Column("created_at")]
   public DateTime? CreatedAt { get; set; }
   
   [Column("updated_at")]
   public DateTime? UpdatedAt { get; set; }
   
   [Column("fields")]
   public List<FormField> Fields { get; set; }
   
   [Column("validations")]
   public List<FormValidationInfo> ValidationInfo { get; set; }
   
   [Column("database_info")]
   public List<FormDatabaseInfo> DatabaseInfo { get; set; }
}
