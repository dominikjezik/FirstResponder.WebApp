using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class UpdateModelCommandHandler : IRequestHandler<UpdateModelCommand>
{
    private readonly IAedModelsRepository _aedModelsRepository;

    public UpdateModelCommandHandler(IAedModelsRepository aedModelsRepository)
    {
        _aedModelsRepository = aedModelsRepository;
    }

    public async Task Handle(UpdateModelCommand request, CancellationToken cancellationToken)
    {
        var model = await _aedModelsRepository.GetModelById(request.ModelId);
        
        if (model == null)
        {
            throw new EntityNotFoundException("Model neexistuje!");
        }

        if (model.Name == request.Name)
        {
            return;
        }
        
        if (await _aedModelsRepository.ModelExists(request.Name, model.ManufacturerId ?? Guid.Empty))
        {
            throw new EntityValidationException("Name", "Model s týmto názvom už existuje!");
        }
        
        model.Name = request.Name;
        
        await _aedModelsRepository.UpdateModel(model);
    }
}