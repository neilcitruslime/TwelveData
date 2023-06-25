using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using NuGet.Frameworks;
using TwelveData.Services.Enums;
using TwelveData.Services.Models;
using TwelveData.Services.Services;

namespace TwelveData.IntegrationTests;

using Services.Classes;

public class TimeSeriesDailyTests : TestBase
{
   private const string AppleTicker = "AAPL";
   private const string FTSE100 = "FTSE";
   private const string NASDAQ = "IXIC";
   private string apiKey;
   private TwelveDataService twelveDataService;

   [SetUp]
   public void Setup()
   {
      this.apiKey = InitConfiguration()["TwelveDataApiKey"];
      this.twelveDataService = new TwelveDataService(new NullLogger<TwelveDataService>(), new RetryManager(new NullLogger<RetryManager>()), new HttpClient());
   }

   [Test]
   public void CheckSymbol()
   {
      

      QueryResultsModel queryResult = twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty).GetAwaiter().GetResult();

      Assert.That(queryResult.Meta.Symbol, Is.EqualTo(AppleTicker));
   }

   [Test]
   public void CheckThrowsOnNotASymbol()
   {
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
   public void Quote()
   {
      QueryResultsModel queryResult = twelveDataService.Quote(apiKey, AppleTicker,  string.Empty).GetAwaiter().GetResult();

      Assert.That(queryResult.Values.Count, Is.EqualTo(1));
   }
   
   [Test]
   public void QuoteFtse()
   {
      QueryResultsModel queryResult = twelveDataService.Quote(apiKey, "FTSE",  "LSE").GetAwaiter().GetResult();

      Assert.That(queryResult.Values.Count, Is.EqualTo(1));
   }

   [Test]
   public void QuoteNasdaq()
   {
      QueryResultsModel queryResult = twelveDataService.Quote(apiKey, "IXIC",  "").GetAwaiter().GetResult();

      Assert.That(queryResult.Values.Count, Is.EqualTo(1));
   }
   
   [Test]
   public void QuoteSpx()
   {
      QueryResultsModel queryResult = twelveDataService.Quote(apiKey, "SPX",  "").GetAwaiter().GetResult();

      Assert.That(queryResult.Values.Count, Is.EqualTo(1));
   }

   
   [Test]
   public void CompactRecordsRetrieve()
   {
      QueryResultsModel queryResult = twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Compact, string.Empty).GetAwaiter().GetResult();

      Assert.That(queryResult.Values.Count, Is.EqualTo(30));
   }
   
   [Test]
   public void QuickRateLimitTest()
   {
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
      QueryResultsModel queryResult = twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Full, string.Empty).GetAwaiter().GetResult();

      Assert.That(queryResult.Values.Count, Is.AtLeast(1000));
   }
   
   [Test]
   public void BpNyse()
   {
      QueryResultsModel queryResult = twelveDataService.GetTimeSeriesDaily(apiKey, "BP", EnumDataSize.Compact, string.Empty).GetAwaiter().GetResult();
       
      Assert.That(queryResult.Meta.Exchange, Is.EqualTo("NYSE"));
   }
   
   
   [Test]
   public void BpLse()
   {
      QueryResultsModel queryResult = twelveDataService.GetTimeSeriesDaily(apiKey, "BP", EnumDataSize.Compact, "LSE").GetAwaiter().GetResult();
       
      Assert.That(queryResult.Meta.Exchange, Is.EqualTo("LSE"));
   }
   
   [Test]
   public void AppleCloseSpecificDate()
   {
      QueryResultsModel queryResult = twelveDataService.GetTimeSeriesDaily(apiKey, AppleTicker, EnumDataSize.Full, string.Empty).GetAwaiter().GetResult();

      decimal close = queryResult.Values.Where(p => p.Datetime == new DateTime(2012, 9, 14))
         .Select(p => p.Close)
         .Single();
      Assert.That(Math.Round(close, 2), Is.EqualTo(24.69M));
   }
   
   [Test]
   public void NasdaqFullValuesSpecificDate()
   {
      QueryResultsModel queryResult = twelveDataService.GetTimeSeriesDaily(apiKey, NASDAQ, EnumDataSize.Full, string.Empty).GetAwaiter().GetResult();

      StockValueModel stockPrice = queryResult.Values
         .Single(p => p.Datetime == new DateTime(2023, 1, 11));
      
      Assert.That(Math.Round(stockPrice.Close, 2), Is.EqualTo(10931.67M));
      Assert.That(Math.Round(stockPrice.Low, 2), Is.EqualTo(10762.73M));
      Assert.That(Math.Round(stockPrice.High, 2), Is.EqualTo(10932.44M));
      Assert.That(Math.Round(stockPrice.Open, 2), Is.EqualTo(10794.99M));
   }
   
   // No license for this data
   [Test]
   public void Nikkei225()
   {
      QueryResultsModel queryResult = twelveDataService.GetTimeSeriesDaily(apiKey, "N225", EnumDataSize.Full, "JPX").GetAwaiter().GetResult();

      Assert.That(queryResult.Values.Any() == false);
   }
}