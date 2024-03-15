using FirstResponder.ApplicationCore.Groups.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Groups.Queries;

public class GetGroupsWithUserInfoQuery : IRequest<IEnumerable<GroupWithUserInfoDTO>>
{
    public Guid UserId { get; set; }

    public string Query { get; set; }
    
    public GetGroupsWithUserInfoQuery(Guid userId, string query)
    {
        Query = query;
        UserId = userId;
    }
}