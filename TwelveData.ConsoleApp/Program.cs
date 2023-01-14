// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Logging.Abstractions;
using TwelveData.Services.Enums;
using TwelveData.Services.Models;
using TwelveData.Services.Services;

TwelveDataService twelveDataService = new TwelveDataService(new NullLogger<TwelveDataService>());

string apiKey = "39795d190bb84f649c9118d694ec15f8";
QueryResultsModel queryResult = twelveDataService.GetTimeSeriesDaily(apiKey,
      "AAPL",
      EnumDataSize.Compact)
   .GetAwaiter()
   .GetResult();

Console.WriteLine($"Information for '{queryResult.Meta.Symbol}' on Exchange {queryResult.Meta.Exchange} with interval {queryResult.Meta.Interval}!");
foreach (StockValueModel stockValueModel in queryResult.Values.OrderByDescending(p=>p.Datetime))
{
   Console.WriteLine($"\tDate {stockValueModel.Datetime.ToShortDateString()} Open {stockValueModel.Open} Close {stockValueModel.Close} High {stockValueModel.High} Low {stockValueModel.Low} Volume {stockValueModel.Volume}");
}