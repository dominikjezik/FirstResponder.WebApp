namespace FirstResponder.ApplicationCore.Common.Abstractions;

public interface IMessagingService
{
    Task RequestDeviceLocationsAsync();
}