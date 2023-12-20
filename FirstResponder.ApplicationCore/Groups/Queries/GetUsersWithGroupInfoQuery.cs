using FirstResponder.ApplicationCore.Groups.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Groups.Queries;

public class GetUsersWithGroupInfoQuery : IRequest<IEnumerable<UserWithGroupInfoDTO>>
{
	public Guid GroupId { get; set; }

	public string? Query { get; set; }

	public GetUsersWithGroupInfoQuery(Guid groupId, string? query = null)
	{
		Query = query;
		GroupId = groupId;
	}
}