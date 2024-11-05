using DynamicFormValidator.Presentation.Models.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace DynamicFormValidator.Presentation.Models.Swagger;

public class FormDtoExamplesProvider:IExamplesProvider<FormDto>
{
    public FormDto GetExamples()
    {
        return new FormDto
        {
            Id = 1,
            Fields = new Dictionary<int, string>
            {
                { 2, "Léon" },
                { 3, "The Professional" },
                { 4, "1994-04-27" }
            }
        };
    }
}