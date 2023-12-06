using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class DatabaseFileRepository(DbContext context) : RepositoryBase<DatabaseFile>(context)
{
    public async ValueTask<DatabaseFile?> GetDatabaseFileByNameAsync(string name)
        => await Set.FirstOrDefaultAsync(f => f.Name == name);
}