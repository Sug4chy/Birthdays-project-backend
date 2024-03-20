using Domain.DTO.Requests.Profiles;
using Domain.DTO.Responses;
using Domain.DTO.Responses.Profiles;
using Domain.Handlers.Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;

namespace Web.Controllers;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class ProfilesController : ControllerBase
{
    [HttpGet("{userId:guid}")]
    public Task<WrapperResponseDto<GetProfileByIdResponse>> GetProfileById(
        [FromRoute] Guid userId,
        [FromServices] GetProfileByIdHandler handler,
        CancellationToken ct = default)
        => handler.Handle(new GetProfileByIdRequest
            {
                UserId = userId
            },
            ct).WrappedWithLinks();

    [HttpGet("current")]
    public Task<WrapperResponseDto<GetCurrentProfileResponse>> GetCurrentProfile(
        [FromServices] GetCurrentProfileHandler handler,
        CancellationToken ct = default)
        => handler.Handle(ct).WrappedWithLinks();

    [HttpPost("{profileId:guid}/subscribe")]
    public Task<WrapperResponseDto<SubscribeToResponse>> SubscribeTo(
        [FromRoute] Guid profileId,
        [FromServices] SubscribeToHandler handler,
        CancellationToken ct = default)
        => handler.Handle(new SubscribeToRequest
            {
                BirthdayManId = profileId
            },
            ct).WrappedWithLinks();

    [HttpPost("{profileId:guid}/unsubscribe")]
    public Task<WrapperResponseDto<UnsubscribeFromResponse>> UnsubscribeFrom(
        [FromRoute] Guid profileId,
        [FromServices] UnsubscribeFromHandler handler,
        CancellationToken ct = default)
        => handler.Handle(new UnsubscribeFromRequest
            {
                BirthdayManId = profileId
            },
            ct).WrappedWithLinks();

    [Authorize(Policy = "ShouldIncludeGuidInJwt", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet]
    public Task<WrapperResponseDto<GetProfilesByPageIndexResponse>> GetProfilesByPageIndex(
        [FromQuery] GetProfilesByPageIndexRequest request,
        [FromServices] GetProfilesByPageIndexHandler handler,
        CancellationToken ct = default)
        => handler.Handle(request, ct)
            .WrappedWithLinks();
}