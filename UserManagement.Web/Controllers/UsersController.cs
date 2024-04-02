using System.Linq;
using AutoMapper;
using UserManagement.Data.Entities;
using UserManagement.Services.Interfaces;
using UserManagement.Web.Models.Logs;
using UserManagement.Web.Models.Users;

namespace UserManagement.Web.Controllers;

[Route("users")]
public class UsersController(IUserService userService, IMapper mapper, ILoggingService loggingService) : Controller
{
    private readonly IUserService _userService = userService;
    private readonly IMapper _mapper = mapper;
    private readonly ILoggingService _loggingService = loggingService;

    [HttpGet("users")]
    public ViewResult GetAllUsers()
    {
        var items = _userService.GetAll()
            .Select(_mapper.Map<User, UserListItemViewModel>);

        var model = new UserListViewModel
        {
            Items = items.ToList(),
        };

        return View("List", model);
    }

    [HttpGet("create-user")]
    public IActionResult CreateUser()
    {
        return View();
    }

    [HttpGet("user")]
    public IActionResult ViewUser(long userId)
    {
        var user = _userService.GetById(userId);

        if (user is null)
        {
            return View(new ViewUserViewModel
            {
                Error = "User does not exist."
            });
        }

        var logEntries = _loggingService.GetLogsForUser(userId);

        return View(new ViewUserViewModel
        {
            User = _mapper.Map<User, UserViewItemViewModel>(user),
            LoggingEntries = logEntries.Select(_mapper.Map<LoggingEntry, LogEntryViewModel>).ToList()
        });
    }

    [HttpGet("edit-user")]
    public IActionResult EditUser(long userId)
    {
        var user = _userService.GetById(userId);

        if (user is null)
        {
            return View(new EditUserViewModel
            {
                Error = "User does not exist."
            });
        }

        return View(new EditUserViewModel
        {
            User = _mapper.Map<User, EditUserItemViewModel>(user)
        });
    }

    [HttpGet("delete-user")]
    public IActionResult DeleteUser(long userId)
    {
        var user = _userService.GetById(userId);

        if (user is null)
        {
            return View(new DeleteUserViewModel
            {
                Error = "User does not exist."
            });
        }

        return View(new DeleteUserViewModel
        {
            User = _mapper.Map<User, DeleteUserItemViewModel>(user)
        });
    }

    [HttpGet("users-by-activity")]
    public IActionResult GetUsersByActivity(bool isActive)
    {
        var activeUsers = _userService.FilterByActive(isActive)
            .Select(_mapper.Map<User, UserListItemViewModel>)
            .ToList();

        return View("List", new UserListViewModel { Items = activeUsers });
    }

    [HttpPost("create-user")]
    public IActionResult CreateUser(CreateUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = _mapper.Map<CreateUserViewModel, User>(model);
        _userService.Create(user);

        return RedirectToAction(nameof(GetAllUsers));
    }

    [HttpPost("edit-user")]
    public IActionResult EditUser(EditUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = _mapper.Map<EditUserItemViewModel, User>(model.User);
        _userService.Update(user);

        return RedirectToAction(nameof(GetAllUsers));
    }

    [HttpPost("delete-user")]
    public IActionResult DeleteUser(EditUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = _mapper.Map<EditUserItemViewModel, User>(model.User);
        _userService.Delete(user);

        return RedirectToAction(nameof(GetAllUsers));
    }
}
