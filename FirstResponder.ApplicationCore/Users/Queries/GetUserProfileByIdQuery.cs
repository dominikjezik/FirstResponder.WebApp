using FirstResponder.ApplicationCore.Users.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Queries;

public class GetUserProfileByIdQuery : IRequest<UserProfileDTO?>
{
    public string UserId { get; private set; }
    
    public GetUserProfileByIdQuery(string userId)
    {
        UserId = userId;
    }
}