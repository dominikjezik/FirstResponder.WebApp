using MediatR;

namespace FirstResponder.ApplicationCore.Notifications.Commands;

public class CreateNotificationCommand : IRequest
{
    public string Content { get; private set; }
    
    public string SenderId { get; private set; }
    
    public CreateNotificationCommand(string content, string senderId)
    {
        Content = content;
        SenderId = senderId;
    }
}