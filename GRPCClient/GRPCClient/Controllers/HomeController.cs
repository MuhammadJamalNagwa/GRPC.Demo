using GRPCClient.Models;
using GRPCClient.Repos.UserRepo;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GRPCClient.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;

    public HomeController(ILogger<HomeController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public async Task<IActionResult> Index(int pageSize = 25, int pageNumber = 1)
    {
        UserListResponse? model = await _userService.GetUsersListAsync(pageSize, pageNumber);
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public async Task<IActionResult> Users(int pageSize = 10, int pageNumber = 1)
    {
        UserListResponse? model = await _userService.GetUsersListAsync(pageSize, pageNumber);
        return View(model);
    }

    public async Task<IActionResult> DeleteUser(int userId)
    {
        var response = _userService.DeleteUserAsync(userId);
        return RedirectToAction(nameof(Index));
    }
}
