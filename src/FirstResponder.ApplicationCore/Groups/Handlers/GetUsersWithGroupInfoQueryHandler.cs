using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.DTOs;
using FirstResponder.ApplicationCore.Groups.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Groups.Handlers;

public class GetUsersWithGroupInfoQueryHandler : IRequestHandler<GetUsersWithGroupInfoQuery, IEnumerable<UserWithAssociationInfoDTO>>
{
	private readonly IGroupsRepository _groupsRepository;

	public GetUsersWithGroupInfoQueryHandler(IGroupsRepository groupsRepository)
	{
		_groupsRepository = groupsRepository;
	}
	
	public async Task<IEnumerable<UserWithAssociationInfoDTO>> Handle(GetUsersWithGroupInfoQuery request, CancellationToken cancellationToken)
	{
		bool includeNotInGroup = request.Query != "";
		int limitResultsCount = request.Query != "" ? 30 : 0;
		
		return await _groupsRepository.GetUsersWithGroupInfoAsync(request.GroupId, request.Query, limitResultsCount, includeNotInGroup);
	}
}