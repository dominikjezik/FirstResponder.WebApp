using FirstResponder.ApplicationCore.Users.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Queries;

public class GetUserByIdQuery : IRequest<UserDTO?>
{
    public string UserId { get; private set; }
    
    public GetUserByIdQuery(string userId)
    {
        UserId = userId;
    }
}