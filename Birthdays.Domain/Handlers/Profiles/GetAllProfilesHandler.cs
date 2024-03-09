using AutoMapper;
using Domain.Accessors;
using Domain.DTO.Responses.Profiles;
using Domain.Services.Users;

namespace Domain.Handlers.Profiles;

public class GetAllProfilesHandler(
    ICurrentUserAccessor userAccessor,
    IUserService userService,
    IMapper mapper)
{
    public async Task<GetAllProfilesResponse> Handle(CancellationToken ct = default)
    {
        var currentUser = await userAccessor.GetCurrentUserAsync(ct);

        var users = await userService.GetAllUsersAsync(ct);
        return new GetAllProfilesResponse
        {
            Profiles = users
                .Where(u => !u.Id.Equals(currentUser.Id))
                .Select(mapper.Map<MainPageProfileDto>).ToArray()
        };
    }
}