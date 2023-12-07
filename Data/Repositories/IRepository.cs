namespace Data.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<IQueryable<TEntity>> Select();
    Task CreateAndSaveAsync(TEntity entity, CancellationToken ct = default);
    Task Update(TEntity entity);
    Task Remove(TEntity entity);
    Task CommitChangesAsync(CancellationToken ct = default);
}