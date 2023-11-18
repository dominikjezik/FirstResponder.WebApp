using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Exceptions;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class DeleteAedCommandHandler : IRequestHandler<DeleteAedCommand>
{
    private readonly IAedRepository _aedRepository;

    public DeleteAedCommandHandler(IAedRepository aedRepository)
    {
        _aedRepository = aedRepository;
    }

    public async Task Handle(DeleteAedCommand request, CancellationToken cancellationToken)
    {
        var isValid = Guid.TryParse(request.AedId, out Guid aedId);
        
        if (!isValid)
        {
            throw new ArgumentException("Invalid aedId Guid format.");
        }

        var aed = await _aedRepository.GetAedById(aedId);

        if (aed == null)
        {
            throw new EntityNotFoundException();
        }

        await _aedRepository.DeleteAed(aed);
    }
}
