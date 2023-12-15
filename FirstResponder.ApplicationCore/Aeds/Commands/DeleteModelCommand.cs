using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Commands;

public class DeleteModelCommand : IRequest
{
    public Guid ModelId { get; private set; }
    
    public DeleteModelCommand(Guid modelId)
    {
        ModelId = modelId;
    }
}