using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
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
}