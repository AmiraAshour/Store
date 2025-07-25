using Microsoft.Extensions.DependencyInjection;
using Store.Core.Interfaces;
using Store.Core.Interfaces.RepositoriesInterFaces;
using Store.Core.Services;

namespace Store.Core
{
  public static class CoreRegisteration
  {
    public static IServiceCollection CoreConfiguration(this IServiceCollection services)
    {
      services.AddScoped<ICategoriesService, CategoriesService>();
      return services;
    }
  }
}
