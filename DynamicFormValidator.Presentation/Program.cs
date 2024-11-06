using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Dapper;
using DynamicFormValidator.Presentation.Models.DTOs;
using DynamicFormValidator.Presentation.Models.Entities.Forms;
using DynamicFormValidator.Presentation.Models.Enums;
using DynamicFormValidator.Presentation.Models.Handlers;
using DynamicFormValidator.Presentation.Models.Interpreter;
using DynamicFormValidator.Presentation.Models.Swagger;
using DynamicFormValidator.Presentation.Models.Validators;
using DynamicFormValidator.Presentation.Repositories;
using DynamicFormValidator.Presentation.Services;
using DynamicFormValidator.Presentation.Services.Connection;
using DynamicFormValidator.Presentation.Services.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Swashbuckle.AspNetCore.Filters;
using ValidationResult = FluentValidation.Results.ValidationResult;

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
DefaultTypeMap.MatchNamesWithUnderscores = true;
SqlMapper.AddTypeHandler(new JsonbTypeHandler<FormValidation>());
SqlMapper.AddTypeHandler(new JsonbTypeHandler<FormValidationInfo>());
SqlMapper.AddTypeHandler(new JsonbTypeHandler<FormDatabaseInfo>());
SqlMapper.AddTypeHandler(new JsonbTypeHandler<FormField>());
builder.Services.AddTransient<IFormDataQueryInterpreter, FormDataQueryInterpreter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/form", async ([FromQuery] string entityId, [FromQuery] int formId, IFormsService formService) =>
        {
            var result = await formService.SelectForm(entityId, formId);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }
    ).WithName("Select form data")
    .WithDescription("Select an entity by its id and form id")
    .Produces<object>(200)
    .Produces(404)
    .Produces(500)
    .WithOpenApi();

app.MapPost("/form", async ([Required, FromBody] FormDto FormDto, IFormsService formService) =>
    {
        ValidationResult validationResult = await formService.ValidateForm(FormDto);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        await formService.InsertForm(FormDto);

        return Results.NoContent();
    })
    .WithDescription("Insert an entity by its form data.")
    .Accepts<FormDto>("application/json")
    .Produces(204)
    .Produces(404)
    .Produces(500);

app.MapDelete("/form",
        async ([Required, FromQuery] int formId, [Required, FromQuery] string entityId, IFormsService formService) =>
        {
            ValidationResult validationResult = await formService.ValidateEntityId(entityId);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            await formService.DeleteForm(formId, entityId);

            return Results.NoContent();
        })
    .WithDescription("Delete an entity by its id using a form reference.")
    .Produces(204)
    .Produces(404)
    .Produces(500);

app.MapPut("/form", async ([Required, FromBody] FormDto FormDto, IFormsService formService) =>
    {
        ValidationResult validationResult = await formService.ValidateForm(FormDto, FormRequestOperation.UPDATE);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        await formService.UpdateForm(FormDto);

        return Results.NoContent();
    })
    .WithDescription("Update an entity by its form data.")
    .Accepts<FormDto>("application/json")
    .Produces(204)
    .Produces(404)
    .Produces(500);

app.Run();