using System.Linq;
using AutoMapper;
using UserManagement.Data.Entities;
using UserManagement.Services.Interfaces;
using UserManagement.Web.Models.Logs;

namespace UserManagement.Web.Controllers;

[Route("logs")]
public class LogsController(ILoggingService loggingService, IMapper mapper, IUserService userService) : Controller
{
    private readonly ILoggingService _loggingService = loggingService;
    private readonly IMapper _mapper = mapper;
    private readonly IUserService _userService = userService;

    [HttpGet]
    public IActionResult Index(int pageSize = 15, int pageNumber = 1)
    {
        if (pageSize < 1 || pageNumber < 1)
        {
            return View(nameof(Index), new LogsIndexViewModel
            {
                Error = "Invalid page number."
            });
        }

        var startingId = pageSize * (pageNumber - 1);

        var logs = _loggingService.GetAllLogs()
            .Where(x => x.Id > startingId && x.Id <= startingId + pageSize);

        return View(new LogsIndexViewModel
        {
            Pagination = new LogsPagination
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
                TotalItems = _loggingService.GetAllLogs().Count()
            },
            Logs = logs.Select(_mapper.Map<LoggingEntry, LogEntryViewModel>).ToList()
        });
    }

    [HttpGet("view-user")]
    public IActionResult GetUserView(long userId)
    {
        var user = _userService.GetById(userId);

        if (user is null)
        {
            return View(nameof(Index), new LogsIndexViewModel
            {
                Error = "User no longer exists. They have either been deleted or never existed."
            });
        }

        return RedirectToAction(nameof(UsersController.ViewUser), "Users", new { userId });
    }

    [HttpGet("view-log")]
    public IActionResult GetLogView(long logId)
    {
        var log = _loggingService.GetLogById(logId);

        if (log is null)
        {
            return View(nameof(Index), new LogEntryViewItemViewModel
            {
                Error = "Log entry no longer exists. It has either been deleted or never existed."
            });
        }

        return View("ViewLog", _mapper.Map<LoggingEntry, LogEntryViewItemViewModel>(log));
    }
}
