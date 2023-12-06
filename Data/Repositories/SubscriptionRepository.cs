using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class SubscriptionRepository(DbContext context) : RepositoryBase<SubscriptionRepository>(context);
