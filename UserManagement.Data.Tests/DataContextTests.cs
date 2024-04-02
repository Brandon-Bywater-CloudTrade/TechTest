using System;
using System.Linq;
using UserManagement.Data.Entities;

namespace UserManagement.Data.Tests;

public class DataContextTests
{
    [Fact]
    public void GetAll_WhenNewEntityAdded_MustIncludeNewEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();

        var entity = new User
        {
            Forename = "Brand New",
            Surname = "User",
            Email = "brandnewuser@example.com"
        };
        context.Create(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = context.GetAll<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result
            .Should().Contain(s => s.Email == entity.Email)
            .Which.Should().BeEquivalentTo(entity);

        context.Dispose();
    }

    [Fact]
    public void GetAll_WhenDeleted_MustNotIncludeDeletedEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();
        var entity = context.GetAll<User>().Last();
        context.Delete(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = context.GetAll<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotContain(s => s.Email == entity.Email);

        context.Dispose();
    }

    [Fact]
    public void GetAll_WhenUpdated_MustIncludeUpdatedEntitiy()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();
        var entity = context.GetAll<User>().First();

        entity.Forename = "Updated";
        entity.Surname = "User";

        context.Update(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = context.GetAll<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().Contain(s => s.Forename == entity.Forename && s.Surname == entity.Surname);

        context.Dispose();
    }

    [Fact]
    public void Update_WhenEntityGotFromNoTrackingAndHavingMultipleEntities_MustSaveSuccessfully()
    {
        var context = CreateContext();

        var user = new User
        {
            Id = 2,
            Forename = "Johnny",
            Surname = "User",
            Email = "juser@example.com",
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-30)),
        };

        var existingEntryNoTracking = context.GetAllNoTracking<User>().First(x => x.Id == 2);

        context.Update<User>(user);

        context.GetAll<User>()
            .First(x => x.Id == 2)
            .Should().BeEquivalentTo(user);

        context.Dispose();
    }

    [Fact]
    public void Update_WhenUpdatingEntityWhileMultipleTracker_ShouldThrowInvalidOperationsException()
    {
        var context = CreateContext();

        var user = new User
        {
            Id = 2,
            Forename = "Johnny",
            Surname = "User",
            Email = "juser@example.com",
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-30)),
        };

        var existingEntryWithTracking = context.GetAll<User>().First(x => x.Id == 2);

        Action action = () => context.Update<User>(user);

        action.Should().Throw<InvalidOperationException>();

        context.Dispose();
    }

    private DataContext CreateContext() => new();
}
