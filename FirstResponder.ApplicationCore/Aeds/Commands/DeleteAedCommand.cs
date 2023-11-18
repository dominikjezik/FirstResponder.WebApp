using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Commands;

public class DeleteAedCommand : IRequest
{
    public string AedId { get; private set; }

    public DeleteAedCommand(string aedId)
    {
        AedId = aedId;
    }
}

