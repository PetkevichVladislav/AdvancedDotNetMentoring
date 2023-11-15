using Azure.Messaging.ServiceBus;
using MessgingService.Services.Interfaces;

namespace MessgingService.Services
{
    public class Receiver : IReceiver, IAsyncDisposable
    {
        private readonly ServiceBusProcessor processor;

        public Receiver(ServiceBusProcessor processor)
        {
            this.processor = processor;
        }

        public async Task AssignProcessorAndStartProcessAsync(
            List<Func<ProcessMessageEventArgs, Task>> messageProcessors,
            List<Func<ProcessErrorEventArgs, Task>> messageErrorProcessors,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            messageProcessors.ForEach(messageProcessor => processor.ProcessMessageAsync += messageProcessor);
            messageErrorProcessors.ForEach(messageErrorProcessor => processor.ProcessErrorAsync += messageErrorProcessor);
            await processor.StartProcessingAsync(cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask DisposeAsync()
        {
            await processor.DisposeAsync();
        }
    }
}
