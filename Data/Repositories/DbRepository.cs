using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class DbRepository<TEntity>(DbContext context) : IRepository<TEntity> 
    where TEntity : class
{
    private DbSet<TEntity> Set => context.Set<TEntity>();

    public Task<IQueryable<TEntity>> ReadFromDbAsync()
        => Task.FromResult(Set.AsQueryable());

    public async Task CreateAndSaveToDbAsync(TEntity entity)
        => await Set.AddAsync(entity);

    public Task UpdateInDbAsync(TEntity entity)
    {
        Set.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteFromDbAsync(TEntity entity)
    {
        Set.Remove(entity);
        return Task.CompletedTask;
    }

    public Task CommitChangesAsync(CancellationToken ct = default)
        => context.SaveChangesAsync(ct);
}