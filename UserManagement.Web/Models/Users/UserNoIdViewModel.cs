using System.ComponentModel.DataAnnotations;
using System;

namespace UserManagement.Web.Models.Users;

public class UserNoIdViewModel
{
    [Required(ErrorMessage = "Forename is required")]
    public string Forename { get; set; } = default!;

    [Required(ErrorMessage = "Surname is required")]
    public string Surname { get; set; } = default!;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Active status required")]
    public bool IsActive { get; set; } = default!;

    [Required(ErrorMessage = "Date Of Birth Required")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateOnly DateOfBirth { get; set; }
}
