namespace FirstResponder.ApplicationCore.Groups.DTOs;

public class GroupWithUserInfoDTO
{
    public Guid GroupId { get; set; }
	
    public string Name { get; set; }
	
    public bool IsUserInGroup { get; set; }
}