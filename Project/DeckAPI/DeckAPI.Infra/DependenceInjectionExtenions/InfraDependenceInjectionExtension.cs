using DeckAPI.Infra.Db;
using DeckAPI.Infra.Repositories;
using DeckAPI.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeckAPI.Infra.DependenceInjectionExtenions
{
    public static class InfraDependenceInjectionExtension
    {
        public static IServiceCollection AddDeckApiInfra(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DatabaseConnection")));
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
