using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class Repository<TEntity>(AppDbContext context) : IRepository<TEntity> 
    where TEntity : class
{
    private DbSet<TEntity> Set => context.Set<TEntity>();

    public IQueryable<TEntity> Select()
        => Set.AsQueryable();

    public async Task AddAsync(TEntity entity, 
        CancellationToken ct = default)
        => await Set.AddAsync(entity, ct);

    public Task Update(TEntity entity)
    {
        Set.Update(entity);
        return Task.CompletedTask;
    }

    public Task Remove(TEntity entity)
    {
        Set.Remove(entity);
        return Task.CompletedTask;
    }

    public Task CommitChangesAsync(CancellationToken ct = default)
        => context.SaveChangesAsync(ct);
}