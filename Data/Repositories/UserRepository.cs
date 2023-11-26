using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class UserRepository(DbContext context) : RepositoryBase<User>(context)
{
    private List<User> Users => Set
        .Include(user => user.Profile)
        .ToList();

    public ValueTask<User?> GetUserByIdAsync(string id)
        => ValueTask.FromResult(Users.FirstOrDefault(user => user.Id == id));

    public ValueTask<User?> GetUserByEmailAsync(string email)
        => ValueTask.FromResult(Users.FirstOrDefault(user => user.Email == email));

    public async Task SaveUserAsync(User user)
    {
        await Set.AddAsync(user);
        await CommitChangesAsync();
    }
    
    public async Task UpdateUserAsync(User user)
    {
        Set.Update(user);
        await CommitChangesAsync();
    }

    public async ValueTask<User> DeleteUserAsync(User user)
    {
        Set.Remove(user);
        await CommitChangesAsync();
        return user;
    }
}