# TwelveData
C# Project to get data from the TwelveDataApi currently supports only TimeSeries data call. 
Includes support for API backoff when hitting rate limits (429) from TwelveData. 


### Version
1.3.2 Added support for indices details endpoints to SymbolDetailsService.

1.3.1 Added support for GainToday and PrevivousClose

1.3 Added support for equities and ETF details endpoints.

1.2.9 Added support for a simple quote method, uses TimeSeries data so will be several minutes out of date.

1.2.8 Added throwing of exceptions for symbols which are not found.

1.2.7 Added support for the exchange parameter.

### Example Console App


#### Getting time series data

```C#


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
   Console.WriteLine($"\tDate {stockValueModel.Datetime.ToShortDateString()} Open {stockValueModel.Open} Close {stockValueModel.Close} High {stockValueModel.High} Low {stockValueModel.Low} Volume {stockValueModel.Volume}");
}

```

#### Getting a symbols meta data

When no exchange is provided we will filter by a default country of 'United States'

e.g. https://api.twelvedata.com/stocks?symbol=AAPL&country=United%20States

```C#

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


// UK Example
symbols = this.twelveDataSymbolDetailsService.GetSymbolDetails(this.apiKey, "LLOY", "stock", "LSE").GetAwaiter().GetResult();

// UK ETF result
symbols = this.twelveDataSymbolDetailsService.GetSymbolDetails(this.apiKey, "VUKE", "etf", "LSE").GetAwaiter().GetResult();

```

