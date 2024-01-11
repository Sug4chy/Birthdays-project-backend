using Domain.DTO.Requests.Auth;
using Domain.DTO.Responses;
using Domain.DTO.Responses.Auth;
using Domain.Handlers.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;

namespace Web.Controllers;

[ApiController]
[Route("/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("register")]
    public Task<WrapperResponseDto<RegisterResponse>> Register(
        [FromBody] RegisterRequest request,
        [FromServices] RegisterHandler handler,
        CancellationToken ct = default)
        => handler.Handle(request, ct).WrappedWithLinks();

    [HttpPost("login")]
    public Task<WrapperResponseDto<LoginResponse>> Login(
        [FromBody] LoginRequest request,
        [FromServices] LoginHandler handler,
        CancellationToken ct = default)
        => handler.Handle(request, ct).WrappedWithLinks();

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("logout")]
    public Task<WrapperResponseDto<LogoutResponse>> Logout(
        [FromBody] LogoutRequest request,
        [FromServices] LogoutHandler handler,
        CancellationToken ct = default)
        => handler.Handle(request, ct).WrappedWithLinks();

    [HttpPost("refresh")]
    public Task<WrapperResponseDto<RefreshResponse>> Refresh(
        [FromBody] RefreshRequest request,
        [FromServices] RefreshHandler handler,
        CancellationToken ct = default)
        => handler.Handle(request, ct).WrappedWithLinks();
}