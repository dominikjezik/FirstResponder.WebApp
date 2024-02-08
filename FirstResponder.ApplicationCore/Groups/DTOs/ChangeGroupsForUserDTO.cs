namespace FirstResponder.ApplicationCore.Groups.DTOs;

public class ChangeGroupsForUserDTO
{
    public Guid UserId { get; set; }
    public IEnumerable<Guid> CheckedOnGroupIds { get; set; } = new List<Guid>();
    public IEnumerable<Guid> CheckedOffGroupIds { get; set; } = new List<Guid>();
}