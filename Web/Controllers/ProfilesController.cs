using Domain.DTO.Requests.Profiles;
using Domain.DTO.Responses;
using Domain.DTO.Responses.Profiles;
using Domain.Handlers.Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;

namespace Web.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ProfilesController : ControllerBase
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("{username}")]
    public Task<WrapperResponseDto<GetProfileByUsernameResponse>> GetProfileByUsername(
        [FromRoute] string username,
        [FromServices] GetProfileByUsernameHandler handler,
        CancellationToken ct = default)
        => handler.Handle(new GetProfileByUsernameRequest
            {
                Username = username,
                Jwt = HttpContext.GetJwtToken()
            },
            ct).WrappedWithLinks();

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("current")]
    public Task<WrapperResponseDto<GetCurrentProfileResponse>> GetCurrentProfile(
        [FromServices] GetCurrentProfileHandler handler,
        CancellationToken ct = default)
        => handler.Handle(new GetCurrentProfileRequest
            {
                Jwt = HttpContext.GetJwtToken()
            },
            ct).WrappedWithLinks();
}