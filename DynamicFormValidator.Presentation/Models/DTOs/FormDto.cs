namespace DynamicFormValidator.Presentation.Models.DTOs;

public class FormDto
{
   public int Id { get; init; } 
   public Dictionary<int, string> Fields { get; init; }
   public List<FormDto> SubForm { get; init; }
}