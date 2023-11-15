using AutoMapper;
using CatalogService.BusinessLogic.DTO;
using CatalogService.BusinessLogic.Services.Interfaces;
using MessgingService.Models.Messages;
using MessgingService.Services.Interfaces;

namespace CatalogService.BusinessLogic.Services
{
    public class ProductNotificationService : IProductNotificationService
    {
        private readonly IPublisher publisher;
        private readonly IMapper mapper;

        public ProductNotificationService(IPublisher publisher, IMapper mapper)
        {
            this.publisher = publisher;
            this.mapper = mapper;
        }

        public async Task PublishProductUpdateNotification(Product product)
        {
            var message = mapper.Map<UpdateProductMessage>(product);
            await publisher.PublishAsync(message);
        }
    }
}
