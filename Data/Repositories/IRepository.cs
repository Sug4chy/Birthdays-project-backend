namespace Data.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<IQueryable<TEntity>> ReadFromDbAsync();
    Task CreateAndSaveToDbAsync(TEntity entity);
    Task UpdateInDbAsync(TEntity entity);
    Task DeleteFromDbAsync(TEntity entity);
    Task CommitChangesAsync(CancellationToken ct = default);
}