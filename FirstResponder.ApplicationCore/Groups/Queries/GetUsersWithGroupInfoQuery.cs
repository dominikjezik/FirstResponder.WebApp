using FirstResponder.ApplicationCore.Common.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Groups.Queries;

public class GetUsersWithGroupInfoQuery : IRequest<IEnumerable<UserWithAssociationInfoDTO>>
{
	public Guid GroupId { get; set; }

	public string Query { get; set; }

	public GetUsersWithGroupInfoQuery(Guid groupId, string query)
	{
		Query = query;
		GroupId = groupId;
	}
}