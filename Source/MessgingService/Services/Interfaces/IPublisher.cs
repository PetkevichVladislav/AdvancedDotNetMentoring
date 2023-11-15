using MessgingService.Models.Messages;

namespace MessgingService.Services.Interfaces
{
    public interface IPublisher
    {
        Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : Message;
    }
}
