using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.ApplicationCore.Exceptions;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class CreateModelCommandHandler : IRequestHandler<CreateModelCommand>
{
    private readonly IAedModelsRepository _aedModelsRepository;

    public CreateModelCommandHandler(IAedModelsRepository aedModelsRepository)
    {
        _aedModelsRepository = aedModelsRepository;
    }
    
    public async Task Handle(CreateModelCommand request, CancellationToken cancellationToken)
    {
        var model = new Model
        {
            Name = request.Name
        };
        
        if (await _aedModelsRepository.ModelExists(model.Name))
        {
            var errors = new Dictionary<string, string>();
            errors["Name"] = "Model s týmto názvom už existuje!";
            
            throw new EntityValidationException(errors);
        }
        
        await _aedModelsRepository.AddModel(model);
    }
}