using Microsoft.AspNetCore.Mvc;
using TestComikApp.Db;
using TestComikApp.Model;
namespace TestComikApp.IRepository;


public interface ILoginService
{
 Task<bool> CheckExist(string username);

 Task<bool> CheckExistUser(string username,string password);

 Task<int> AddUser(User user);

 Task<int> UpdateUser(string email,string password,string new_password);

 Task<int> UpdateUser(User user);

 Task<User> GetUser(string email);
}
