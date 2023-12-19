using FirstResponder.ApplicationCore.Abstractions;

namespace FirstResponder.ApplicationCore.Entities.UserAggregate;

public class GroupUser : BaseEntity<Guid>
{
	public Guid GroupId { get; set; }
	public Group? Group { get; set; }
	
	public Guid UserId { get; set; }
	public User? User { get; set; }
}