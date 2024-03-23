using Domain.Enums;
using Xunit;
using CrossCutting;
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
    }
}