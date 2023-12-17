using Microsoft.AspNetCore.Mvc;
using Web.Handlers.Auth;

namespace Web.Controllers;

[ApiController]
[Route("/[controller]")]
public class AuthController
{
    [HttpPost("/register")]
    public Task<RegisterResponse> Register(
        [FromBody] RegisterRequest request,
        [FromServices] RegisterHandler handler)
        => handler.Handle(request);
}