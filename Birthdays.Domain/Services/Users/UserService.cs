﻿using Data.Entities;
using Domain.Requests.Auth;

namespace Domain.Services.Users;

public class UserService : IUserService
{
    public Task<User> CreateUserAsync(RegisterRequest request, 
        Profile profile, CancellationToken ct = default)
        => Task.FromResult(new User
        {
            ProfileId = profile.Id,
            Profile = profile,
            Name = request.Name,
            Surname = request.Surname,
            Patronymic = request.Patronymic,
            UserName = request.Email,
            Email = request.Email,
            BirthDate = DateOnly.FromDateTime(request.BirthDate)
        });
}