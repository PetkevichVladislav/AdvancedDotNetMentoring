using CatalogService.BusinessLogic.Mapping;
using CatalogService.BusinessLogic.Services;
using CatalogService.BusinessLogic.Services.Interfaces;
using CatalogService.DataAccess.Repositories.Interfaces;
using FluentAssertions;
using Moq;


namespace CatalogService.BusinessLogic.UnitTests.Services
{
    public class CategoryServiceTests
    {
        private ICategoryService categoryService = null!;

        private readonly Mock<IRepository<MODELS.Category>> categoryRepositoryMock = new();
        private readonly Mock<IRepository<MODELS.Product>> productRepositoryMock = new();

        [SetUp]
        public void Setup()
        {
            categoryService = new CategoryService(categoryRepositoryMock.Object, productRepositoryMock.Object, MapperProvider.GetMapper());
        }

        [Test]
        public async Task GetByIdAsync_WhenItemsExistsWithSuchId_ShouldReturnModel()
        {
            // Arrange
            var expectedResult = new MODELS.Category
            {
                Id = 1,
                Name = "Apples",
                Image = new Uri("http://www.images.com/apple"),
                ParentCategoryId = null,
                ParentCategory = null,
            };
            categoryRepositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            // Act
            var actualResult = await this.categoryService.GetByIdAsync(categoryId: 1, CancellationToken.None);

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task GetByIdAsync_WhenItemsDoNotExistsWithSuchId_ShouldReturnNull()
        {
            // Arrange
            categoryRepositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()));

            // Act
            var actualResult = await this.categoryService.GetByIdAsync(categoryId: 1, CancellationToken.None);

            // Assert
            actualResult.Should().BeNull();
        }

        [Test]
        public async Task GetByIdAsync_WhenTaskCancellationIsRequested_ShouldThrowOperationCanceledException()
        {
            // Arrange
            var cancelationTokenSource = new CancellationTokenSource();

            // Act 
            var actualResult = async () => await this.categoryService.GetByIdAsync(categoryId: 1, cancelationTokenSource.Token);
            cancelationTokenSource.Cancel();

            // Assert
            await actualResult.Should().ThrowAsync<OperationCanceledException>();
        }
    }
}

