using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class WishListRepository(DbContext context) : RepositoryBase<WishList>(context);