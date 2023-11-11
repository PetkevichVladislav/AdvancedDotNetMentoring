using CatalogService.BusinessLogic.DTO;

namespace CatalogService.BusinessLogic.Services.Interfaces
{
    public interface IProductNotificationService
    {
        void PublishProductUpdateNotification(Product product);
    }
}
