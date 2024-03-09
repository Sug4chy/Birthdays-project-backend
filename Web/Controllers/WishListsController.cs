﻿using Domain.DTO.Requests.WishLists;
using Domain.DTO.Responses;
using Domain.DTO.Responses.WishLists;
using Domain.Handlers.WishLists;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;

namespace Web.Controllers;

[ApiController]
[Route("/api/profiles")]
public class WishListsController : ControllerBase
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("current/[controller]")]
    public Task<WrapperResponseDto<CreateWishListResponse>> CreateWishList(
        [FromBody] CreateWishListRequest request,
        [FromServices] CreateWishListHandler handler,
        CancellationToken ct = default)
        => handler.Handle(request, ct).WrappedWithLinks();

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("current/[controller]")]
    public Task<WrapperResponseDto<GetProfileWishListsResponse>> GetCurrentProfileWishLists(
        [FromServices] GetCurrentProfileWishListsHandler handler,
        CancellationToken ct = default)
        => handler.Handle(ct).WrappedWithLinks();

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("{userId}/[controller]")]
    public Task<WrapperResponseDto<GetProfileWishListsResponse>> GetProfileWishListsById(
        [FromRoute] string userId,
        [FromServices] GetProfileWishListsByIdHandler handler,
        CancellationToken ct = default)
        => handler.Handle(new GetProfileWishListsByIdRequest { UserId = userId }, ct)
            .WrappedWithLinks();

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("current/[controller]/{wishListId}")]
    public Task<WrapperResponseDto<CreateWishResponse>> CreateWish(
        [FromRoute] Guid wishListId,
        [FromBody] CreateWishRequest request,
        [FromServices] CreateWishHandler handler,
        CancellationToken ct = default)
        => handler.Handle(request with { WishListId = wishListId }, ct)
            .WrappedWithLinks();

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut("current/[controller]/{wishListId}")]
    public Task<WrapperResponseDto<UpdateWishListResponse>> UpdateWishList(
        [FromRoute] Guid wishListId,
        [FromBody] UpdateWishListRequest request,
        [FromServices] UpdateWishListHandler handler,
        CancellationToken ct = default)
        => handler.Handle(request with { WishListId = wishListId }, ct)
            .WrappedWithLinks();

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpDelete("current/[controller]/{wishListId}")]
    public Task<WrapperResponseDto<DeleteWishListResponse>> DeleteWishList(
        [FromRoute] Guid wishListId,
        [FromServices] DeleteWishListHandler handler,
        CancellationToken ct = default)
        => handler.Handle(new DeleteWishListRequest { WishListId = wishListId }, ct)
            .WrappedWithLinks();
}