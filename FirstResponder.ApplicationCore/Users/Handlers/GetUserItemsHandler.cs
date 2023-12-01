using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Users.DTOs;
using FirstResponder.ApplicationCore.Users.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Handlers;

public class GetUserItemsHandler : IRequestHandler<GetUserItems, IEnumerable<UserItemDTO>>
{
    private readonly IUsersRepository _usersRepository;

    public GetUserItemsHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<IEnumerable<UserItemDTO>> Handle(GetUserItems request, CancellationToken cancellationToken)
    {
        return await _usersRepository.GetUserItems(request.PageNumber, 30, request.Filters);
    }
}