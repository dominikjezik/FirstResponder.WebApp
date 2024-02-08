using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Groups.DTOs;
using FirstResponder.ApplicationCore.Groups.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Groups.Handlers;

public class GetGroupsWithUserInfoQueryHandler : IRequestHandler<GetGroupsWithUserInfoQuery, IEnumerable<GroupWithUserInfoDTO>>
{
    private readonly IGroupsRepository _groupsRepository;

    public GetGroupsWithUserInfoQueryHandler(IGroupsRepository groupsRepository)
    {
        _groupsRepository = groupsRepository;
    }
    
    public async Task<IEnumerable<GroupWithUserInfoDTO>> Handle(GetGroupsWithUserInfoQuery request, CancellationToken cancellationToken)
    {
        bool includeNotInGroups = request.Query != "";
        int limitResultsCount = request.Query != "" ? 30 : 0;
		
        return await _groupsRepository.GetGroupsWithUserInfoAsync(request.UserId, request.Query, limitResultsCount, includeNotInGroups);
    }
}