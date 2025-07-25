using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.Core.Interfaces;
using Store.Core.Interfaces.RepositoriesInterFaces;
using Store.infrastructure.Data;
using Store.infrastructure.Repositories;

namespace Store.infrastructure
{
  public static class InfrastructureRegisteration
  {
    public static IServiceCollection InfrasturctureConfiguration(this IServiceCollection services,IConfiguration configuration)
    {
      services.AddScoped(typeof(IGenericReposeitry<>), typeof(GenericReposeitory<>));
      //applay unit of work pattern
      services.AddScoped<IUnitOfWork, UnitOfWork>();
      // applay dbContext
      services.AddDbContext<AppDbContext>(options =>
      {
        options.UseSqlServer(configuration.GetConnectionString("Store"));
      });
        return services;
    }
  }
}
