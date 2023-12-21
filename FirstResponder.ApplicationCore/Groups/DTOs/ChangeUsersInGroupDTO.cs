namespace FirstResponder.ApplicationCore.Groups.DTOs;

public class ChangeUsersInGroupDTO
{
    public Guid GroupId { get; set; }
    public IEnumerable<Guid> CheckedOnUserIds { get; set; } = new List<Guid>();
    public IEnumerable<Guid> CheckedOffUserIds { get; set; } = new List<Guid>();
}