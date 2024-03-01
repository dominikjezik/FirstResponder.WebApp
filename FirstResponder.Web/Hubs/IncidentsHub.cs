using Microsoft.AspNetCore.SignalR;

namespace FirstResponder.Web.Hubs;

public class IncidentsHub : Hub
{
    public async Task JoinIncidentGroup(string incidentId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, incidentId);
    }
}