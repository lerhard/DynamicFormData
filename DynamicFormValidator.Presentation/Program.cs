using DynamicFormValidator.Presentation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/validate", ([FromBody]FormDataRequestV1Dto formData) =>
{
    var validator = new FormDataRequestV1Validator();
    var result = validator.Validate(formData);
    if (!result.IsValid)
    {
        var errors = new Dictionary<string, string[]>();
        foreach (var error in result.Errors)
        {
            errors.Add(error.PropertyName, new[] {error.ErrorMessage});
        }


        return Results.ValidationProblem(errors);

    }

    return Results.Ok();
}).WithName("ValidateFormData").WithOpenApi();

app.Run();
