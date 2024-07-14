using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

public static class DatabaseInitializer
{
    public static IHost InitializeDatabase<TContext>(this IHost host) where TContext : DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetRequiredService<TContext>();

            try
            {
                var databaseProvider = context.Database.ProviderName;

                if (databaseProvider != "Microsoft.EntityFrameworkCore.InMemory")
                {
                    logger.LogInformation("Checking if database exists...");
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        logger.LogInformation("Applying migrations...");
                        context.Database.Migrate();
                        logger.LogInformation("Migrations applied successfully.");
                    }
                    else
                    {
                        logger.LogInformation("Database is up to date.");
                    }
                }
                else
                {
                    logger.LogInformation("InMemory database detected, skipping migrations.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }

        return host;
    }
}
