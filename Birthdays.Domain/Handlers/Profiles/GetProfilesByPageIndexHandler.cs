using AutoMapper;
using Domain.Accessors;
using Domain.DTO.Requests.Profiles;
using Domain.DTO.Responses.Profiles;
using Domain.Exceptions;
using Domain.Services.Users;
using Domain.Validators.Profiles;

namespace Domain.Handlers.Profiles;

public class GetProfilesByPageIndexHandler(
    GetProfilesByPageIndexRequestValidator validator,
    ICurrentUserAccessor userAccessor,
    IUserService userService,
    IMapper mapper)
{
    public async Task<GetProfilesByPageIndexResponse> Handle(GetProfilesByPageIndexRequest request, 
        CancellationToken ct = default)
    {
        var currentUser = await userAccessor.GetCurrentUserAsync(ct);

        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var users = await userService
            .GetAllUsersWithPaginationIndexAsync(currentUser.Id, 15 * request.PageIndex, 15, ct);
        return new GetProfilesByPageIndexResponse
        {
            Profiles = users
                
                .Select(mapper.Map<MainPageProfileDto>).ToArray()
        };
    }
}