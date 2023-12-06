using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class WishRepository(DbContext context) : RepositoryBase<Wish>(context);