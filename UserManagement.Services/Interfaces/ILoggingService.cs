using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data.Entities;

namespace UserManagement.Services.Interfaces;
public interface ILoggingService
{
    IQueryable<LoggingEntry> GetAllLogs();
    LoggingEntry? GetLogById(long id);
    IQueryable<LoggingEntry> GetLogsForUser(long userId);
    void LogChange(long userId, string action, string changes = "");
    Task LogChangesAsync(long userId, string action, string changes = "");
}
