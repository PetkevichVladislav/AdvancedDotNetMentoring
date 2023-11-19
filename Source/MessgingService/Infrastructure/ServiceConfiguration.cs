using Azure.Messaging.ServiceBus;
using MessgingService.Services;
using MessgingService.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessgingService.Infrastructure
{
    public static class ServiceConfiguration
    {
        public static void AddCatalogMessagesPublisher(this IServiceCollection services, ConfigurationManager configuration)
        {
            var messagingServicesSection = configuration.GetSection("MessagingServices");
            var productMessageServiceSection = messagingServicesSection.GetSection("ProductMessageService");
            var connectionString = productMessageServiceSection.GetSection("ConnectionString").Value;
            var queueOrTopicName = productMessageServiceSection.GetSection("QueueOrTopicName").Value;
            var client = new ServiceBusClient(connectionString, new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            });
            var sender = client.CreateSender(queueOrTopicName);
            services.AddSingleton<IPublisher>(new Publisher(sender));
        }

        public static void AddCatalogMessagesReceiver(
            this IServiceCollection services,
            ConfigurationManager configuration,
            List<Func<ProcessMessageEventArgs, Task>> messageProcessors,
            List<Func<ProcessErrorEventArgs, Task>> messageErrorProcessors)
        {
            var messagingServicesSection = configuration.GetSection("MessagingServices");
            var productMessageServiceSection = messagingServicesSection.GetSection("ProductMessageService");
            var connectionString = productMessageServiceSection.GetSection("ConnectionString").Value;
            var queueOrTopicName = productMessageServiceSection.GetSection("QueueOrTopicName").Value;

            var client = new ServiceBusClient(connectionString, new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            });
            var processor = client.CreateProcessor(queueOrTopicName, new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
            });
            var receiver = new Receiver(processor);
            services.AddSingleton<IReceiver>(receiver);
            receiver.AssignProcessorAndStartProcessAsync(messageProcessors, messageErrorProcessors).GetAwaiter().GetResult();
        }
    }
}
