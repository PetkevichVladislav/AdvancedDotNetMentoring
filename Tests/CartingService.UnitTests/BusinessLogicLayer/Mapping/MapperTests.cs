using CartingService.BusinessLogicLayer.Mapping;

namespace CartingService.UnitTests.BusinessLogicLayer.Mapping
{

    public class MapperTests
    {
        [Test]
        public void AutoMapper_WhenConfiguring_ShouldConfigureWithoutErrors()
        {
            // Arrange
            var mapper = MapperProvider.GetMapper();

            // Act & assert
            Assert.DoesNotThrow(() => mapper.ConfigurationProvider.AssertConfigurationIsValid());
        }
    }
}
