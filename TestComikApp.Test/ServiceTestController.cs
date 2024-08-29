using Xunit;
using Moq;
using TestComikApp.Service;
using Microsoft.AspNetCore.Mvc;

public class ServiceTestController
{
    [Fact]
   public async void webScrapingTest()
   {
    var services=new SupportService();
    
    await services.webScrapingTesting();
   }
}