using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using TestComikApp.Models;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Tls;
using System.IO;
using System.Reflection;
using TestComikApp.Service;
using System.Diagnostics;
namespace TestComikApp.Service;
public class EmailRepository
{
private readonly SmtpModel _smtpClient;

private readonly SupportService _spService;
public EmailRepository(IOptions<SmtpModel> smtpClient,SupportService spService)
{
    this._smtpClient=smtpClient.Value;
    this._spService=spService;
}


public string htmlContent(string receiver,string operating_system,string browser_name)
{
    string htmlContent="";
    
    string path=this._spService.GetCurrentFilePath("Views/Login/SendMailTemplate.html");
    
    using(StreamReader sr=new StreamReader(path))
    {
        htmlContent=sr.ReadToEnd();
    }

    string random_password=this._spService.generateRandomPassword();

    htmlContent=htmlContent.Replace("{name}",receiver);
    htmlContent=htmlContent.Replace("{operating_system}",operating_system);
    htmlContent=htmlContent.Replace("{browser_name}",browser_name);
    htmlContent=htmlContent.Replace("{new_password}",random_password);
    htmlContent=htmlContent.Replace("{email_value}",receiver);
    return htmlContent;
}


// public string getCurrentBrowser()
// {
//     string web_brower=HttpContext..Headers["User-Agent"].ToString();
// }
public async Task sendEmail(string receiver,string subject)
{    

    Console.WriteLine("Port:"+this._smtpClient.Port);
    Console.WriteLine("Username:"+this._smtpClient.Username);
        Console.WriteLine("Password:"+this._smtpClient.Password);

    Console.WriteLine("SenderName:"+this._smtpClient.SenderName);

    Console.WriteLine("SenderEmail:"+this._smtpClient.SenderEmail);

    Console.WriteLine("Usessl:"+this._smtpClient.UseSsl);

    Console.WriteLine("Host:"+this._smtpClient.Host);

    string currentOs=this._spService.getCurrentOs();
    string htmlValue=htmlContent(receiver,currentOs,"Chrome");
    Console.WriteLine(currentOs);
   var emailMessage = new MimeMessage();
   emailMessage.From.Add(new MailboxAddress(this._smtpClient.SenderName,this._smtpClient.SenderEmail));
   emailMessage.To.Add(new MailboxAddress("",receiver));
   emailMessage.Subject=subject;
   
   var bodyBuilder =new BodyBuilder{HtmlBody=htmlValue};
   
   emailMessage.Body=bodyBuilder.ToMessageBody();   
   
   using(var client = new SmtpClient())
   {
      await client.ConnectAsync(_smtpClient.Host,this._smtpClient.Port,this._smtpClient.UseSsl);
      await client.AuthenticateAsync(this._smtpClient.Username,this._smtpClient.Password);
      await client.SendAsync(emailMessage);
      await client.DisconnectAsync(true);
   }
}

}