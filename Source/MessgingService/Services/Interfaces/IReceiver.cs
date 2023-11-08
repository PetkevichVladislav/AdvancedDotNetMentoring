using Azure.Messaging.ServiceBus;

namespace MessgingService.Services.Interfaces
{
    public interface IReceiver
    {
        public Task AssignProcessorAndStartProcessAsync(
           List<Func<ProcessMessageEventArgs, Task>> messageProcessors,
           List<Func<ProcessErrorEventArgs, Task>> messageErrorProcessors,
           CancellationToken cancellationToken);
    }
}
