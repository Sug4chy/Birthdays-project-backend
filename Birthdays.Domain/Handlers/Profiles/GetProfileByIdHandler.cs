using Domain.DTO.Requests.Profiles;
using Domain.DTO.Responses.Profiles;
using FluentValidation;

namespace Domain.Handlers.Profiles;

public class GetProfileByIdHandler(
    IValidator<GetProfileByIdRequest> validator)
{
    public async Task<GetProfileByIdResponse> Handle(GetProfileByIdRequest request, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(request, ct);
    }
}