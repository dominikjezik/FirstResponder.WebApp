using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Groups.DTOs;
using FirstResponder.ApplicationCore.Groups.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Groups.Handlers;

public class GetUsersWithGroupInfoQueryHandler : IRequestHandler<GetUsersWithGroupInfoQuery, IEnumerable<UserWithGroupInfoDTO>>
{
	private readonly IUsersRepository _usersRepository;

	public GetUsersWithGroupInfoQueryHandler(IUsersRepository usersRepository)
	{
		_usersRepository = usersRepository;
	}
	
	public async Task<IEnumerable<UserWithGroupInfoDTO>> Handle(GetUsersWithGroupInfoQuery request, CancellationToken cancellationToken)
	{
		return await _usersRepository.GetUsersWithGroupInfoAsync(request.GroupId, request.Query ?? "", request.Query != null);
	}
}