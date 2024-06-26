﻿using Domain.DTO.Requests.WishLists;
using Domain.DTO.Responses.WishLists;
using Domain.Handlers.WishLists;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Authorize]
[ApiController]
[Route("/api/profiles")]
public class WishListsController : ControllerBase
{
    [HttpPost("current/[controller]")]
    public Task CreateWishList(
        [FromBody] CreateWishListRequest request,
        [FromServices] CreateWishListHandler handler,
        CancellationToken ct = default)
        => handler.Handle(request, ct);

    [HttpGet("current/[controller]")]
    public Task<GetProfileWishListsResponse> GetCurrentProfileWishLists(
        [FromServices] GetCurrentProfileWishListsHandler handler,
        CancellationToken ct = default)
        => handler.Handle(ct);

    [HttpGet("{userId}/[controller]")]
    public Task<GetProfileWishListsResponse> GetProfileWishListsById(
        [FromRoute] string userId,
        [FromServices] GetProfileWishListsByIdHandler handler,
        CancellationToken ct = default)
        => handler.Handle(new GetProfileWishListsByIdRequest { UserId = userId }, ct);

    [HttpPost("current/[controller]/{wishListId:guid}")]
    public Task CreateWish(
        [FromRoute] Guid wishListId,
        [FromBody] CreateWishRequest request,
        [FromServices] CreateWishHandler handler,
        CancellationToken ct = default)
        => handler.Handle(request with { WishListId = wishListId }, ct);

    [Authorize(Policy = "ShouldIncludeGuidInJwt", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut("current/[controller]/{wishListId:guid}")]
    public Task UpdateWishList(
        [FromRoute] Guid wishListId,
        [FromBody] UpdateWishListRequest request,
        [FromServices] UpdateWishListHandler handler,
        CancellationToken ct = default)
        => handler.Handle(request with { WishListId = wishListId }, ct);

    [Authorize(Policy = "ShouldIncludeGuidInJwt", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpDelete("current/[controller]/{wishListId:guid}")]
    public Task DeleteWishList(
        [FromRoute] Guid wishListId,
        [FromServices] DeleteWishListHandler handler,
        CancellationToken ct = default)
        => handler.Handle(new DeleteWishListRequest { WishListId = wishListId }, ct);

    [Authorize(Policy = "ShouldIncludeGuidInJwt", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut("current/[controller]/{wishListId:guid}/wishes/{wishId:guid}")]
    public Task UpdateWish(
        [FromRoute] Guid wishListId,
        [FromRoute] Guid wishId,
        [FromBody] UpdateWishRequest request,
        [FromServices] UpdateWishHandler handler,
        CancellationToken ct = default)
        => handler.Handle(request with { WishListId = wishListId, WishId = wishId }, ct);

    [Authorize(Policy = "ShouldIncludeGuidInJwt", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpDelete("current/[controller]/{wishListId:guid}/wishes/{wishId:guid}")]
    public Task DeleteWish(
        [FromRoute] Guid wishListId,
        [FromRoute] Guid wishId,
        [FromServices] DeleteWishHandler handler,
        CancellationToken ct = default)
        => handler.Handle(new DeleteWishRequest
            {
                WishListId = wishListId,
                WishId = wishId
            }, ct);
}