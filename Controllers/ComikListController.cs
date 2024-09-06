using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestComikApp.Models;

namespace TestComikApp.Controllers;

public class ComikListController : Controller
{
    private readonly ILogger<ComikListController> _logger;

    public ComikListController(ILogger<ComikListController> logger)
    {
        _logger = logger;
    }

    [Route("latest-comik")]
    public IActionResult Index()
    {
        return View();
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
}
