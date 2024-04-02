using System.Linq;

namespace UserManagement.Data;

public interface IDataContext
{

    /// <summary>
    /// Get a queryable of items
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    IQueryable<TEntity> GetAll<TEntity>() where TEntity : class;

    /// <summary>
    /// Get a queryable of items with no entity tracking
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="include"></param>
    /// <returns></returns>
    IQueryable<TEntity> GetAllNoTracking<TEntity>() where TEntity : class;

    /// <summary>
    /// Create a new item
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    void Create<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Uodate an existing item matching the ID
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    void Update<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Delete an item
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    void Delete<TEntity>(TEntity entity) where TEntity : class;
}
