using Domain.Handlers.Auth;
using Domain.Requests.Auth;
using Domain.Responses;
using Domain.Responses.Auth;
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
}