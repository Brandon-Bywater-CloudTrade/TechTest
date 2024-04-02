namespace UserManagement.Web.Models.Users;

public class DeleteUserViewModel
{
    public DeleteUserItemViewModel User { get; set; } = new();
    public string? Error { get; set; } = default!;
}

public class DeleteUserItemViewModel : UserWithIdViewModel
{
}
