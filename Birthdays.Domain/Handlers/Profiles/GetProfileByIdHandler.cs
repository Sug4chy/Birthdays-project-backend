using AutoMapper;
using Domain.DTO;
using Domain.DTO.Requests.Profiles;
using Domain.DTO.Responses.Profiles;
using Domain.Exceptions;
using Domain.Services.Profiles;
using FluentValidation;

namespace Domain.Handlers.Profiles;

public class GetProfileByIdHandler(
    IValidator<GetProfileByIdRequest> validator, 
    IProfileService profileService,
    IMapper mapper)
{
    public async Task<GetProfileByIdResponse> Handle(GetProfileByIdRequest request, CancellationToken ct = default)
    {
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var profile = await profileService.GetProfileWithUserByIdAsync(request.ProfileId, ct);
        NotFoundException.ThrowIfNull(profile, $"Profile with id {request.ProfileId} wasn't found");

        return new GetProfileByIdResponse
        {
            Birthdate = profile!.User!.BirthDate,
            Name = profile.User!.Name,
            Surname = profile.User!.Surname,
            Patronymic = profile.User!.Patronymic ?? "",
            Profile = mapper.Map<ProfileDto>(profile)
        };
    }
}