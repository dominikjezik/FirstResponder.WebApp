using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Commands;

public class UpdateModelCommand : IRequest
{
    public Guid ModelId { get; private set; }
    
    public string Name { get; private set; }

    public UpdateModelCommand(Guid modelId, string name)
    {
        ModelId = modelId;
        Name = name;
    }
}