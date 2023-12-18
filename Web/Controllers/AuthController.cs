using Domain.Handlers.Auth;
using Domain.Requests.Auth;
using Domain.Responses.Auth;
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
}