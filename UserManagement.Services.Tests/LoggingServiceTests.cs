using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Data.Entities;
using UserManagement.Data.Tests.Models;
using UserManagement.Services.Implementations;

namespace UserManagement.Services.Tests;
public class LoggingServiceTests
{
    [Fact]
    public void LogChange_WhenCalled_MustCreateLogEntry()
    {
        // Arrange
        var userId = 1;
        var action = "Not_Real_action";
        var changes = "Not_Real_Changes";
        var service = CreateService();
        var logs = SetupLogs(numOfEntities: 5);

        // Setup a call to the Add method
        _dataAccess.Setup(x => x.Create(It.IsAny<LoggingEntry>()));

        // Act
        service.LogChange(userId, action, changes);

        // Assert
        // Verify that the Add method was called once
        _dataAccess.Verify(x => x.Create(It.IsAny<LoggingEntry>()), Times.Once());

    }

    [Fact]
    public void GetAllLogs_WhenCalled_MustReturnAllLogs()
    {
        // Arrange
        var service = CreateService();
        var logs = SetupLogs(numOfEntities: 5);

        // Act
        var result = service.GetAllLogs();

        // Assert
        result.Should().BeEquivalentTo(logs);
    }

    [Fact]
    public void GetLogsForUser_WhenCalledWithUserId_MustReturnLogsForThatUser()
    {
        // Arrange
        var userId = 1;
        var service = CreateService();
        var logs = SetupLogs(userId: userId, numOfEntities: 5);

        var logsForId = logs.Where(x => x.UserId == userId);

        // Act
        var result = service.GetLogsForUser(userId);

        // Assert
        result.Should().BeEquivalentTo(logsForId);
    }

    [Fact]
    public void GetLogById_WhenCalledWithId_MustReturnLogWithThatId()
    {
        var id = 1;
        var service = CreateService();
        var logs = SetupLogs(id: id, numOfEntities: 5);

        // Act
        var result = service.GetLogById(id);

        // Assert
        result.Should().NotBeNull();
        result?.Id.Should().Be(id);
    }

    [Fact]
    public void GetAllLogs_WhenCalled_MustCallGetAllOnce()
    {
        // Arrange
        var service = CreateService();
        var logs = SetupLogs(numOfEntities: 5);

        // Act
        var result = service.GetAllLogs();

        // Assert
        _dataAccess.Verify(x => x.GetAll<LoggingEntry>(), Times.Once());
    }

    [Fact]
    public void GetLogsForUser_WhenCalledWithUserId_MustCallGetAllOnce()
    {
        // Arrange
        var userId = 1;
        var service = CreateService();
        var logs = SetupLogs(userId: userId, numOfEntities: 5);

        // Act
        var result = service.GetLogsForUser(userId);

        // Assert
        _dataAccess.Verify(x => x.GetAll<LoggingEntry>(), Times.Once());
    }

    [Fact]
    public void GetLogById_WhenCalledWithId_MustCallGetAllOnce()
    {
        // Arrange
        var id = 1;
        var service = CreateService();
        var logs = SetupLogs(id: id, numOfEntities: 5);

        // Act
        var result = service.GetLogById(id);

        // Assert
        _dataAccess.Verify(x => x.GetAll<LoggingEntry>(), Times.Once());
    }

    [Fact]
    public void GetLogsForUser_WhenNoLogsForUser_MustReturnEmpty()
    {
        // Arrange
        var userId = 1;
        var service = CreateService();
        SetupLogs(userId: userId + 1, numOfEntities: 5); // Setup logs for a different user

        // Act
        var result = service.GetLogsForUser(userId);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetLogById_WhenNoLogWithId_MustReturnNull()
    {
        // Arrange
        var id = 1;
        var service = CreateService();
        SetupLogs(id: id + 1, numOfEntities: 5); // Setup logs with a different id

        // Act
        var result = service.GetLogById(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    /// TODO: write actual test when implemented
    public async Task LogChangesAsync_WhenCalled_MustThrowNotImplementedException()
    {
        // Arrange
        var userId = 1;
        var action = "Not_Real_action";
        var changes = "Not_Real_Changes";
        var service = CreateService();

        // Act
        Func<Task> act = async () => await service.LogChangesAsync(userId, action, changes);

        // Assert
        await act.Should().ThrowAsync<NotImplementedException>();
    }

    public IQueryable<LoggingEntry> SetupLogs(long id = 0, long userId = 0, string action = "",
        string changes = "", DateTime timeOfChange = default, int numOfEntities = 0)
    {
        var logs = new List<LoggingEntry>()
        {
            new LoggingEntry
            {
                Id = id,
                UserId = userId,
                Action = action,
                Changes = changes,
                TimeOfChange = timeOfChange
            }
        };

        if (numOfEntities > 0)
        {
            logs.AddRange(new LoggingEntryFaker().Generate(numOfEntities));
        }

        _dataAccess
            .Setup(x => x.GetAll<LoggingEntry>())
            .Returns(logs.AsQueryable());

        return logs.AsQueryable();
    }

    private readonly Mock<IDataContext> _dataAccess = new();
    private LoggingService CreateService() => new(_dataAccess.Object);
}
