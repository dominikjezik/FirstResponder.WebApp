using FirstResponder.ApplicationCore.Entities.UserAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Queries;

public class GetUserEntityByIdQuery : IRequest<User?>
{
    public Guid UserId { get; private set; }
    
    public GetUserEntityByIdQuery(Guid userId)
    {
        UserId = userId;
    }
}