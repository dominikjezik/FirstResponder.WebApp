using FirstResponder.ApplicationCore.Aeds.Queries;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.ApplicationCore.Exceptions;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class GetAllModelsQueryHandler : IRequestHandler<GetAllModelsQuery, IEnumerable<Model>>
{
    private readonly IAedModelsRepository _aedModelsRepository;
    private readonly IAedManufacturersRepository _aedManufacturersRepository;

    public GetAllModelsQueryHandler(IAedModelsRepository aedModelsRepository, IAedManufacturersRepository aedManufacturersRepository)
    {
        _aedModelsRepository = aedModelsRepository;
        _aedManufacturersRepository = aedManufacturersRepository;
    }

    public async Task<IEnumerable<Model>> Handle(GetAllModelsQuery request, CancellationToken cancellationToken)
    {
        if (request.ManufacturerId == null)
        {
            return await _aedModelsRepository.GetAllModels();
        }

        if (Guid.TryParse(request.ManufacturerId, out var manufacturerId) && await _aedManufacturersRepository.GetManufacturerById(manufacturerId) != null)
        {
            return await _aedModelsRepository.GetAllModelsByManufacturerId(manufacturerId);
        }
        
        throw new EntityNotFoundException("Výrobca neexistuje!");
    }
}