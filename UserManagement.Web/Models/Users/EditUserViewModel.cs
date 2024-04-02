namespace UserManagement.Web.Models.Users;

public class EditUserViewModel
{
    public EditUserItemViewModel User { get; set; } = new();
    public string? Error { get; set; } = default!;
}

public class  EditUserItemViewModel : UserWithIdViewModel
{
}
