using Data.Context;
using Extensions.Hosting.AsyncInitialization;
using Microsoft.EntityFrameworkCore;

namespace Web.Initializers;

public class MigrationAsyncInitializer(AppDbContext context) : IAsyncInitializer
{
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken);
            await context.Database.ExecuteSqlAsync(
                $"", 
                cancellationToken);
        }
    }
}