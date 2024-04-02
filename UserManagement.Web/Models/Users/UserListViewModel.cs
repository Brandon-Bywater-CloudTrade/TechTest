namespace UserManagement.Web.Models.Users;

public class UserListViewModel
{
    public List<UserListItemViewModel> Items { get; set; } = new();
}

public class UserListItemViewModel : UserWithIdViewModel
{
}
