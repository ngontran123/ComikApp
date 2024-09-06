using Microsoft.EntityFrameworkCore;
using TestComikApp.Db;
using TestComikApp.Service;
using TestComikApp.Model;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Numerics;
using MimeKit.Cryptography;
using TestComikApp.IRepository;
using System.Text.RegularExpressions;

namespace TestComikApp.Service;

public class LoginRepository:ILoginService
{
   // private readonly ApplicationDbContext _db;
    private readonly ComicBookContext _dbb;

    private readonly SupportService _support_service;
    public LoginRepository(ComicBookContext dbb,SupportService support_service)
    {
        this._dbb=dbb;
        this._support_service=support_service;
    }
 public async Task<bool> CheckExist(string username)
 {
    var is_existed=await _dbb.Users.FirstOrDefaultAsync(p=>p.Username==username);
    if(is_existed!=null)
    {
        return true;        
    }
    return false;    
 }

  public async Task<bool> CheckExistUser(string username,string password)
 {  
    string hashed_passwod=this._support_service.AddSha256(password);
    var is_existed=await _dbb.Users.FirstOrDefaultAsync(p=>(p.Username==username || p.Email==username) && p.Password==hashed_passwod);
    if(is_existed!=null)
    {
        return true;        
    }
    return false;
 }


  public async Task<bool> CheckExistEmail(string email)
 {
    var is_existed=await _dbb.Users.FirstOrDefaultAsync(p=>p.Email==email);

    if(is_existed!=null)
    {
        return true;        
    }
    return false;
 }

 public async Task<bool> CheckPassword(string password)
 {  string hashed_password=this._support_service.AddSha256(password);
    
    var is_existed=await _dbb.Users.FirstOrDefaultAsync(p=>p.Password==hashed_password);
    
    if(is_existed!=null)
    {
        return true;                
    }
    return false;            
 }


 public async Task<int> AddUser(User user)
 {  
    int res;
    string username=user.Username;
    string password=this._support_service.AddSha256(user.Password);
    string email=user.Email;
    string phone_number=user.Phonenumber;
    string gender=user.Gender;
    string avatar=user.Avatar;

    bool checkExist=await CheckExist(username);
    
    if(checkExist)
    {
        res=1;
        return res;
    }
    bool checkExistMail = await CheckExistEmail(email);
    
    if(checkExistMail)
    {
        res=2;
        return res;
    }
    Console.WriteLine("Username:"+username);
    
    Console.WriteLine("Password:"+password);

    Console.WriteLine("Email:"+email);

    Console.WriteLine("Phone Number:"+phone_number);

    Console.WriteLine("Gender:"+gender);

    Console.WriteLine("Avatar:"+avatar);

    user.Password=password;

    await this._dbb.AddAsync(user);

    await this._dbb.SaveChangesAsync();
    res=0;
    return res;
    }

public async Task<int> UpdateUser(string email,string password,string new_password)
{
    int res=0;
  try{
    var user=await this._dbb.Users.FirstOrDefaultAsync(p=>p.Email==email);
    if(user==null)
    {
        res=1;
        return res;        
    }
    else
    {  
      string? user_password=user.Password;
      string hashed_passwod=this._support_service.AddSha256(password);
      if(user_password!=hashed_passwod)
      {
        res=2;
        return res;
      }
      else
      {
    Regex check_password_valid=new Regex(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W)");
    Match check_match=check_password_valid.Match(new_password);
    if(check_match.Success)
    {
     string new_hashed_password=this._support_service.AddSha256(new_password);
     user.Password=new_hashed_password; 
      this._dbb.Users.Update(user);
      await this._dbb.SaveChangesAsync();
      }
      else
      {
     res=3;
     return res;
      }
    }
  
    }
  }
  catch(Exception er)
  {
    Console.WriteLine("Update Exception:"+er.Message);
  }
    return res;
}

public async Task<int> UpdateUser(User user)
{
    int res=0;
    this._dbb.Users.Update(user);
    await this._dbb.SaveChangesAsync();
    return res;
}

public async Task<User> GetUser(string email)
{
    var user=await this._dbb.Users.FirstOrDefaultAsync(p=>p.Email==email);
    return user;
}
}