using Microsoft.EntityFrameworkCore;
using TestComikApp.Models;
namespace TestComikApp.Db;


public class ApplicationDbContext:DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
    {
    }
    public virtual DbSet<User> User{get;set;}
}