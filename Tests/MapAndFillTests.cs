using Application.UseCases.UserOperations.ManageUser;
using Domain.Entities;
using Xunit;
using CrossCutting.Providers;

namespace Tests;

public class MapAndFillTests
{
    private readonly IDateTimeProvider _timeProvider;

    public MapAndFillTests()
    {
        _timeProvider = new DateTimeProvider();
    }

    

    [Fact]
    public void MapAndFill_ShouldFillUserFields_WhenUpdateUserRequestModelHasValues()
    {
        // Arrange
        var source = new User
        {
            Username = "oldUsername",
            Email = "oldEmail@example.com",
            FirstName = "John",
            LastName = "Doe",
            UpdatedAt = new DateTime(2022, 1, 1)
        };

        var target = new UpdateUserRequestModel
        {
            Username = "newUsername",
            Email = "newEmail@example.com",
            FirstName = "Jane",
            LastName = "Doe",
        };

        // Act
        var result = source.MapAndFill(target, _timeProvider);

        // Assert
        Assert.Equal(target.Username, result.Username);
        Assert.Equal(target.Email, result.Email);
        Assert.Equal(target.FirstName, result.FirstName);
        Assert.Equal(target.LastName, result.LastName);
    }

    

    [Fact]
    public void MapAndFill_ShouldNotFillUserFields_WhenUpdateUserRequestModelIsEmpty()
    {
        // Arrange
        var source = new User
        {
            Username = "oldUsername",
            Email = "oldEmail@example.com",
            FirstName = "John",
            LastName = "Doe"
        };

        var target = new UpdateUserRequestModel();

        // Act
        var result = source.MapAndFill(target, _timeProvider);

        // Assert
        Assert.Equal(source.Username, result.Username);
        Assert.Equal(source.Email, result.Email);
        Assert.Equal(source.FirstName, result.FirstName);
        Assert.Equal(source.LastName, result.LastName);
    }

    
}