using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestComikApp.Models;

namespace TestComikApp.Controllers;

public class NotFoundController : Controller
{
    private readonly ILogger<NotFoundController> _logger;

    public NotFoundController(ILogger<NotFoundController> logger)
    {
        _logger = logger;
    }
    
   [Route("Error/404")]
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
