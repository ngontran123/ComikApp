using System.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
using System.Management;
using Microsoft.Playwright;

namespace TestComikApp.Service;

public class SupportService
{
     public string AddSha256(string data)
 {
    using(SHA256 hash=SHA256.Create())
    {
        byte[] bytes=hash.ComputeHash(Encoding.UTF8.GetBytes(data));

        StringBuilder sha_hash=new StringBuilder();
        
        for(int i=0;i<bytes.Length;i++)
        {
            sha_hash.Append(bytes[i].ToString("x2"));
        }
        return sha_hash.ToString();
    }
 }

 public string GetCurrentFilePath(string file_name)
 {
    string full_file_path=Path.Combine(Directory.GetCurrentDirectory(),file_name);
    return full_file_path;
 }

 public string generateRandomPassword()
{
string guid=Guid.NewGuid().ToString();
return guid;
}

public string getCurrentOs()
{
var os_name = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                      select x.GetPropertyValue("Caption")).FirstOrDefault();

return os_name != null ? os_name.ToString() : "Unknown";
}

public async Task webScrapingTesting()
{
    using(var playwright=await Playwright.CreateAsync())
    {
        var browser=await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions{Headless=false});
        var page = await browser.NewPageAsync();
        await page.GotoAsync("https://parkmanga.com/");
        await page.GetByRole(AriaRole.Link,new(){Name="Browse"}).ClickAsync();
        await page.ScreenshotAsync(new PageScreenshotOptions{Path="screenshot.png"});
        await browser.CloseAsync();                        
    }
}

}