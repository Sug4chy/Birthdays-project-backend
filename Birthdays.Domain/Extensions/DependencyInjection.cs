using Domain.Accessors;
using Domain.Handlers.Auth;
using Domain.Handlers.Profiles;
using Domain.Handlers.WishLists;
using Domain.Services.Auth;
using Domain.Services.Profiles;
using Domain.Services.Subscriptions;
using Domain.Services.Telegram;
using Domain.Services.Tokens;
using Domain.Services.Users;
using Domain.Services.WishLists;
using Domain.Validators.Auth;
using Domain.Validators.Dto;
using Domain.Validators.Profiles;
using Domain.Validators.WishLists;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Extensions;

public static class DependencyInjection
{
    
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        //Auth handlers
        services.AddScoped<RegisterHandler>();
        services.AddScoped<LoginHandler>();
        services.AddScoped<LogoutHandler>();
        services.AddScoped<RefreshHandler>();

        //Profile handlers
        services.AddScoped<GetProfileByIdHandler>();
        services.AddScoped<GetCurrentProfileHandler>();
        services.AddScoped<SubscribeToHandler>();
        services.AddScoped<UnsubscribeFromHandler>();
        services.AddScoped<GetProfilesByPageIndexHandler>();

        //WishList handlers
        services.AddScoped<CreateWishListHandler>();
        services.AddScoped<GetCurrentProfileWishListsHandler>();
        services.AddScoped<GetProfileWishListsByIdHandler>();
        services.AddScoped<CreateWishHandler>();
        services.AddScoped<UpdateWishListHandler>();
        services.AddScoped<DeleteWishListHandler>();
        services.AddScoped<UpdateWishHandler>();
        services.AddScoped<DeleteWishHandler>();
        
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ISubscriptionsService, SubscriptionsService>();
        services.AddScoped<IWishListService, WishListService>();
        services.AddScoped<ITelegramService, TelegramService>();
        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        //Auth validators
        services.AddScoped<RegisterRequestValidator>();
        services.AddScoped<LoginRequestValidator>();
        services.AddScoped<RefreshRequestValidator>();

        //Profile validators
        services.AddScoped<GetProfileByIdRequestValidator>();
        services.AddScoped<SubscribeToRequestValidator>();
        services.AddScoped<UnsubscribeFromRequestValidator>();
        services.AddScoped<GetProfilesByPageIndexRequestValidator>();

        //WishList validators
        services.AddScoped<CreateWishListRequestValidator>();
        services.AddScoped<GetProfileWishListsByIdRequestValidator>();
        services.AddScoped<CreateWishRequestValidator>();
        services.AddScoped<UpdateWishListRequestValidator>();
        services.AddScoped<DeleteWishListRequestValidator>();
        services.AddScoped<UpdateWishRequestValidator>();
        services.AddScoped<DeleteWishRequestValidator>();

        //Dto validators
        services.AddScoped<WishListDtoValidator>();
        services.AddScoped<WishDtoValidator>();
        services.AddScoped<DateDtoValidator>();
        
        return services;
    }

    public static IServiceCollection AddCurrentUserAccessor(this IServiceCollection services)
        => services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
}