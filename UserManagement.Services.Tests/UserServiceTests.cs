using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Data.Entities;
using UserManagement.Data.Tests.Models;
using UserManagement.Services.Implementations;
using UserManagement.Services.Interfaces;

namespace UserManagement.Services.Tests;

public class UserServiceTests
{
    [Fact]
    public void GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeSameAs(users);
    }

    [Fact]
    public void GetById_WhenContextReturnsEntity_MustReturnCorrectUser()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers();
        var user = users.First();

        // Act
        var result = service.GetById(user.Id);

        // Assert
        result.Should().BeSameAs(user);
    }

    [Fact]
    public void GetById_WhenUserDoesNotExist_MustThrowException()
    {
        // Arrange
        var service = CreateService();
        SetupUsers(numOfEntities: 5);

        // Act
        var result = service.GetById(int.MaxValue); // ID that does not exist

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void FilterByActive_WhenContextHasActiveEntities_MustReturnActiveEntities()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers(isActive: true);
        var activeUsers = users.Where(x => x.IsActive);

        // Act
        var result = service.FilterByActive(true);

        // Assert
        result.Should().BeEquivalentTo(activeUsers);
    }

    [Fact]
    public void FilterByActive_WhenContextHasInactiveEntities_MustReturnInactiveEntities()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers(isActive: false);
        var inactiveUsers = users.Where(x => !x.IsActive);

        // Act
        var result = service.FilterByActive(false);

        // Assert
        result.Should().BeEquivalentTo(inactiveUsers);
    }

    [Fact]
    public void Update_WhenUserUpdated_LogChangeCalledOnce()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers();
        var user = users.First();

        var updatedUser = new User
        {
            Id = user.Id,
            Forename = "Updated",
            Surname = "User",
            Email = "uuser@example.com"
        };


        // Act
        service.Update(updatedUser);

        // Assert
        _loggingService.Verify(x => x.LogChange(user.Id, "Update", It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void Update_WhenUserUpdated_ContextUpdateCalledOnce()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers();
        var user = users.First();

        var updatedUser = new User
        {
            Id = user.Id,
            Forename = "Koopa",
            Surname = "Keep",
            Email = "uuser@example.com",
            DateOfBirth = new DateOnly(1990, 1, 1),
            IsActive = !user.IsActive
        };

        // Act
        service.Update(updatedUser);

        // Assert
        _dataContext.Verify(x => x.Update(updatedUser), Times.Once);
    }

    [Fact]
    public void Delete_WhenUserRemoved_LogChangeCalledOnce()
    {
        // Arrange
        var service = CreateService();
        var user = SetupUsers().First();

        // Act
        service.Delete(user);

        // Assert
        _loggingService.Verify(x => x.LogChange(user.Id, "Delete", "User Deleted."), Times.Once);
    }

    [Fact]
    public void Delete_WhenUserRemoved_ContextUpdateCalledOnce()
    {
        // Arrange
        var service = CreateService();
        var user = SetupUsers().First();

        // Act
        service.Delete(user);

        // Assert
        _dataContext.Verify(x => x.Delete(user), Times.Once);
    }

    [Fact]
    public void Create_WhenUserCreated_LogChangeCalledOnce()
    {
        // Arrange
        var service = CreateService();
        var user = SetupUsers().First();

        // Act
        service.Create(user);

        // Assert
        _loggingService.Verify(x => x.LogChange(user.Id, "Create", "User Created."), Times.Once);
    }

    [Fact]
    public void Create_WhenUserCreated_ContextUpdateCalledOnce()
    {
        // Arrange
        var service = CreateService();
        var user = new UserFaker().Generate(1).First();

        // Act
        service.Create(user);

        // Assert
        _dataContext.Verify(x => x.Create(user), Times.Once);
    }

    private IQueryable<User> SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true, DateOnly dateOfBirth = default, int numOfEntities = 0)
    {
        // Fill a default user with optional data
        var users = new List<User>
        {
            new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                IsActive = isActive,
                DateOfBirth = dateOfBirth
            }
        };

        if (numOfEntities > 0)
        {
            var fakeUsers = new UserFaker().Generate(numOfEntities);
            users.AddRange(fakeUsers);
        }

        var userQuerable = users.AsQueryable();

        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(userQuerable);

        _dataContext
            .Setup(s => s.GetAllNoTracking<User>())
            .Returns(userQuerable);

        return userQuerable;
    }

    private readonly Mock<IDataContext> _dataContext = new();
    private readonly Mock<ILoggingService> _loggingService = new();

    private UserService CreateService() => new(_dataContext.Object, _loggingService.Object);
}
