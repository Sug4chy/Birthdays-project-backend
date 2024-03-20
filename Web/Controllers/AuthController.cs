using Domain.DTO.Requests.Auth;
using Domain.DTO.Responses.Auth;
using Domain.Handlers.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("register")]
    public Task<RegisterResponse> Register(
        [FromBody] RegisterRequest request,
        [FromServices] RegisterHandler handler,
        CancellationToken ct = default)
        => handler.Handle(request, ct);

    [HttpPost("login")]
    public Task<LoginResponse> Login(
        [FromBody] LoginRequest request,
        [FromServices] LoginHandler handler,
        CancellationToken ct = default)
        => handler.Handle(request, ct);

    [Authorize]
    [HttpPost("logout")]
    public Task Logout(
        [FromServices] LogoutHandler handler,
        CancellationToken ct = default)
        => handler.Handle(ct);

    [HttpPost("refresh")]
    public Task<RefreshResponse> Refresh(
        [FromBody] RefreshRequest request,
        [FromServices] RefreshHandler handler,
        CancellationToken ct = default)
        => handler.Handle(request, ct);
}