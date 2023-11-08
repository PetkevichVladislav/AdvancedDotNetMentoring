using Azure.Messaging.ServiceBus;
using MessgingService.Models.Messages;
using MessgingService.Services.Interfaces;
using Newtonsoft.Json;

namespace MessgingService.Services
{
    public class Publisher : IPublisher, IAsyncDisposable
    {
        private readonly ServiceBusSender sender;
        private readonly ServiceBusClient client;

        public Publisher(ServiceBusSender sender, ServiceBusClient client)
        {
            this.sender = sender;
            this.client = client;
        }

        public async Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : Message
        {
            cancellationToken.ThrowIfCancellationRequested();

            var messageAsJson = JsonConvert.SerializeObject(message);
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync(cancellationToken).ConfigureAwait(false);
            messageBatch.TryAddMessage(new ServiceBusMessage
            {
                Body = BinaryData.FromString(messageAsJson),
                ContentType = "application/json",
            });
            await sender.SendMessagesAsync(messageBatch, cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask DisposeAsync()
        {
            await client.DisposeAsync().ConfigureAwait(false);
            await sender.DisposeAsync().ConfigureAwait(false);
        }
    }
}