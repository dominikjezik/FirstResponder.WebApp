using FirstResponder.ApplicationCore.Aeds.Queries;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class GetAedByIdQueryHandler : IRequestHandler<GetAedByIdQuery, Aed?>
{
    private readonly IAedRepository _aedRepository;
    private readonly IUsersRepository _usersRepository;

    public GetAedByIdQueryHandler(IAedRepository aedRepository, IUsersRepository usersRepository)
    {
        _aedRepository = aedRepository;
        _usersRepository = usersRepository;
    }

    public async Task<Aed?> Handle(GetAedByIdQuery request, CancellationToken cancellationToken)
    {
        if (Guid.TryParse(request.AedId, out Guid guid))
        {
            var aed = await _aedRepository.GetAedById(guid);

            if (aed == null)
            {
                return null;
            }

            if (aed is PersonalAed personalAed)
            {
                personalAed.Owner = await _usersRepository.GetUserById(personalAed.OwnerId);
            }
            else
            {
                var publicAed = (PublicAed)aed;
                publicAed.Photos = await _aedRepository.GetAedPhotos(aed.Id);
            }

            return aed;
        }

        return null;
    }
}