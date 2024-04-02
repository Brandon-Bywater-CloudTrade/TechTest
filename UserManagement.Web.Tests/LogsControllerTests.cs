using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Data.Entities;
using UserManagement.Services.Interfaces;
using UserManagement.Web.Controllers;
using UserManagement.Web.Mappings;
using UserManagement.Web.Models.Logs;

namespace UserManagement.Web.Tests;
public class LogsControllerTests
{
    [Fact]
    public void Index_WhenCalled_ReturnsViewResultWithModel()
    {
        // Arrange
        var controller = CreateController();
        var logs = SetupLogs(1);

        // Act
        var result = controller.Index();

        // Assert
        result.Should().BeOfType<ViewResult>();
        var model = ((ViewResult)result).Model;

        model.Should().NotBeNull();
        if (model is null) return; // This should never happen as the test will fail, but it doesn't satisfy warnings as errors

        model.Should().BeOfType<LogsIndexViewModel>();
        ((LogsIndexViewModel)model).Logs.Should().BeEquivalentTo(logs);
    }

    [Fact]
    public void Index_WhenCalledWithPaginationParameters_ReturnsViewResultWithCorrectPagination()
    {
        // Arrange
        var controller = CreateController();
        var logs = SetupLogs(30); // Setup 30 logs
        var pageSize = 15;
        var pageNumber = 2;

        // Act
        var result = controller.Index(pageSize, pageNumber);

        // Assert
        result.Should().BeOfType<ViewResult>();
        var model = ((ViewResult)result).Model;

        model.Should().NotBeNull();
        if (model is null) return;

        model.Should().BeOfType<LogsIndexViewModel>();

        var pagination = ((LogsIndexViewModel)model).Pagination;
        pagination.PageSize.Should().Be(pageSize);
        pagination.PageNumber.Should().Be(pageNumber);
        pagination.TotalItems.Should().Be(logs.Count()); // Total logs
    }

    [Fact]
    public void GetUserView_WhenUserDoesNotExist_ReturnsViewResultWithError()
    {
        // Arrange
        var controller = CreateController();
        _userService
            .Setup(x => x.GetById(It.IsAny<long>()))
            .Returns<User?>(null);

        // Act
        var result = controller.GetUserView(1);

        // Assert
        result.Should().BeOfType<ViewResult>();
        var model = ((ViewResult)result).Model;

        model.Should().NotBeNull();
        if (model is null) return;

        model.Should().BeOfType<LogsIndexViewModel>();
        ((LogsIndexViewModel)model).Error.Should().Be("User no longer exists. They have either been deleted or never existed.");
    }

    [Fact]
    public void GetLogView_WhenLogDoesNotExist_ReturnsViewResultWithError()
    {
        // Arrange
        var controller = CreateController();
        _loggingService
            .Setup(x => x.GetLogById(It.IsAny<long>()))
            .Returns<LoggingEntry?>(null);

        // Act
        var result = controller.GetLogView(1);

        // Assert
        result.Should().BeOfType<ViewResult>();
        var model = ((ViewResult)result).Model;

        model.Should().NotBeNull();
        if (model is null) return;

        model.Should().BeOfType<LogEntryViewItemViewModel>();
        ((LogEntryViewItemViewModel)model).Error.Should().Be("Log entry no longer exists. It has either been deleted or never existed.");
    }

    [Fact]
    public void GetUserView_WhenUserExists_RedirectsToViewUser()
    {
        // Arrange
        var controller = CreateController();
        var logs = SetupLogs(3);

        _userService
            .Setup(x => x.GetById(It.IsAny<long>()))
            .Returns(new User
            {
                Id = 1,
            });

        // Act
        var result = controller.GetUserView(1);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();
        var redirectResult = (RedirectToActionResult)result;

        redirectResult.ControllerName.Should().Be("Users");
        redirectResult.ActionName.Should().Be("ViewUser");
        redirectResult.RouteValues?["userId"].Should().Be(1);
    }

    [Fact]
    public void GetLogView_WhenLogExists_ReturnsCorrectLog()
    {
        // Arrange
        var controller = CreateController();
        var log = new LoggingEntry { Id = 1 };
        _loggingService
            .Setup(x => x.GetLogById(It.IsAny<long>()))
            .Returns(log);

        // Act
        var result = controller.GetLogView(1);

        // Assert
        result.Should().BeOfType<ViewResult>();
        var model = ((ViewResult)result).Model;

        model.Should().NotBeNull();
        if (model is null) return;

        model.Should().BeOfType<LogEntryViewItemViewModel>();
        ((LogEntryViewItemViewModel)model).Id.Should().Be(log.Id);
    }

    [Fact]
    public void Index_WhenCalledWithNegativePageNumber_ReturnsError()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.Index(-1);

        // Assert
        result.Should().BeOfType<ViewResult>();
        var model = ((ViewResult)result).Model;

        model.Should().NotBeNull();
        if (model is null) return;

        model.Should().BeOfType<LogsIndexViewModel>();
        ((LogsIndexViewModel)model).Error.Should().Be("Invalid page number.");
    }

    [Fact]
    public void Index_WhenNoLogsExist_ReturnsEmptyList()
    {
        // Arrange
        var controller = CreateController();
        _loggingService
            .Setup(s => s.GetAllLogs())
            .Returns(Enumerable.Empty<LoggingEntry>().AsQueryable());

        // Act
        var result = controller.Index();

        // Assert
        result.Should().BeOfType<ViewResult>();
        var model = ((ViewResult)result).Model;

        model.Should().NotBeNull();
        if (model is null) return;

        model.Should().BeOfType<LogsIndexViewModel>();
        ((LogsIndexViewModel)model).Logs.Should().BeEmpty();
    }

    private IQueryable<LoggingEntry> SetupLogs(int count, string action = "Test action", string changes = "Test changes")
    {
        IEnumerable<LoggingEntry> logs = Enumerable.Range(1, count).Select(i => new LoggingEntry
        {
            Id = i,
            Action = action,
            Changes = changes
        }).ToList();

        _loggingService
            .Setup(s => s.GetAllLogs())
            .Returns(logs.AsQueryable());

        return logs.AsQueryable();
    }

    private readonly Mock<ILoggingService> _loggingService = new();
    private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfiles())));
    private readonly Mock<IUserService> _userService = new();

    private LogsController CreateController() => new(_loggingService.Object, _mapper, _userService.Object);
}
