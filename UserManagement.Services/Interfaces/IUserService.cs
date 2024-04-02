using System.Collections.Generic;
using UserManagement.Data.Entities;

namespace UserManagement.Services.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="user"></param>
    void Create(User user);

    /// <summary>
    /// Delete an existing user
    /// </summary>
    /// <param name="user"></param>
    void Delete(User user);

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    IEnumerable<User> FilterByActive(bool isActive);

    /// <summary>
    /// Return all users
    /// </summary>
    /// <returns></returns>
    IEnumerable<User> GetAll();

    /// <summary>
    /// Return user by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    User? GetById(long id);

    /// <summary>
    /// Update an existing user
    /// </summary>
    /// <param name="user"></param>
    void Update(User user);
}
