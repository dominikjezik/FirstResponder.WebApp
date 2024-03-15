using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class DeleteAedCommandHandler : IRequestHandler<DeleteAedCommand>
{
    private readonly IAedRepository _aedRepository;
    private readonly IFileService _fileService;

    public DeleteAedCommandHandler(IAedRepository aedRepository, IFileService fileService)
    {
        _aedRepository = aedRepository;
        _fileService = fileService;
    }

    public async Task Handle(DeleteAedCommand request, CancellationToken cancellationToken)
    {
        var aed = await _aedRepository.GetAedById(request.AedId);

        if (aed == null)
        {
            throw new EntityNotFoundException();
        }
        
        if (aed is PublicAed publicAed)
        {
            var aedPhotos = await _aedRepository.GetAedPhotos(publicAed.Id);
            foreach (var aedPhoto in aedPhotos)
            {
                await _fileService.DeleteFile(aedPhoto.PhotoName);
            }
        }
        
        await _aedRepository.DeleteAed(aed);
    }
}
