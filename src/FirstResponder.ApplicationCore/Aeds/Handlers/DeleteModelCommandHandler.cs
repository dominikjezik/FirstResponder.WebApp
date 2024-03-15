using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class DeleteModelCommandHandler : IRequestHandler<DeleteModelCommand>
{
    private readonly IAedModelsRepository _aedModelsRepository;

    public DeleteModelCommandHandler(IAedModelsRepository aedModelsRepository)
    {
        _aedModelsRepository = aedModelsRepository;
    }
    
    public async Task Handle(DeleteModelCommand request, CancellationToken cancellationToken)
    {
        var model = await _aedModelsRepository.GetModelById(request.ModelId);
        
        if (model == null)
        {
            throw new EntityNotFoundException();
        }
        
        await _aedModelsRepository.DeleteModel(model);
    }
}