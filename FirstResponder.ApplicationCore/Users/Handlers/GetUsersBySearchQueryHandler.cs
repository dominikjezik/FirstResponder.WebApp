using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Users.DTOs;
using FirstResponder.ApplicationCore.Users.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Handlers;

public class GetUsersBySearchQueryHandler : IRequestHandler<GetUsersBySearchQuery, IEnumerable<UserSearchResultDTO>>
{
    private readonly IUsersRepository _usersRepository;

    public GetUsersBySearchQueryHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<IEnumerable<UserSearchResultDTO>> Handle(GetUsersBySearchQuery request, CancellationToken cancellationToken)
    {
        return await _usersRepository.GetUsersBySearch(request.SearchQuery, 10);
    }
}