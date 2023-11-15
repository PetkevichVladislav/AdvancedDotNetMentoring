using CatalogService.BusinessLogic.DTO;

namespace CatalogService.BusinessLogic.Services.Interfaces
{
    public interface IProductNotificationService
    {
        Task PublishProductUpdateNotification(Product product);
    }
}
