using Azure.Messaging.ServiceBus;

namespace CartingService.BusinessLogicLayer.Services.Interfaces
{
    public interface IMessagesReceiverService
    {
        Task HandleMessageWithUpdateLineItemMessage(ProcessMessageEventArgs args);

        Task ErrorMessageHandler(ProcessErrorEventArgs args);
    }
}
