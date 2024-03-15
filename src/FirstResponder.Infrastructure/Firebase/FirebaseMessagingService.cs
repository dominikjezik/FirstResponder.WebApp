using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using Notification = FirebaseAdmin.Messaging.Notification;

namespace FirstResponder.Infrastructure.Firebase;

public class FirebaseMessagingService : IMessagingService
{
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
        
        Console.WriteLine("Successfully sent message: " + response);
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
        
        // TODO: multicast ma obmedzenie na 500 zariadeni, treba to rozdelit na viac multicastov
        
        var firebaseMessaging = FirebaseMessaging.DefaultInstance;
        
        if (firebaseMessaging == null)
        {
            throw new Exception("Nastala chyba pri inicializácii FirebaseMessaging");
        }
        
        // When using Notification instead of Data, the message is displayed to the user
        var multicastMessage = new MulticastMessage
        {
            Tokens = deviceTokens.Select(dt => dt.Token).ToList(),
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
        
        var failedTokens = new List<DeviceToken>();
        
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
                        failedTokens.Add(deviceTokens[i]);
                    }
                    else
                    {
                        // TODO: Logovat neznamu chybu
                    }
                }
            }
        }
        
        return failedTokens;
    }
}