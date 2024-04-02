using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Data.Entities;
using UserManagement.Services.Interfaces;

namespace UserManagement.Services.Implementations;
public class LoggingService(IDataContext dataAccess) : ILoggingService
{
    private readonly IDataContext _dataAccess = dataAccess;

    public void LogChange(long userId, string action, string changes = "")
    {
        _dataAccess.Create(new LoggingEntry { UserId = userId, Action = action, Changes = changes });
    }

    /// <summary>
    /// not setup yet. Best to expose async dataaccess methods first.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="action"></param>
    /// <param name="changes"></param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public async Task LogChangesAsync(long userId, string action, string changes = "")
    {
        await Task.CompletedTask;
        throw new System.NotImplementedException();
    }

    public IQueryable<LoggingEntry> GetLogsForUser(long userId)
        => _dataAccess.GetAll<LoggingEntry>().Where(x => x.UserId == userId);

    public IQueryable<LoggingEntry> GetAllLogs()
        => _dataAccess.GetAll<LoggingEntry>();

    public LoggingEntry? GetLogById(long id)
        => _dataAccess.GetAll<LoggingEntry>().FirstOrDefault(x => x.Id == id);

}
