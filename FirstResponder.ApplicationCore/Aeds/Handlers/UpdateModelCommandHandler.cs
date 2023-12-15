using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Exceptions;
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
            throw new EntityNotFoundException();
        }

        if (model.Name == request.Name)
        {
            return;
        }
        
        if (await _aedModelsRepository.ModelExists(request.Name))
        {
            var errors = new Dictionary<string, string>();
            errors["Name"] = "Model s týmto názvom už existuje!";
            
            throw new EntityValidationException(errors);
        }
        
        model.Name = request.Name;
        
        await _aedModelsRepository.UpdateModel(model);
    }
}