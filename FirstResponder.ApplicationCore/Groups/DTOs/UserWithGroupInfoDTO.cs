namespace FirstResponder.ApplicationCore.Groups.DTOs;

public class UserWithGroupInfoDTO
{
	public Guid UserId { get; set; }
	
	public string FullName { get; set; }

	public string Email { get; set; }
	
	public bool IsInGroup { get; set; }
}