using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestComikApp.Models;

namespace TestComikApp.Controllers;

public class UserInfoController : Controller
{
    private readonly ILogger<UserInfoController> _logger;

    public UserInfoController(ILogger<UserInfoController> logger)
    {
        _logger = logger;        
    }
   [Route("profile-data")]
    public IActionResult Index()
    {
        return View();                                
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
