// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Logging.Abstractions;
using TwelveData.Services.Classes;
using TwelveData.Services.Enums;
using TwelveData.Services.Models;
using TwelveData.Services.Services;

string apiKey = "39795d190bb84f649c9118d694ec15f8";

TwelveDataService twelveDataService = new TwelveDataService(new NullLogger<TwelveDataService>(), new RetryManager(new NullLogger<RetryManager>()), new HttpClient());

QueryResultsModel queryResult = twelveDataService.GetTimeSeriesDaily(apiKey,
      "AAPL",
      EnumDataSize.Compact, 
      string.Empty)
   .GetAwaiter()
   .GetResult();

Console.WriteLine($"Information for '{queryResult.Meta.Symbol}' on Exchange {queryResult.Meta.Exchange} with interval {queryResult.Meta.Interval}!");
foreach (StockValueModel stockValueModel in queryResult.Values.OrderByDescending(p=>p.Datetime))
{
   Console.WriteLine($"\tDate {stockValueModel.Datetime.ToShortDateString()} Open {stockValueModel.Open} Close {stockValueModel.Close} High {stockValueModel.High} Low {stockValueModel.Low} Volume {stockValueModel.Volume} GainToday {stockValueModel.GainToday} Previvous Close {stockValueModel.PreviousClose}");
}

TwelveDataSymbolDetailsService twelveDataSymbolDetailsService = new TwelveDataSymbolDetailsService(new NullLogger<TwelveDataSymbolDetailsService>(), new RetryManager(new NullLogger<RetryManager>()), new HttpClient());
List<SymbolDetailsModel> symbolDetailsModels = twelveDataSymbolDetailsService.GetSymbolDetails(apiKey, "AAPL", "stocks", string.Empty).GetAwaiter().GetResult();

foreach (SymbolDetailsModel symbolDetailsModel in symbolDetailsModels)
{
   Console.WriteLine($"Symbol: {symbolDetailsModel.Symbol}\n" +
                     $"Name: {symbolDetailsModel.Name}\n" +
                     $"Exchange: {symbolDetailsModel.Exchange}\n" +
                     $"Country: {symbolDetailsModel.Country}\n" +
                     $"Type: {symbolDetailsModel.Type}\n" +
                     $"Currency: {symbolDetailsModel.Currency} ");
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey(true);