using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public abstract class RepositoryBase<T>(AppDbContext context)
    where T : class
{
    private readonly AppDbContext _context = context;
    protected DbSet<T> Set => _context.Set<T>();

    protected Task CommitChangesAsync()
        => context.SaveChangesAsync();
}