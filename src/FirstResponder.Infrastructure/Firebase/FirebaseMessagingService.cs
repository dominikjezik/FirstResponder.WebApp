using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using Microsoft.Extensions.Logging;
using Notification = FirebaseAdmin.Messaging.Notification;

namespace FirstResponder.Infrastructure.Firebase;

public class FirebaseMessagingService : IMessagingService
{
    private readonly ILogger<FirebaseMessagingService> _logger;
    
    public FirebaseMessagingService(ILogger<FirebaseMessagingService> logger)
    {
        _logger = logger;
    }
    
    public async Task RequestDeviceLocationsAsync()
    {
        var firebaseMessaging = FirebaseMessaging.DefaultInstance;
        
        if (firebaseMessaging == null)
        {
            throw new Exception("Nastala chyba pri inicializácii FirebaseMessaging");
        }
        
        // When using Data instead of Notification, the message is not displayed to the user
        var message = new Message
        {
            Topic = "incidents-request-location",
            Data = new Dictionary<string, string>
            {
                { "title", "" },
                { "body", "" },
                { "type", "request-location" }
            },
            Android = new AndroidConfig
            {
                TimeToLive = TimeSpan.FromMinutes(10),
                Priority = Priority.High
            }
        };
        
        var response = await firebaseMessaging.SendAsync(message);
        
        _logger.LogInformation($"RequestDeviceLocationsAsync: {response}");
    }

    public async Task<IList<DeviceToken>> SendNotificationAsync(IList<DeviceToken> deviceTokens, string title, string message)
    {
        if (!deviceTokens.Any())
        {
            return new List<DeviceToken>();
        }
        
        // Limit characters to 1000
        if (message.Length > 1000)
        {
            message = message.Substring(0, 997) + "...";
        }
        
        // Limit title to 100
        if (title.Length > 100)
        {
            title = title.Substring(0, 97) + "...";
        }
        
        var firebaseMessaging = FirebaseMessaging.DefaultInstance;
        
        if (firebaseMessaging == null)
        {
            throw new Exception("Nastala chyba pri inicializácii FirebaseMessaging");
        }
        
        var failedTokens = new List<DeviceToken>();
        
        // Divide tokens into batches of 500
        do
        {
            var batch = deviceTokens.Take(500).ToList();
            deviceTokens = deviceTokens.Skip(500).ToList();
            
            // When using Notification instead of Data, the message is displayed to the user
            var multicastMessage = new MulticastMessage
            {
                Tokens = batch.Select(dt => dt.Token).ToList(),
                Notification = new Notification()
                {
                    Title = title,
                    Body = message
                },
                Data = new Dictionary<string, string>
                {
                    { "type", "display-notification" }
                }
            };
        
            var response = await firebaseMessaging.SendMulticastAsync(multicastMessage);
        
            if (response.FailureCount > 0)
            {
                for (var i = 0; i < response.Responses.Count; i++)
                {
                    if (!response.Responses[i].IsSuccess)
                    {
                        // NotFound -> when the token is no longer valid
                        // InvalidArgument -> when the token is not valid or error in the payload
                        if (response.Responses[i].Exception.ErrorCode == ErrorCode.InvalidArgument || response.Responses[i].Exception.ErrorCode == ErrorCode.NotFound)
                        {
                            failedTokens.Add(batch[i]);
                        }
                        else
                        {
                            _logger.LogError(response.Responses[i].Exception, "Error sending FCM notification");
                        }
                    }
                }
            }
            
            _logger.LogInformation($"SendNotificationAsync: {response} batch count: {batch.Count}; deviceTokens count: {deviceTokens.Count}");
        } while (deviceTokens.Count > 0);
        
        return failedTokens;
    }

    public async Task<IList<DeviceToken>> NotifyNewMessageInIncidentAsync(IList<DeviceToken> deviceTokens, string incidentId, string title, string message)
    {
        var additionalData = new Dictionary<string, string>
        {
            { "incidentId", incidentId }
        };

        return await NotifyDevicesOnBackground(deviceTokens, "new-incident-message", title, message, additionalData);
    }

    public async Task<IList<DeviceToken>> NotifyIncidentUpdateAsync(IList<DeviceToken> deviceTokens, string incidentId, string title, string message)
    {
        var additionalData = new Dictionary<string, string>
        {
            { "incidentId", incidentId }
        };
        
        return await NotifyDevicesOnBackground(deviceTokens, "incident-update", title, message, additionalData);
    }

    private async Task<IList<DeviceToken>> NotifyDevicesOnBackground(IList<DeviceToken> deviceTokens, string type, string title, string message, Dictionary<string, string>? additionalData = null)
    {
        if (!deviceTokens.Any())
        {
            return new List<DeviceToken>();
        }
        
        // Limit characters to 1000
        if (message.Length > 1000)
        {
            message = message.Substring(0, 997) + "...";
        }
        
        // Limit title to 100
        if (title.Length > 100)
        {
            title = title.Substring(0, 97) + "...";
        }
        
        var firebaseMessaging = FirebaseMessaging.DefaultInstance;
        
        if (firebaseMessaging == null)
        {
            throw new Exception("Nastala chyba pri inicializácii FirebaseMessaging");
        }
        
        var failedTokens = new List<DeviceToken>();
        
        // Divide tokens into batches of 500
        do
        {
            var batch = deviceTokens.Take(500).ToList();
            deviceTokens = deviceTokens.Skip(500).ToList();
            
            var data = new Dictionary<string, string>
            {
                { "title", title },
                { "body", message },
                { "type", type }
            };
            
            if (additionalData != null)
            {
                foreach (var (key, value) in additionalData)
                {
                    data[key] = value;
                }
            }
            
            var multicastMessage = new MulticastMessage
            {
                Tokens = batch.Select(dt => dt.Token).ToList(),
                Data = data,
                Android = new AndroidConfig
                {
                    Priority = Priority.High
                }
            };
        
            var response = await firebaseMessaging.SendMulticastAsync(multicastMessage);
        
            if (response.FailureCount > 0)
            {
                for (var i = 0; i < response.Responses.Count; i++)
                {
                    if (!response.Responses[i].IsSuccess)
                    {
                        // NotFound -> when the token is no longer valid
                        // InvalidArgument -> when the token is not valid or error in the payload
                        if (response.Responses[i].Exception.ErrorCode == ErrorCode.InvalidArgument || response.Responses[i].Exception.ErrorCode == ErrorCode.NotFound)
                        {
                            failedTokens.Add(batch[i]);
                        }
                        else
                        {
                            _logger.LogError(response.Responses[i].Exception, "Error sending FCM notification");
                        }
                    }
                }
            }
            
            _logger.LogInformation($"NotifyDevicesOnBackground: {response} batch count: {batch.Count}; deviceTokens count: {deviceTokens.Count}");
        } while (deviceTokens.Count > 0);
        
        return failedTokens;
    }
}