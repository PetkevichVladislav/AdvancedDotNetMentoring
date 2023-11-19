using CatalogService.BusinessLogic.Mapping;
using CatalogService.BusinessLogic.Services;
using CatalogService.BusinessLogic.Services.Interfaces;
using CatalogService.DataAccess.Repositories.Interfaces;
using FluentAssertions;
using Moq;

namespace CatalogService.BusinessLogic.UnitTests.Services
{
    public class ProductServiceTests
    {
        private ProductService productService = null!;

        private readonly Mock<IRepository<MODELS.Product>> productRepositoryMock = new();

        [SetUp]
        public void Setup()
        {
            var notificationMock = new Mock<IProductNotificationService>();
            productService = new ProductService(productRepositoryMock.Object, MapperProvider.GetMapper(), notificationMock.Object);
        }

        [Test]
        public async Task GetByIdAsync_WhenItemsExistsWithSuchId_ShouldReturnModel()
        {
            // Arrange
            var expectedResult = new MODELS.Product
            {
                Id = 1,
                Name = "Apple",
                Image = new Uri("http://www.images.com/apple"),
                Amount = 3,
                Description = "Description",
                Category = new MODELS.Category
                {
                    Id = 1,
                    Name = "Apples",
                    Image = new Uri("http://www.images.com/apple"),
                    ParentCategoryId = null,
                    ParentCategory = null,
                },
                CategoryId = 1,
                Price = 15.3m,
            };
            productRepositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            // Act
            var actualResult = await this.productService.GetByIdAsync(productId: 1, CancellationToken.None);

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task GetByIdAsync_WhenItemsDoNotExistsWithSuchId_ShouldReturnNull()
        {
            // Arrange
            productRepositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()));

            // Act
            var actualResult = await this.productService.GetByIdAsync(productId: 1, CancellationToken.None);

            // Assert
            actualResult.Should().BeNull();
        }

        [Test]
        public async Task GetByIdAsync_WhenTaskCancellationIsRequested_ShouldThrowOperationCanceledException()
        {
            // Arrange
            var cancelationTokenSource = new CancellationTokenSource();

            // Act 
            var actualResult = async () => await this.productService.GetByIdAsync(productId: 1, cancelationTokenSource.Token);
            cancelationTokenSource.Cancel();

            // Assert
            await actualResult.Should().ThrowAsync<OperationCanceledException>();
        }
    }
}
