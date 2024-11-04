using DynamicFormValidator.Presentation.Repositories.Forms;

namespace DynamicFormValidator.Presentation.Repositories;

public static class RepositoriesInjection
{
   public static void AddRepositories(this IServiceCollection services)
   {
       services.AddScoped<IFormsRepository, FormsRepository>();
   } 
}