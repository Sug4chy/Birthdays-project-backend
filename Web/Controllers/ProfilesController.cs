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
    [HttpGet("{profileId:guid}")]
    public Task<WrapperResponseDto<GetProfileByIdResponse>> GetProfileById(
        Guid profileId,
        GetProfileByIdRequest request,
        GetProfileByIdHandler handler,
        CancellationToken ct = default)
        => handler.Handle(request with { Jwt = HttpContext.GetJwtToken() }, 
            ct).WrappedWithLinks();
}