using FirstResponder.ApplicationCore.Common.DTOs;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Groups.DTOs;

namespace FirstResponder.ApplicationCore.Common.Abstractions;

public interface IGroupsRepository
{
	Task<IEnumerable<Group>> GetAllGroups();
    
	Task<Group?> GetGroupById(Guid groupId);
	
	Task<bool> GroupExists(string name);
    
	Task AddGroup(Group group);
    
	Task UpdateGroup(Group group);
    
	Task DeleteGroup(Group group);
	
	Task<IEnumerable<UserWithAssociationInfoDTO>> GetUsersWithGroupInfoAsync(Guid groupId, string searchQuery, int limitResultsCount, bool includeNotInGroup = false);
	
	Task ChangeUsersInGroup(Guid groupId, IEnumerable<Guid> addUsers, IEnumerable<Guid> removeUsers);
	
	Task<IEnumerable<GroupWithUserInfoDTO>> GetGroupsWithUserInfoAsync(Guid userId, string searchQuery, int limitResultsCount, bool includeNotInGroups = false);
	
	Task ChangeGroupsForUser(Guid userId, IEnumerable<Guid> addGroups, IEnumerable<Guid> removeGroups);
}