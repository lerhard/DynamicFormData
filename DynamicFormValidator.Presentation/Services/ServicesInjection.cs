using DynamicFormValidator.Presentation.Services.Connection;
using DynamicFormValidator.Presentation.Services.Forms;

namespace DynamicFormValidator.Presentation.Services;

public static class ServicesInjection
{
   public static void AddServices(this IServiceCollection services)
   {
       services.AddScoped<IConnectionService, ConnectionService>();
       services.AddScoped<IFormsService, FormsService>();
   } 
}