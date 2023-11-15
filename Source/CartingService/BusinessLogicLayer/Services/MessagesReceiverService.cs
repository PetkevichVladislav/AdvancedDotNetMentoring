using AutoMapper;
using Azure.Messaging.ServiceBus;
using CartingService.BusinessLogicLayer.DTO;
using CartingService.BusinessLogicLayer.Services.Interfaces;
using MessgingService.Models.Messages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CartingService.BusinessLogicLayer.Services
{
    public class MessagesReceiverService : IMessagesReceiverService
    {
        private readonly ILogger<MessagesReceiverService> logger;
        private readonly ILineItemService lineItemService;
        private readonly IMapper mapper;

        public MessagesReceiverService(ILogger<MessagesReceiverService> logger, ILineItemService lineItemService, IMapper mapper)
        {
            this.logger = logger;
            this.lineItemService = lineItemService;
            this.mapper = mapper;
        }

        public async Task HandleMessageWithUpdateLineItemMessage(ProcessMessageEventArgs args)
        {
            try
            {
                string body = args.Message.Body.ToString();
                var message = JsonConvert.DeserializeObject<UpdateProductMessage>(body);
                var lineItemInfo = this.mapper.Map<LineItemInfo>(message);
                if (message is not null)
                {
                    ThreadPool.QueueUserWorkItem(async callback => await lineItemService.UpdateLineItemByProductId(message.ProductId, lineItemInfo, args.CancellationToken));
                }

                await args.CompleteMessageAsync(args.Message);

            }
            catch (Exception exception)
            {
                logger.LogError(exception,"Exception was thrown while handle message with message id: {messageId}", args.Message.MessageId);
            }
        }

        public Task ErrorMessageHandler(ProcessErrorEventArgs args)
        {
            logger.LogError(args.Exception, "Exception was thrown while read message.");
            return Task.CompletedTask;
        }
    }
}
