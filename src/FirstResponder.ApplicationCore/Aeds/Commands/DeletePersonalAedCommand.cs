using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Commands;

public class DeletePersonalAedCommand : IRequest
{
    public string AedId { get; private set; }
    
    public Guid UserId { get; private set; }
    
    public DeletePersonalAedCommand(string aedId, Guid userId)
    {
        AedId = aedId;
        UserId = userId;
    }
}