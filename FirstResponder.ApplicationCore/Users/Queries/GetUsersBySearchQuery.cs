using FirstResponder.ApplicationCore.Users.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Queries;

public class GetUsersBySearchQuery : IRequest<IEnumerable<UserSearchResultDTO>>
{
    public string SearchQuery { get; private set; }

    public GetUsersBySearchQuery(string searchQuery)
    {
        SearchQuery = searchQuery;
    }
}