using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace Store.Data.EF
{
    public static class ServiceCollectionExtentions
    {
        public static IServiceCollection AddEfRepositories(this IServiceCollection services,string connetctionString) {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(connetctionString);
                
                
            }, ServiceLifetime.Transient

            );
            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<IBookRepository, BookRepository>();

            return services;
        }
    }
}
