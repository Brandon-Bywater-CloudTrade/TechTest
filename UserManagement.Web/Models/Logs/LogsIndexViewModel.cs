namespace UserManagement.Web.Models.Logs;

public class LogsIndexViewModel
{
    public List<LogEntryViewModel> Logs { get; set; } = new();
    public LogsPagination Pagination { get; set; } = new();
    public string? Error { get; set; }
}
