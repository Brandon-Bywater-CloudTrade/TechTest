using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Data.Entities;
using UserManagement.Services.Interfaces;

namespace UserManagement.Services.Implementations;

public class UserService(IDataContext dataAccess, ILoggingService loggingService) : IUserService
{
    private readonly IDataContext _dataAccess = dataAccess;
    private readonly ILoggingService _loggingService = loggingService;

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public IEnumerable<User> FilterByActive(bool isActive)
        => _dataAccess.GetAll<User>().Where(x => x.IsActive == isActive);

    /// <summary>
    /// Returns user by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public User? GetById(long id)
        => _dataAccess.GetAll<User>().FirstOrDefault(x => x.Id == id);

    /// <summary>
    /// Return all users 
    /// </summary>
    /// <returns></returns>
    public IEnumerable<User> GetAll()
        => _dataAccess.GetAll<User>();

    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="user"></param>
    public void Create(User user)
    {
        _dataAccess.Create(user);
        _loggingService.LogChange(user.Id, "Create", "User Created.");
    }

    /// <summary>
    /// Update an existing user
    /// </summary>
    /// <param name="user"></param>
    public void Update(User user)
    {
        var oldEntity = _dataAccess.GetAllNoTracking<User>().First(x => x.Id == user.Id);
        _dataAccess.Update(user);
        _loggingService.LogChange(user.Id, "Update", GetDifferences(oldEntity, user));
    }

    /// <summary>
    /// Delete an existing user
    /// </summary>
    /// <param name="user"></param>
    public void Delete(User user)
    {
        var oldEntity = _dataAccess.GetAllNoTracking<User>().First(x => x.Id == user.Id);
        _dataAccess.Delete(user);
        _loggingService.LogChange(user.Id, "Delete", "User Deleted.");
    }

    private string GetDifferences(User oldUser, User newUser)
    {
        var differences = new List<string>();

        if (oldUser.Forename != newUser.Forename)
        {
            differences.Add($"Forename changed from '{oldUser.Forename}' to '{newUser.Forename}'");
        }

        if (oldUser.Surname != newUser.Surname)
        {
            differences.Add($"Surname changed from '{oldUser.Surname}' to '{newUser.Surname}'");
        }

        if (oldUser.Email != newUser.Email)
        {
            differences.Add($"Email changed from '{oldUser.Email}' to '{newUser.Email}'");
        }

        if (oldUser.IsActive != newUser.IsActive)
        {
            differences.Add($"IsActive changed from '{oldUser.IsActive}' to '{newUser.IsActive}'");
        }

        if (oldUser.DateOfBirth != newUser.DateOfBirth)
        {
            differences.Add($"DateOfBirth changed from '{oldUser.DateOfBirth}' to '{newUser.DateOfBirth}'");
        }

        return string.Join("\n", differences);
    }

}
