using FirstResponder.ApplicationCore.Common.Abstractions;
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
        int pageNumber = request.PageNumber;

        if (pageNumber < 0)
        {
            pageNumber = 0;
        }
        
        return await _usersRepository.GetUserItems(pageNumber, 30, request.Filters);
    }
}