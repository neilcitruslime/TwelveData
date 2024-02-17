using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TwelveData.Services.Builders;
using TwelveData.Services.Enums;
using TwelveData.Services.Models;

namespace TwelveData.Services.Services
{
   using Interfaces;
   using TwelveData.Services.Services.Abstract;

   public class TwelveDataServiceService : AbstractTwelveDataService
   {
      private readonly ILogger<TwelveDataServiceService> logger;
      private readonly IRetryManager retryManager;
      private readonly HttpClient client;

      private const string TimeSeries = "time_series";

      private static readonly  int maxRetryAttempts = 10;
      private static readonly TimeSpan pauseBetweenFailures = TimeSpan.FromSeconds(60);

      public TwelveDataServiceService(ILogger<TwelveDataServiceService> logger, IRetryManager retryManager, HttpClient client)
      {
         this.logger = logger;
         this.retryManager = retryManager;
         this.client = client;
      }

      public  async  Task<QueryResultsModel> Quote(string apiKey, string symbol, string exchange)
      {
         return await this.RunTimeSeriesQuery(apiKey, symbol, EnumDataSize.Quote, exchange, period: "1min");
      }
      
      public async Task<QueryResultsModel> GetTimeSeriesDaily(string apiKey, string symbol, EnumDataSize dataSize, string exchange)
      {
         return await this.RunTimeSeriesQuery(apiKey, symbol, dataSize, exchange, period: "1day");
      }

      private async Task<QueryResultsModel> RunTimeSeriesQuery(string apiKey, string symbol, EnumDataSize dataSize, string exchange, string period)
      {
         string body = string.Empty;

         await this.retryManager.RetryOnExceptionAsync(maxRetryAttempts, pauseBetweenFailures, async () =>
         {
            RequestBuilder requestBuilder = new RequestBuilder();

            HttpRequestMessage request = new HttpRequestMessage
            {
               Method = HttpMethod.Get,
               RequestUri = requestBuilder.BuildRequestTimeServiesUri(TimeSeries, apiKey, period, symbol, dataSize, exchange),
            };
            body = await this.MakeApiCall(this.client, request, symbol);
         });

         return JsonConvert.DeserializeObject<QueryResultsModel>(body);
      }


   }
}