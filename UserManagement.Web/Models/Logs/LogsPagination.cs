namespace UserManagement.Web.Models.Logs;

public class LogsPagination
{
    public long PageSize { get; set; }
    public long PageNumber { get; set; }
    public int TotalItems { get; set; }
}
