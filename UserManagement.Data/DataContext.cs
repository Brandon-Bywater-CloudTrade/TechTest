using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data.Entities;

namespace UserManagement.Data;

public class DataContext : DbContext, IDataContext
{
    public DataContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseInMemoryDatabase("UserManagement.Data.DataContext");

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<User>().HasData(new[]
            {
            new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", IsActive = true , DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-Random.Shared.Next(70)))},
            new User { Id = 2, Forename = "Benjamin Franklin", Surname = "Gates", Email = "bfgates@example.com", IsActive = true, DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-Random.Shared.Next(70))) },
            new User { Id = 3, Forename = "Castor", Surname = "Troy", Email = "ctroy@example.com", IsActive = false, DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-Random.Shared.Next(70)))},
            new User { Id = 4, Forename = "Memphis", Surname = "Raines", Email = "mraines@example.com", IsActive = true, DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-Random.Shared.Next(70))) },
            new User { Id = 5, Forename = "Stanley", Surname = "Goodspeed", Email = "sgodspeed@example.com", IsActive = true, DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-Random.Shared.Next(70))) },
            new User { Id = 6, Forename = "H.I.", Surname = "McDunnough", Email = "himcdunnough@example.com", IsActive = true, DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-Random.Shared.Next(70))) },
            new User { Id = 7, Forename = "Cameron", Surname = "Poe", Email = "cpoe@example.com", IsActive = false , DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-Random.Shared.Next(70)))},
            new User { Id = 8, Forename = "Edward", Surname = "Malus", Email = "emalus@example.com", IsActive = false , DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-Random.Shared.Next(70)))},
            new User { Id = 9, Forename = "Damon", Surname = "Macready", Email = "dmacready@example.com", IsActive = false , DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-Random.Shared.Next(70)))},
            new User { Id = 10, Forename = "Johnny", Surname = "Blaze", Email = "jblaze@example.com", IsActive = true , DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-Random.Shared.Next(70)))},
            new User { Id = 11, Forename = "Robin", Surname = "Feld", Email = "rfeld@example.com", IsActive = true , DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-Random.Shared.Next(70)))},
        });

        model.Entity<LoggingEntry>().HasData(new[]
        {
            new LoggingEntry { Id = 1, UserId = 1, Action = "Created", Changes = "User Created.", TimeOfChange = DateTime.Now},
            new LoggingEntry { Id = 2, UserId = 2, Action = "Created", Changes = "User Created.", TimeOfChange = DateTime.Now},
            new LoggingEntry { Id = 3, UserId = 3, Action = "Created", Changes = "User Created.", TimeOfChange = DateTime.Now},
            new LoggingEntry { Id = 4, UserId = 4, Action = "Created", Changes = "User Created.", TimeOfChange = DateTime.Now},
            new LoggingEntry { Id = 5, UserId = 5, Action = "Created", Changes = "User Created.", TimeOfChange = DateTime.Now},
            new LoggingEntry { Id = 6, UserId = 6, Action = "Created", Changes = "User Created.", TimeOfChange = DateTime.Now},
            new LoggingEntry { Id = 7, UserId = 7, Action = "Created", Changes = "User Created.", TimeOfChange = DateTime.Now},
            new LoggingEntry { Id = 8, UserId = 8, Action = "Created", Changes = "User Created.", TimeOfChange = DateTime.Now},
            new LoggingEntry { Id = 9, UserId = 9, Action = "Created", Changes = "User Created.", TimeOfChange = DateTime.Now},
            new LoggingEntry { Id = 10, UserId = 1, Action = "Created", Changes = "User Created.", TimeOfChange = DateTime.Now},
            new LoggingEntry { Id = 11, UserId = 1, Action = "Created", Changes = "User Created.", TimeOfChange = DateTime.Now},
            new LoggingEntry { Id = 12, UserId = 2, Action = "Created", Changes = "User Created.", TimeOfChange = DateTime.Now},
            new LoggingEntry { Id = 13, UserId = 3, Action = "Created", Changes = "User Created.", TimeOfChange = DateTime.Now},
            new LoggingEntry { Id = 14, UserId = 4, Action = "Created", Changes = "User Created.", TimeOfChange = DateTime.Now},
            new LoggingEntry { Id = 15, UserId = 5, Action = "Created", Changes = "User Created.", TimeOfChange = DateTime.Now},
            new LoggingEntry { Id = 16, UserId = 6, Action = "Created", Changes = "User Created.", TimeOfChange = DateTime.Now},
            new LoggingEntry { Id = 17, UserId = 7, Action = "Created", Changes = "User Created.", TimeOfChange = DateTime.Now},
        });
    }

    public DbSet<User>? Users { get; set; }
    public DbSet<LoggingEntry>? LoggingEntries { get; set; }

    public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        => base.Set<TEntity>();

    public IQueryable<TEntity> GetAllNoTracking<TEntity>() where TEntity : class
        => base.Set<TEntity>().AsNoTracking();

    public void Create<TEntity>(TEntity entity) where TEntity : class
    {
        base.Add(entity);
        SaveChanges();
    }

    public new void Update<TEntity>(TEntity entity) where TEntity : class
    {
        base.Update(entity);
        SaveChanges();
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : class
    {
        base.Remove(entity);
        SaveChanges();
    }
}
