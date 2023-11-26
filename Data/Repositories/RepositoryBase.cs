using Microsoft.EntityFrameworkCore;

namespace Birthdays.Data.Repositories;

public abstract class RepositoryBase<T>(DbContext context)
    where T : class
{
    protected DbSet<T> Set => context.Set<T>();

    protected Task CommitChangesAsync()
        => context.SaveChangesAsync();
}