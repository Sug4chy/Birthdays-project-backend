using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class UserRepository(DbContext context) : RepositoryBase<User>(context)
{
    private IEnumerable<User> Users => Set
        .Include(user => user.Profile)
        .ToList();

    public override ValueTask<User?> FindByIdAsync(Guid id)
        => FindByIdAsync(id.ToString());

    private ValueTask<User?> FindByIdAsync(string id)
        => ValueTask.FromResult(Users.FirstOrDefault(user => user.Id == id));

    public ValueTask<User?> GetUserByEmailAsync(string email)
        => ValueTask.FromResult(Users.FirstOrDefault(user => user.Email == email));
}