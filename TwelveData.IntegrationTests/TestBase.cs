using Microsoft.Extensions.Configuration;

namespace TwelveData.IntegrationTests;

public abstract class TestBase
{
   protected static IConfiguration InitConfiguration()
   {
      IConfigurationRoot config = new ConfigurationBuilder()
         .AddJsonFile("appsettings.json")
         .AddJsonFile("appsettings.test.json")
         .AddEnvironmentVariables() 
         .Build();
      return config;
   }
}