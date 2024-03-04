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
        
        var message = new Message
        {
            Topic = "incidents-request-location",
            Data = new Dictionary<string, string>
            {
                { "title", "" },
                { "body", "" },
            }
        };
        
        var response = await firebaseMessaging.SendAsync(message);
        
        Console.WriteLine("Successfully sent message: " + response);
    }

    public async Task SendNotificationAsync(List<User> users, string title, string message)
    {
        // TODO: z userov ziskat device tokeny a poslat im notifikaciu
        // TEMP: Docasne ziskania device tokenov
        var deviceTokens = new List<string> { "device-token-1", "device-token-2" };
        
        var firebaseMessaging = FirebaseMessaging.DefaultInstance;
        
        if (firebaseMessaging == null)
        {
            throw new Exception("Nastala chyba pri inicializácii FirebaseMessaging");
        }
        
        var multicastMessage = new MulticastMessage
        {
            Tokens = deviceTokens,
            Notification = new Notification()
            {
                Title = title,
                Body = message
            }
        };
        
        var response = await firebaseMessaging.SendMulticastAsync(multicastMessage);
        
        if (response.FailureCount > 0)
        {
            for (var i = 0; i < response.Responses.Count; i++)
            {
                if (!response.Responses[i].IsSuccess)
                {
                    Console.WriteLine("Failed to send message to " + deviceTokens[i]);
                }
                else
                {
                    Console.WriteLine("Successfully sent message: " + response.Responses[i]);
                }
            }
        }
        
        // TODO: odstranit neplatne device tokeny
    }

    public async Task StoreDeviceTokenAsync(User user, string deviceToken)
    {
        Console.WriteLine("Storing device token: " + deviceToken);
        // TODO: ulozit device token do tabulky device tokenov
    }
}