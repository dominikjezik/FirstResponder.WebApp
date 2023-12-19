using FirstResponder.ApplicationCore.Entities.UserAggregate;

namespace FirstResponder.ApplicationCore.Abstractions;

public interface IGroupsRepository
{
	Task<IEnumerable<Group>> GetAllGroups();
    
	Task<Group?> GetGroupById(Guid groupId);
	
	Task<bool> GroupExists(string name);
    
	Task AddGroup(Group group);
    
	Task UpdateGroup(Group group);
    
	Task DeleteGroup(Group group);
}