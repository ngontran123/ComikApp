using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using TestComikApp.Models;
using TestComikApp.Db;
using TestComikApp.Service;
using Org.BouncyCastle.Tls;
using TestComikApp.IRepository;

namespace TestComikApp.Controllers;

public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;

    private readonly EmailRepository _email_repos;

   private readonly ILoginService _login_repos;

   private readonly SupportService _support_service;


    public LoginController(ILogger<LoginController> logger,ILoginService login_repos,EmailRepository email_repos,SupportService support_service)
    {
        _logger = logger;
        this._login_repos=login_repos;
        this._email_repos=email_repos;
        this._support_service=support_service;
    }
 
  
[HttpGet]
public IActionResult Index()
{
 return View();
}

[HttpGet("/Login/Register",Name ="Get Register Form")]
public IActionResult Register()
{
    return View();    
}

[HttpGet("/Login/ForgotPassword",Name ="Get Forgot Password Form")]

public IActionResult Forgotpassword()
{
    return View();
}

 [HttpGet("/Login/ChangePassword",Name="Get Form Change Password")]
    public async Task<IActionResult> ChangePassword(string email,string password)
    {   if(!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
    {
        ViewBag.Email=email;
        ViewBag.Password=password;
        var user=await this._login_repos.GetUser(email);
        if(user!=null)
        {
            user.Password=this._support_service.AddSha256(password);
            Console.WriteLine("Converted password;"+user.Password);
            int val= await this._login_repos.UpdateUser(user);
            _logger.LogInformation("Converted Password successfully");
        }
        else
        {
            Console.WriteLine("Change password:Not exist this user");
            _logger.LogError("Change password:Not exist this user");
        }
    }
        return View();
    }

    // [HttpGet()]
    // public IActionResult ChangePassword(string email)
    // {  Console.WriteLine("Email here is:"+email);
    //     return View();
    // }


    [HttpPost]
    public async Task<IActionResult> Index(LoginModel user)
    {

    if(ModelState.IsValid)
    {
    string username=user.UserName;
    
    string password=user.Password;

    bool check_exist=await _login_repos.CheckExistUser(username,password);
    
    if(check_exist)
    { 
        TempData["LoginFailed"]="False";
        return RedirectToAction("Index","Login");
    }
    else
    {
        Console.WriteLine("This user is not existed yet");
        _logger.LogError("This user is not existed");
        TempData["LoginFailed"]="True";
    }
    }
    // string page_name="https://mangakakalot.to/new?sort=default&page=2";
    // await this._support_service.webScrapingTesting(page_name);
    return RedirectToAction("Index","Login");
}

    public IActionResult ForgotPassword()
    {
        return View();
    }
  
    
    [HttpPost("/Login/Register",Name ="Submit Register Form")]
    public async Task<IActionResult> Register(Model.User user)
    { 
    try{
       if(ModelState.IsValid)
       {
        var user_created=await _login_repos.AddUser(user);

        if(user_created==1)
        {   TempData["ErrorContent"]="Username has existed"; 
        }
        else if(user_created==0)
        {
        TempData["Register"]="True";
        return RedirectToAction("Index","Login");
        }
       }
    TempData["LoginFailed"]="True";
    
    TempData["ErrorContent"]="Register Failed"; 

    _logger.LogError("Register failed");
        }
    catch(Exception er)
    {
    _logger.LogError("Register user:"+er.Message);
    }
    return RedirectToAction("Register","Login");

    }
  
[HttpPost("/Login/ForgotPassword",Name ="Submit Forgot Password Form")]
public async Task<IActionResult> ForgotPassword(string email)
{  ViewBag.SendMail="";
   ViewBag.SendMailMessage= "";
  try{ 
   Console.WriteLine("User email here is:"+email);
   string subject="Receive New Password";
   await this._email_repos.sendEmail(email,subject);
   ViewBag.SendMail="True";
   ViewBag.SendMailMessage="New Password has been sent to your email";
  }
  catch(Exception er)
  {
  ViewBag.SendMail="False";
  ViewBag.SendMailMessage=er.Message;
  _logger.LogError("Forgot password:"+er.Message);
  }
  return View();  
}

[HttpPost("/Login/ChangePassword",Name ="Submit form change password")]

public async Task<IActionResult> ChangePassword(string email,string password,string new_password)
{  Console.WriteLine(new_password);
    int res=await this._login_repos.UpdateUser(email,password,new_password);
    
    if(res==1)
    {
    ViewBag.ChangePassword="False";
    ViewBag.ErrorContent="This email not exist";
    _logger.LogTrace("Change password:This email is not exist");
    }
    else if(res==2)
    {
    ViewBag.ChangePassword="False";
    ViewBag.ErrorContent="Your current password is incorrect";
    _logger.LogTrace("Change password:This email is incorrect");

    }
    else if(res==3)
    {
    ViewBag.ChangePassword="False";
    ViewBag.ErrorContent="Your new password format is incorrect";
    _logger.LogTrace("Your new password format is incorrect");
    }
    else
    {
    TempData["ChangePassword"]="True";

    return RedirectToAction("Index","Login");          
    }
    return View();    
}
   
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
