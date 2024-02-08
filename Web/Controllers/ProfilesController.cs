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
    [HttpGet("{profileId}")]
    public Task<WrapperResponseDto<GetProfileByIdResponse>> GetProfileById(
        [FromRoute] Guid profileId,
        [FromBody] GetProfileByIdRequest request,
        [FromServices] GetProfileByIdHandler handler,
        CancellationToken ct = default)
        => handler.Handle(request, 
            ct).WrappedWithLinks();
}