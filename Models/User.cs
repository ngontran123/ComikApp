using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace TestComikApp.Models;

public class User
{
    public int Id{get;set;}
    public string? Username{get;set;}
    public string? Password{get;set;}
    
    public string? Email{get;set;}
    public string? Gender{get;set;}
    public string? Avatar{get;set;} 
    public string? Phonenumber{get;set;}
}