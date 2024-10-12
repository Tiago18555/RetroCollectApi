using Domain.Enums;
using Xunit;
using Domain;
using Domain.Exceptions;

namespace Tests
{
    public class StringExtensionsTests
    {
        [Fact]
        public void ToCapitalize_OwnershipStatus_ReturnsCorrectCapitalizedString()
        {
            // Arrange
            var input = "owned";
            var expected = "Owned";
            var type = typeof(OwnershipStatus);

            // Act
            var result = input.ToCapitalize(type);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToCapitalize_Condition_ReturnsCorrectCapitalizedString()
        {
            // Arrange
            var input = "likenew";
            var expected = "LikeNew";
            var type = typeof(Condition);

            // Act
            var result = input.ToCapitalize(type);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToCapitalize_InvalidType_ThrowsArgumentException()
        {
            // Arrange
            var input = "invalid";
            var type = typeof(string);

            // Act & Assert
            Assert.Throws<InvalidEnumTypeException>(() => input.ToCapitalize(type));
        }

        [Fact]
        public void ExtractMessage_ShouldReturnMessageContent()
        {
            // Arrange
            string json = "{\"SourceType\":\"create-user\",\"Message\":{\"Username\":\"user\",\"Password\":\"pwd\",\"ConfirmPassword\":\"pwd\",\"Email\":\"email\",\"FirstName\":\"Regina\",\"LastName\":\"Fabiana Helena da Paz\"}}";
            string expectedMessage = "{\"Username\":\"user\",\"Password\":\"pwd\",\"ConfirmPassword\":\"pwd\",\"Email\":\"email\",\"FirstName\":\"Regina\",\"LastName\":\"Fabiana Helena da Paz\"}";

            // Act
            string actualMessage = json.ExtractMessage();

            // Assert
            Assert.Equal(expectedMessage, actualMessage);
        }

        [Fact]
        public void ExtractSourceType_ShouldReturnSourceTypeValue()
        {
            // Arrange
            string json = "{\"SourceType\":\"create-user\",\"Message\":{\"Username\":\"user\",\"Password\":\"batatapalha\",\"ConfirmPassword\":\"batatapalha\",\"Email\":\"regina_fabiana_dapaz@brf-br.com\",\"FirstName\":\"Regina\",\"LastName\":\"Fabiana Helena da Paz\"}}";
            string expectedSourceType = "create-user";

            // Act
            string actualSourceType = json.ExtractSourceType();

            // Assert
            Assert.Equal(expectedSourceType, actualSourceType);
        }
    }
}