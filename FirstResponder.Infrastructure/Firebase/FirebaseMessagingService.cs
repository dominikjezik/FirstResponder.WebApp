using FirebaseAdmin.Messaging;
using FirstResponder.ApplicationCore.Common.Abstractions;

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
}