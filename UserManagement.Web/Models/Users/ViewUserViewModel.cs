using UserManagement.Web.Models.Logs;

namespace UserManagement.Web.Models.Users;

public class ViewUserViewModel
{
    public UserViewItemViewModel User { get; set; } = new();
    
    public string? Error { get; set; } = default!;
    public List<LogEntryViewModel> LoggingEntries { get; set; } = new();
}

public class UserViewItemViewModel : UserWithIdViewModel
{
}
