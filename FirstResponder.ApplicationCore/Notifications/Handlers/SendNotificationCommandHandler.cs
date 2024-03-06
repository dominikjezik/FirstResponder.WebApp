using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Notifications.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Notifications.Handlers;

public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand>
{
    private readonly IMessagingService _messagingService;
    private readonly INotificationsRepository _notificationsRepository;
    private readonly IDeviceTokensRepository _deviceTokensRepository;

    public SendNotificationCommandHandler(IMessagingService messagingService, INotificationsRepository notificationsRepository, IDeviceTokensRepository deviceTokensRepository)
    {
        _messagingService = messagingService;
        _notificationsRepository = notificationsRepository;
        _deviceTokensRepository = deviceTokensRepository;
    }
    
    public async Task Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = await _notificationsRepository.GetNotificationByIdWithUsers(request.NotificationId);
        
        if (notification == null)
        {
            throw new EntityNotFoundException("Notifikácia nebola nájdená!");
        }
        
        var recipients = notification.Recipients.Select(n => n.User).ToList();
        
        if (!recipients.Any())
        {
            throw new EntityNotFoundException("Notifikácia nemá žiadnych príjemcov!");
        }
        
        var deviceTokens = await _deviceTokensRepository.GetDeviceTokensForUsers(recipients);
        
        // Send notifications to devices and get failed tokens
        var failedTokens = await _messagingService.SendNotificationAsync(deviceTokens, "Máte novú notifikáciu", notification.Content);

        // Find users with only failed tokens and no valid tokens (user may have multiple devices and we need to notify atleast one)
        var successfullyNotifiedUsers = deviceTokens.Except(failedTokens).Select(dt => dt.UserId).Distinct();
        var notNotifiedUsers = recipients.Where(r => !successfullyNotifiedUsers.Contains(r.Id)).ToList();
        
        // Remove invalid tokens
        await _deviceTokensRepository.RemoveTokens(failedTokens);
        
        if (notNotifiedUsers.Any())
        {
            var notNotifiedUsersNames = string.Join(", ", notNotifiedUsers.Select(u => u.FullName));
            throw new NotificationNotSentException($"Notifikáciu sa nepodarilo odoslať nasledujúcim používateľom: {notNotifiedUsersNames}");
        }
    }
}