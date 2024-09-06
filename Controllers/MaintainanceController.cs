using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestComikApp.Models;

namespace TestComikApp.Controllers;

public class MaintainanceController : Controller
{
    private readonly ILogger<MaintainanceController> _logger;

    public MaintainanceController(ILogger<MaintainanceController> logger)
    {
        _logger = logger;
    }
    
   [HttpGet]
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
