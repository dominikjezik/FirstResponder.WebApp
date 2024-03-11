using FirstResponder.ApplicationCore.Incidents.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Queries;

public class GetAedEventsQuery : IRequest<IEnumerable<AedEventDTO>>
{
    public DateTime From { get; private set; }
    
    public DateTime To { get; private set; }
    
    public GetAedEventsQuery(DateTime from, DateTime to)
    {
        From = from;
        To = to;
    }
} 