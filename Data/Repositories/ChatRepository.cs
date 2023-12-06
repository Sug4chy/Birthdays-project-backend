using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ChatRepository(DbContext context) : RepositoryBase<Chat>(context);