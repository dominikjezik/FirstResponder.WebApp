namespace FirstResponder.ApplicationCore.Common.DTOs;

public class UsersAssociationChangeDTO
{
    public Guid EntityId { get; set; }
    
    public IEnumerable<Guid> CheckedOnUserIds { get; set; } = new List<Guid>();
    
    public IEnumerable<Guid> CheckedOffUserIds { get; set; } = new List<Guid>();
}