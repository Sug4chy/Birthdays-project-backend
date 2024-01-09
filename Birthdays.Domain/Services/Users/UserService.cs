﻿using Data.Entities;
using Data.Repositories;
using Domain.DTO.Requests.Auth;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services.Users;

public class UserService(IRepository<Profile> profileRepo) : IUserService
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

    public async Task<User?> GetUserByEmail(string email, CancellationToken ct = default)
    {
        var profile = (await profileRepo.Select())
            .Include(p => p.User)
            .Include(p => p.SubscriptionsAsSubscriber)
            .Include(p => p.SubscriptionsAsBirthdayMan)
            .FirstOrDefault(p => p.User!.Email == email);
        return profile?.User;
    }
}