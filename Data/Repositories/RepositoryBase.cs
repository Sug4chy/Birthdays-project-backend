using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public abstract class RepositoryBase<T>(DbContext context)
    where T : class
{
    protected DbSet<T> Set => context.Set<T>();

    public abstract ValueTask<T?> FindByIdAsync(Guid id);

    public async Task SaveAsync(T entity)
        => await Set.AddAsync(entity);

    public Task UpdateAsync(T entity)
    {
        Set.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        Set.Remove(entity);
        return Task.CompletedTask;
    }

    public Task CommitChangesAsync(CancellationToken ct = default)
        => context.SaveChangesAsync(ct);
}