using System.ComponentModel.DataAnnotations;
using FirstResponder.ApplicationCore.Common.Abstractions;

namespace FirstResponder.ApplicationCore.Entities.UserAggregate;

public class Group : AuditableEntity<Guid>
{
	[Required]
	public string Name { get; set; }

	public string? Description { get; set; }

	public List<GroupUser> Users { get; set; } = new();
}