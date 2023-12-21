using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Groups.DTOs;
using FirstResponder.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace FirstResponder.Infrastructure.Repositories;

public class GroupsRepository : IGroupsRepository
{
	private readonly ApplicationDbContext _dbContext;

	public GroupsRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}
	
	public async Task<IEnumerable<Group>> GetAllGroups()
	{
		return await _dbContext.Groups
			.OrderByDescending(g => g.CreatedAt)
			.ToListAsync();
	}

	public async Task<Group?> GetGroupById(Guid groupId)
	{
		return await _dbContext.Groups
			.Where(g => g.Id == groupId)
			.FirstOrDefaultAsync();
	}

	public async Task<bool> GroupExists(string name)
	{
		return await _dbContext.Groups
			.Where(g => g.Name == name)
			.AnyAsync();
	}

	public async Task AddGroup(Group group)
	{
		_dbContext.Groups.Add(group);
		await _dbContext.SaveChangesAsync();
	}

	public async Task UpdateGroup(Group group)
	{
		_dbContext.Update(group);
		await _dbContext.SaveChangesAsync();
	}

	public async Task DeleteGroup(Group group)
	{
		_dbContext.Groups.Remove(group);
		await _dbContext.SaveChangesAsync();
	}

	public async Task<IEnumerable<UserWithGroupInfoDTO>> GetUsersWithGroupInfoAsync(Guid groupId, string searchQuery, int limitResultsCount = 0, bool includeNotInGroup = false)
	{
		var queryable = _dbContext.Users
			.GroupJoin(
				_dbContext.GroupUser.Where(groupUser => groupUser.GroupId == groupId),
				user => user.Id,
				groupUser => groupUser.UserId,
				(user, groupUsers) => new { User = user, GroupUsers = groupUsers }
			)
			.Where(userGroup => 
				userGroup.User.FullName.Contains(searchQuery) || 
				userGroup.User.Email.Contains(searchQuery)
			)
			.Where(userGroup => includeNotInGroup || userGroup.GroupUsers.Any())
			.OrderByDescending(groupUser => groupUser.User.CreatedAt)
			.SelectMany(
				userGroup => userGroup.GroupUsers.DefaultIfEmpty(),
				(userGroup, groupUser) => new UserWithGroupInfoDTO
				{
					UserId = userGroup.User.Id,
					FullName = userGroup.User.FullName,
					Email = userGroup.User.Email,
					IsInGroup = groupUser != null
				}
			);

		if (limitResultsCount > 0)
		{
			queryable = queryable.Take(limitResultsCount);
		}
        
		return await queryable.ToListAsync();
	}

	public async Task ChangeUsersInGroup(Guid groupId, IEnumerable<Guid> addUsers, IEnumerable<Guid> removeUsers)
	{
		// Filter out users that are already in the group
		var alreadyInGroup = await _dbContext.GroupUser
			.Where(groupUser => groupUser.GroupId == groupId)
			.Select(groupUser => groupUser.UserId)
			.ToListAsync();
		
		addUsers = addUsers.Except(alreadyInGroup);
		
		// Filter out users that are not in the group
		var notInGroup = await _dbContext.GroupUser
			.Where(groupUser => groupUser.GroupId == groupId)
			.Select(groupUser => groupUser.UserId)
			.ToListAsync();
		
		removeUsers = removeUsers.Intersect(notInGroup);
		
		var addGroupUsers = addUsers.Select(userId => new GroupUser
		{
			GroupId = groupId,
			UserId = userId
		});
		
		var removeGroupUsers = removeUsers.Select(userId => new GroupUser
		{
			GroupId = groupId,
			UserId = userId
		});
		
		_dbContext.GroupUser.AddRange(addGroupUsers);
		_dbContext.GroupUser.RemoveRange(removeGroupUsers);
		
		await _dbContext.SaveChangesAsync();
	}
}