using Xunit;
using CrossCutting;

namespace Tests
{
    public class QueryBuilderTests
    {
        [Theory]
        [InlineData("search", "search")]
        [InlineData("genre", "where genres.name ~")]
        [InlineData("keyword", "where keywords.name ~")]
        [InlineData("companie", "where involved_companies.company.name ~")]
        [InlineData("language", "where language_supports.language ~")]
        [InlineData("theme", "where themes.name ~")]
        [InlineData("releaseyear", "where first_release_date ~")]
        [InlineData("invalid", "invalid: invalid")]
        public void ToQueryParam_ShouldReturnCorrectQueryParam(string input, string expected)
        {
            // Act
            var result = input.ToQueryParam();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void BuildSearchQuery_ShouldReturnDefaultQuery_WhenNoQueryParams()
        {
            // Arrange
            var querys = new Dictionary<string, string>();

            // Act
            var result = querys.BuildSearchQuery(10);

            // Assert
            Assert.Equal("fields name, first_release_date, cover.image_id; limit 10; ", result);
        }

        [Fact]
        public void BuildSearchQuery_ShouldReturnQueryWithParams_WhenQueryParamsExist()
        {
            // Arrange
            var querys = new Dictionary<string, string>
            {
                { "genre", "RPG" },
                { "keyword", "adventure" }
            };

            // Act
            var result = querys.BuildSearchQuery(10);

            // Assert
            Assert.Equal("fields name, first_release_date, cover.image_id; limit 10; where genres.name ~ \"RPG\"; where keywords.name ~ \"adventure\"; ", result);
        }

        [Fact]
        public void BuildSearchQuery_ShouldThrowException_WhenInvalidQueryParamExists()
        {
            // Arrange
            var querys = new Dictionary<string, string>
            {
                { "invalid", "value" }
            };

            // Act & Assert
            Assert.Throws<Exception>(() => querys.BuildSearchQuery(10));
        }

        [Fact]
        public void CleanKeyword_ShouldProperlyCleanEspecificCharacters_WhenTheyExist()
        {
            //Arrange
            var maliciousString = "keyword; another command, other value \"";

            //Act
            var result = maliciousString.CleanKeyword();
            var expected = "keyword another command other value";

            //Assert
            Assert.Equal(expected, result);
        }
    }
}