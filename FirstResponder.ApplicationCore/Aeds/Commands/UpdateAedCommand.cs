using FirstResponder.ApplicationCore.Aeds.DTOs;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Commands;

public class UpdateAedCommand : IRequest<Aed>
{
    public AedFormDTO AedFormDto { get; private set; }

    public string[]? PhotosToDelete { get; set; }
    
    public UpdateAedCommand(AedFormDTO aedFormDto, string[]? photosToDelete = null)
    {
        AedFormDto = aedFormDto;
        PhotosToDelete = photosToDelete;
    }
}