using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Commands;

public class DeleteAedCommand : IRequest
{
    public Guid AedId { get; private set; }

    public DeleteAedCommand(Guid aedId)
    {
        AedId = aedId;
    }
}