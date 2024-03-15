namespace FirstResponder.ApplicationCore.Common.DTOs;

public class UserWithAssociationInfoDTO
{
    public Guid UserId { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public bool IsAssociated { get; set; }
}