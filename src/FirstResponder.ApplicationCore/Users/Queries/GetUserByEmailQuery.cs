using FirstResponder.ApplicationCore.Entities.UserAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Queries;

public class GetUserByEmailQuery : IRequest<User?>
{
    public string Email { get; private set; }
    
    public GetUserByEmailQuery(string email)
    {
        Email = email;
    }
}