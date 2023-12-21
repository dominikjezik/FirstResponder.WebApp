using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Groups.DTOs;
using FirstResponder.ApplicationCore.Groups.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Groups.Handlers;

public class GetUsersWithGroupInfoQueryHandler : IRequestHandler<GetUsersWithGroupInfoQuery, IEnumerable<UserWithGroupInfoDTO>>
{
	private readonly IGroupsRepository _groupsRepository;

	public GetUsersWithGroupInfoQueryHandler(IGroupsRepository groupsRepository)
	{
		_groupsRepository = groupsRepository;
	}
	
	public async Task<IEnumerable<UserWithGroupInfoDTO>> Handle(GetUsersWithGroupInfoQuery request, CancellationToken cancellationToken)
	{
		bool includeNotInGroup = request.Query != "";
		int limitResultsCount = request.Query != "" ? 30 : 0;
		
		return await _groupsRepository.GetUsersWithGroupInfoAsync(request.GroupId, request.Query, limitResultsCount, includeNotInGroup);
	}
}