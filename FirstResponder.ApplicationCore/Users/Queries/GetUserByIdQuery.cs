using FirstResponder.ApplicationCore.Entities.UserAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Queries;

public class GetUserByIdQuery : IRequest<User?>
{
    public string UserId { get; private set; }
    
    public GetUserByIdQuery(string userId)
    {
        UserId = userId;
    }
}