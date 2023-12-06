using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProfileRepository(DbContext context) : RepositoryBase<Profile>(context);