using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Dapper;
using DynamicFormValidator.Presentation.Models.DTOs;
using DynamicFormValidator.Presentation.Models.Entities.Forms;
using DynamicFormValidator.Presentation.Models.Handlers;
using DynamicFormValidator.Presentation.Models.Swagger;
using DynamicFormValidator.Presentation.Models.Validators;
using DynamicFormValidator.Presentation.Repositories;
using DynamicFormValidator.Presentation.Services;
using DynamicFormValidator.Presentation.Services.Connection;
using DynamicFormValidator.Presentation.Services.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.ExampleFilters_PrioritizingExplicitlyDefinedExamples();
    options.ExampleFilters();
});
builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
SqlMapper.AddTypeHandler(new JsonbTypeHandler<FormValidation>());
SqlMapper.AddTypeHandler(new JsonbTypeHandler<FormValidationInfo>());
SqlMapper.AddTypeHandler(new JsonbTypeHandler<FormDatabaseInfo>());
SqlMapper.AddTypeHandler(new JsonbTypeHandler<FormField>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/form", async ([FromQuery] int formId, IFormsService formService) =>
{

        var form = await formService.GetForm(formId);
        return form is null ? Results.NotFound() : Results.Ok(form);
}
).WithName("ValidateFormData").WithOpenApi();

app.MapPost("/form", async ([Required,FromBody] FormDto FormDto, IFormsService formService) =>
{
    var validationResult = await formService.ValidateForm(FormDto);
    if (!validationResult.IsValid)
    {
        Dictionary<string, string[]> errors = validationResult.Errors.ToDictionary(error => error.PropertyName, error => error.ErrorMessage.Split('|'));
        return Results.ValidationProblem(errors);
    }

    return Results.NoContent();
});

app.Run();