using FirstResponder.ApplicationCore.Entities.UserAggregate;

namespace FirstResponder.ApplicationCore.Groups.DTOs;

public class GroupFormDTO
{
	public required string Name { get; set; }

	public string? Description { get; set; }
	
	public Guid GroupId { get; set; }
	
	public Group ToGroup(Group? group = null)
	{
		if (group == null)
		{
			group = new Group();
		}
		
		group.Name = Name;
		group.Description = Description;
		
		return group;
	}

}