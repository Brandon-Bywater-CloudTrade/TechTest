using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Data.Entities;
using UserManagement.Services.Interfaces;
using UserManagement.Web.Controllers;
using UserManagement.Web.Mappings;
using UserManagement.Web.Models.Users;

namespace UserManagement.Web.Tests;

public class UserControllerTests
{
    [Fact]
    public void List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = controller.GetAllUsers();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void CreateUser_WhenCalled_ReturnsViewResult()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.CreateUser();

        // Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public void ViewUser_WhenUserDoesNotExist_ReturnsViewResultWithError()
    {
        // Arrange
        var controller = CreateController();
        _userService
            .Setup(x => x.GetById(It.IsAny<long>()))
            .Returns<User?>(null);

        // Act
        var result = controller.ViewUser(1);

        // Assert
        result.Should().BeOfType<ViewResult>();
        var model = ((ViewResult)result).Model;

        model.Should().NotBeNull();
        if (model is null) return;

        model.Should().BeOfType<ViewUserViewModel>();
        ((ViewUserViewModel)model).Error.Should().Be("User does not exist.");
    }

    [Fact]
    public void EditUser_WhenUserDoesNotExist_ReturnsViewResultWithError()
    {
        // Arrange
        var controller = CreateController();
        _userService
            .Setup(x => x.GetById(It.IsAny<long>()))
            .Returns<User?>(null);

        // Act
        var result = controller.EditUser(1);

        // Assert
        result.Should().BeOfType<ViewResult>();
        var model = ((ViewResult)result).Model;

        model.Should().NotBeNull();
        if (model is null) return;

        model.Should().BeOfType<EditUserViewModel>();
        ((EditUserViewModel)model).Error.Should().Be("User does not exist.");
    }

    [Fact]
    public void DeleteUser_WhenUserDoesNotExist_ReturnsViewResultWithError()
    {
        // Arrange
        var controller = CreateController();
        _userService
            .Setup(x => x.GetById(It.IsAny<long>()))
            .Returns<User?>(null);

        // Act
        var result = controller.DeleteUser(1);

        // Assert
        result.Should().BeOfType<ViewResult>();
        var model = ((ViewResult)result).Model;

        model.Should().NotBeNull();
        if (model is null) return;

        model.Should().BeOfType<DeleteUserViewModel>();
        ((DeleteUserViewModel)model).Error.Should().Be("User does not exist.");
    }

    [Fact]
    public void GetUsersByActivity_WhenCalled_ReturnsViewResultWithUserListViewModel()
    {
        // Arrange
        var controller = CreateController();
        var users = new List<User> { new User(), new User() };
        _userService.Setup(x => x.FilterByActive(It.IsAny<bool>())).Returns(users.AsQueryable());

        // Act
        var result = controller.GetUsersByActivity(true);

        // Assert
        result.Should().BeOfType<ViewResult>();
        var model = ((ViewResult)result).Model;
        model.Should().BeOfType<UserListViewModel>();
    }

    [Fact]
    public void CreateUser_WhenModelStateIsValid_RedirectsToGetAllUsers()
    {
        // Arrange
        var controller = CreateController();
        var model = new CreateUserViewModel();

        // Act
        var result = controller.CreateUser(model);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        ((RedirectToActionResult)result).ActionName.Should().Be(nameof(controller.GetAllUsers));
    }

    [Fact]
    public void EditUser_WhenModelStateIsValid_RedirectsToGetAllUsers()
    {
        // Arrange
        var controller = CreateController();
        var model = new EditUserViewModel();

        // Act
        var result = controller.EditUser(model);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        ((RedirectToActionResult)result).ActionName.Should().Be(nameof(controller.GetAllUsers));
    }

    [Fact]
    public void DeleteUser_WhenModelStateIsValid_RedirectsToGetAllUsers()
    {
        // Arrange
        var controller = CreateController();
        var model = new EditUserViewModel();

        // Act
        var result = controller.DeleteUser(model);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        ((RedirectToActionResult)result).ActionName.Should().Be(nameof(controller.GetAllUsers));
    }

    private User[] SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true, DateOnly dateOfBirth = default)
    {

        var users = new[]
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

        _userService
            .Setup(s => s.GetAll())
            .Returns(users);

        return users;
    }

    private readonly Mock<IUserService> _userService = new();
    private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfiles())));
    private readonly Mock<ILoggingService> _loggingService = new();

    private UsersController CreateController() => new(_userService.Object, _mapper, _loggingService.Object);
}
