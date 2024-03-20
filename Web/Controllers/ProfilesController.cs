using Domain.DTO.Requests.Profiles;
using Domain.DTO.Responses.Profiles;
using Domain.Handlers.Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class ProfilesController : ControllerBase
{
    [HttpGet("{userId:guid}")]
    public Task<GetProfileByIdResponse> GetProfileById(
        [FromRoute] Guid userId,
        [FromServices] GetProfileByIdHandler handler,
        CancellationToken ct = default)
        => handler.Handle(new GetProfileByIdRequest
            {
                UserId = userId
            }, ct);

    [HttpGet("current")]
    public Task<GetCurrentProfileResponse> GetCurrentProfile(
        [FromServices] GetCurrentProfileHandler handler,
        CancellationToken ct = default)
        => handler.Handle(ct);

    [HttpPost("{profileId:guid}/subscribe")]
    public Task SubscribeTo(
        [FromRoute] Guid profileId,
        [FromServices] SubscribeToHandler handler,
        CancellationToken ct = default)
        => handler.Handle(new SubscribeToRequest
            {
                BirthdayManId = profileId
            }, ct);

    [HttpPost("{profileId:guid}/unsubscribe")]
    public Task UnsubscribeFrom(
        [FromRoute] Guid profileId,
        [FromServices] UnsubscribeFromHandler handler,
        CancellationToken ct = default)
        => handler.Handle(new UnsubscribeFromRequest
            {
                BirthdayManId = profileId
            }, ct);

    [Authorize(Policy = "ShouldIncludeGuidInJwt", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet]
    public Task<GetProfilesByPageIndexResponse> GetProfilesByPageIndex(
        [FromQuery] GetProfilesByPageIndexRequest request,
        [FromServices] GetProfilesByPageIndexHandler handler,
        CancellationToken ct = default)
        => handler.Handle(request, ct);
}