using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Solstice.Database.MySql.Injections
{
    public static class MySqlInjections
    {

        public static void AddDatabaseContext<TDbContext>(this IServiceCollection services, string config, bool migrate = false)
        where TDbContext : DbContext
        {
            services.AddDbContext<TDbContext>(options =>
            {
#if DEBUG
                options.EnableSensitiveDataLogging(true);
#endif
                options.UseMySql(config, ServerVersion.AutoDetect(config));
            }, ServiceLifetime.Transient);

            if (migrate)
            {
                services.BuildServiceProvider()!.GetService<TDbContext>()!.Database.Migrate();
            }
        }

        public static void AddDatabaseContext<TDbContext>(this IServiceCollection services, string config, ILoggerFactory? loggerFactory, bool migrate = false)
        where TDbContext : DbContext
        {
            services.AddDbContext<TDbContext>(options =>
            {
#if DEBUG
                options.EnableSensitiveDataLogging(true);
                if (loggerFactory is not null)
                {
                    options.UseLoggerFactory(loggerFactory);
                }
#endif
                options.UseMySql(config, ServerVersion.AutoDetect(config));
            }, ServiceLifetime.Transient);

            if (migrate)
            {
                services.BuildServiceProvider()!.GetService<TDbContext>()!.Database.Migrate();
            }
        }
    }
}