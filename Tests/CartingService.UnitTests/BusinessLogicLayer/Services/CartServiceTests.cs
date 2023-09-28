using CartingService.BusinessLogicLayer.Mapping;
using CartingService.BusinessLogicLayer.Services;
using CartingService.BusinessLogicLayer.Services.Interfaces;
using CartingService.DataAcessLayer.Repositories.Interfaces;
using FluentAssertions;
using Moq;

namespace CartingService.UnitTests.BusinessLogicLayer.Services
{
    public class CartServiceTests
    {
        private ICartService cartService = null!;

        private readonly Mock<ICartRepository> cartRepositoryMock = new();

        [SetUp]
        public void Setup()
        {
            cartService = new CartService(cartRepositoryMock.Object, MapperProvider.GetMapper());
        }

        [Test]
        public async Task GetLineItemsAsync_WhenItemsExistsInCartWithSuchId_ShouldReturnLineItems()
        {
            // Arrange
            var expectedResult = new List<MODELS.LineItem>
            {
                new MODELS.LineItem
                {
                    Id = 1,
                    Name = "Apple",
                    Quantity = 1,
                    Price = 1.14m,
                    Image = new()
                    {
                        Url = new Uri("http://www.images.com/apple"),
                        AlternativeText = "Apple",
                    },
                },
                new MODELS.LineItem
                {
                    Id = 1,
                    Name = "Orange",
                    Quantity = 1,
                    Price = 1.14m,
                    Image = new()
                    {
                        Url = new Uri("http://www.images.com/apple"),
                        AlternativeText = "Orange",
                    },
                },
            };
            cartRepositoryMock.Setup(repository => repository.GetLineItemsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            // Act
            var actualResult = await this.cartService.GetLineItemsAsync(cartId: 1, CancellationToken.None);

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task GetLineItemsAsync_WhenItemsDoNotExistsInCartWithSuchId_ShouldReturnLineItems()
        {
            // Arrange
            var expectedResult = new List<MODELS.LineItem>();
            cartRepositoryMock.Setup(repository => repository.GetLineItemsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            // Act
            var actualResult = await this.cartService.GetLineItemsAsync(cartId: 1, CancellationToken.None);

            // Assert
            actualResult.Should().BeEmpty();
        }

        [Test]
        public async Task GetLineItemsAsync_WhenTaskCancellationIsRequested_ShouldThrowOperationCanceledException()
        {
            // Arrange
            var cancelationTokenSource = new CancellationTokenSource();

            // Act 
            var actualResult = async () => await this.cartService.GetLineItemsAsync(cartId: 1, cancelationTokenSource.Token);
            cancelationTokenSource.Cancel();

            // Assert
            await actualResult.Should().ThrowAsync<OperationCanceledException>();
        }

        [Test]
        public async Task AddLineItemAsync_WhenLineItemIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            var actualResult = async () => await this.cartService.AddLineItemAsync(cartId: 1, lineItem: null, CancellationToken.None);

            // Assert
            await actualResult.Should().ThrowAsync<ArgumentNullException>();
        }

        [Test]
        public async Task AddLineItemAsync_WhenTaskCancellationIsRequested_ShouldThrowOperationCanceledException()
        {
            // Arrange
            var cancelationTokenSource = new CancellationTokenSource();

            // Act 
            var actualResult = async () => await this.cartService.AddLineItemAsync(cartId: 1, new DTO.LineItem(), cancelationTokenSource.Token);
            cancelationTokenSource.Cancel();

            // Assert
            await actualResult.Should().ThrowAsync<OperationCanceledException>();
        }

        [Test]
        public async Task RemoveLineItemAsync_WhenTaskCancellationIsRequested_ShouldThrowOperationCanceledException()
        {
            // Arrange
            var cancelationTokenSource = new CancellationTokenSource();

            // Act 
            var actualResult = async () => await this.cartService.RemoveLineItemAsync(cartId: 1, LineIteemId: 1, cancelationTokenSource.Token);
            cancelationTokenSource.Cancel();

            // Assert
            await actualResult.Should().ThrowAsync<OperationCanceledException>();
        }
    }
}