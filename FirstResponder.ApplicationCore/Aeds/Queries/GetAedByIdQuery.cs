using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Queries;

public class GetAedByIdQuery : IRequest<Aed?>
{
    public string AedId { get; private set; }
    
    public GetAedByIdQuery(string aedId)
    {
        AedId = aedId;
    }
}