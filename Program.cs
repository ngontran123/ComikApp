
using Microsoft.EntityFrameworkCore;
using TestComikApp.Db;
using TestComikApp.Model;
using TestComikApp.Models;
using TestComikApp.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using DotNetEnv;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ComicBookContext>(options=>options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ILoginService,LoginRepository>();

builder.Services.AddTransient<EmailRepository>();

builder.Services.AddTransient<SupportService>();

builder.Services.Configure<SmtpModel>(builder.Configuration.GetSection("SmtpModel"));

// builder.Services.AddMvc().AddRazorPagesOptions(options=>{
//     options.Conventions.AddAreaPageRoute("Identity","/Login","");
// });

// builder.Services.AddAuthentication(options=>{
//     options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
// }).AddJwtBearer(options=>{
//   options.TokenValidationParameters=new TokenValidationParameters{
//     ValidateIssuer=true,
//     ValidateAudience=true,
//     ValidateLifetime=true,
//     ValidateIssuerSigningKey=true,
//     ValidIssuer="localhost.com:5223",
//     IssuerSigningKey=new SymmetricSecurityKey(jwt_key),
//     ClockSkew=TimeSpan.Zero
//   };
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();    
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name:"default",
    pattern:"{controller=Login}/{action=Index}/{id?}"
);

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.MapControllerRoute(
    name: "Home",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


