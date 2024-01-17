namespace Data.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> Select();
    Task AddAsync(TEntity entity, CancellationToken ct = default);
    Task Update(TEntity entity);
    Task Remove(TEntity entity);
    Task CommitChangesAsync(CancellationToken ct = default);
}