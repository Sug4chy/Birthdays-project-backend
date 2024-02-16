using Domain.DTO.Requests.WishLists;
using Domain.DTO.Responses;
using Domain.DTO.Responses.WishLists;
using Domain.Handlers.WishLists;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;

namespace Web.Controllers;

[ApiController]
[Route("/api/profiles/current/[controller]")]
public class WishListsController : ControllerBase
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost]
    public Task<WrapperResponseDto<CreateWishListResponse>> CreateWishList(
        [FromBody] CreateWishListRequest request,
        [FromServices] CreateWishListHandler handler,
        CancellationToken ct = default)
        => handler.Handle(request, ct).WrappedWithLinks();
}