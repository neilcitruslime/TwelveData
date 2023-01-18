using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using NuGet.Frameworks;
using TwelveData.Services.Enums;
using TwelveData.Services.Models;
using TwelveData.Services.Services;

namespace TwelveData.IntegrationTests;

public class TimeSeriesDailyTests : TestBase
{
   private const string AppleTicker = "AAPL";
   private const string FTSE100 = "FTSE";
   private const string NASDAQ = "IXIC";
   private string apiKey;
   
   [SetUp]
   public void Setup()
   {
      this.apiKey = InitConfiguration()["TwelveDataApiKey"];
   }

   [Test]
   public void CheckSymbol()
   {
      
      TwelveDataService twelveDataService = new TwelveDataService(new NullLogger<TwelveDataService>());

      QueryResultsModel queryResult = twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty).GetAwaiter().GetResult();

      Assert.That(queryResult.Meta.Symbol, Is.EqualTo(AppleTicker));
   }

   [Test]
   public void CheckThrowsOnNotASymbol()
   {

      TwelveDataService twelveDataService = new TwelveDataService(new NullLogger<TwelveDataService>());
      string notASymbol = "EXAMPLENA";
      Assert.Throws(Is.TypeOf<Exception>()
            .And.Message.EqualTo($"Cannot find ticker code '{notASymbol}'"),
         () =>
         {
            QueryResultsModel queryResult = twelveDataService
               .GetTimeSeriesDaily(apiKey, notASymbol, EnumDataSize.Compact, string.Empty).GetAwaiter().GetResult();
         });
   }

   
   [Test]
   public void CompactRecordsRetrieve()
   {
      TwelveDataService twelveDataService = new TwelveDataService(new NullLogger<TwelveDataService>());
      QueryResultsModel queryResult = twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty).GetAwaiter().GetResult();

      Assert.That(queryResult.Values.Count, Is.EqualTo(30));
   }
   
   [Test]
   public void QuickRateLimitTest()
   {
      TwelveDataService twelveDataService = new TwelveDataService(new NullLogger<TwelveDataService>());
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);
      twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty);

      QueryResultsModel queryResult = twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty).GetAwaiter().GetResult();
      Assert.That(queryResult.Values.Count, Is.EqualTo(30));
   }
   
   [Test]
   public void FullRecordsRetrieve()
   {
      TwelveDataService twelveDataService = new TwelveDataService(new NullLogger<TwelveDataService>());
      QueryResultsModel queryResult = twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Full, string.Empty).GetAwaiter().GetResult();

      Assert.That(queryResult.Values.Count, Is.AtLeast(1000));
   }
   
   [Test]
   public void BpNyse()
   {
      TwelveDataService twelveDataService = new TwelveDataService(new NullLogger<TwelveDataService>());
      QueryResultsModel queryResult = twelveDataService.GetTimeSeriesDaily(apiKey, "BP", EnumDataSize.Compact, string.Empty).GetAwaiter().GetResult();
       
      Assert.That(queryResult.Meta.Exchange, Is.EqualTo("NYSE"));
   }
   
   
   [Test]
   public void BpLse()
   {
      TwelveDataService twelveDataService = new TwelveDataService(new NullLogger<TwelveDataService>());
      QueryResultsModel queryResult = twelveDataService.GetTimeSeriesDaily(apiKey, "BP", EnumDataSize.Compact, "LSE").GetAwaiter().GetResult();
       
      Assert.That(queryResult.Meta.Exchange, Is.EqualTo("LSE"));
   }
   
   [Test]
   public void AppleCloseSpecificDate()
   {
      TwelveDataService twelveDataService = new TwelveDataService(new NullLogger<TwelveDataService>());
      QueryResultsModel queryResult = twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Full, string.Empty).GetAwaiter().GetResult();

      decimal close = queryResult.Values.Where(p => p.Datetime == new DateTime(2012, 9, 14))
         .Select(p => p.Close)
         .Single();
      Assert.That(Math.Round(close, 2), Is.EqualTo(24.69M));
   }
   
   [Test]
   public void NasdaqFullValuesSpecificDate()
   {
      TwelveDataService twelveDataService = new TwelveDataService(new NullLogger<TwelveDataService>());
      QueryResultsModel queryResult = twelveDataService.GetTimeSeriesDaily(apiKey, NASDAQ, EnumDataSize.Full, string.Empty).GetAwaiter().GetResult();

      StockValueModel stockPrice = queryResult.Values.Where(p => p.Datetime == new DateTime(2023, 1, 11))
         .Single();
      
      Assert.That(Math.Round(stockPrice.Close, 2), Is.EqualTo(10931.67M));
      Assert.That(Math.Round(stockPrice.Low, 2), Is.EqualTo(10762.73M));
      Assert.That(Math.Round(stockPrice.High, 2), Is.EqualTo(10932.44M));
      Assert.That(Math.Round(stockPrice.Open, 2), Is.EqualTo(10794.99M));
   }
}