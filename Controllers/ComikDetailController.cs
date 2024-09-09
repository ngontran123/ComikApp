using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestComikApp.Models;

namespace TestComikApp.Controllers;

public class ComikDetailController : Controller
{
    private readonly ILogger<ComikDetailController> _logger;

    public ComikDetailController(ILogger<ComikDetailController> logger)
    {
        _logger = logger;
    }

    [Route("title")]
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
