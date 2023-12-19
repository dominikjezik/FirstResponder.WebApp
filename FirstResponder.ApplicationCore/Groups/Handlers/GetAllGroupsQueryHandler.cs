using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Groups.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Groups.Handlers;

public class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, IEnumerable<Group>>
{
	private readonly IGroupsRepository _groupsRepository;

	public GetAllGroupsQueryHandler(IGroupsRepository groupsRepository)
	{
		_groupsRepository = groupsRepository;
	}
	
	public async Task<IEnumerable<Group>> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
	{
		return await _groupsRepository.GetAllGroups();
	}
}