# TwelveData
C# Project to get data from the TwelveDataApi currently supports only TimeSeries data call. 
Includes support for API backoff when hitting rate limits (429) from TwelveData. 

#### Example Console App
```

using Microsoft.Extensions.Logging.Abstractions;
using TwelveData.Services.Enums;
using TwelveData.Services.Models;
using TwelveData.Services.Services;

TwelveDataService twelveDataService = new TwelveDataService(new NullLogger<TwelveDataService>());

string apiKey = "your key here";
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

```



