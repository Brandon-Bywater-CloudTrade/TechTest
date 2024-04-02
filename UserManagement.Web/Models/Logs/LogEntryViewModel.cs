using System;

namespace UserManagement.Web.Models.Logs;

public class LogEntryViewModel
{
    public long Id { get; set; }
    public long? UserId { get; set; }
    public string Action { get; set; } = default!;
    public string Changes { get; set; } = default!;
    public DateTime TimeOfChange { get; set; } = DateTime.UtcNow;
}
