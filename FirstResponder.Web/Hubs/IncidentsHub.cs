using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FirstResponder.Web.Hubs;

[Authorize(Policy = "IsEmployee")]
public class IncidentsHub : Hub
{
    public async Task JoinIncidentGroup(string incidentId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, incidentId);
    }
}