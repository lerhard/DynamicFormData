using DynamicFormValidator.Presentation;
using DynamicFormValidator.Presentation.Models.DTOs;
using DynamicFormValidator.Presentation.Models.Validators;
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

app.MapPost("/validate", ([FromBody] FormRequest formData) =>
{
    var validator = new RequestValidatorService();
    var result = RequestValidatorService.Validate(formData);
    if (result.IsValid)
    {
        return Results.Ok();
    }

    var errors = result.Errors
        .ToDictionary(error => error.PropertyName, error => new[] { error.ErrorMessage });

    return Results.ValidationProblem(errors);
    
}).WithName("ValidateFormData").WithOpenApi();

app.Run();