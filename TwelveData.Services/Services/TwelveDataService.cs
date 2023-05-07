using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TwelveData.Services.Builders;
using TwelveData.Services.Enums;
using TwelveData.Services.Models;

namespace TwelveData.Services.Services
{
   using Interfaces;

   public class TwelveDataService
   {
      private readonly ILogger<TwelveDataService> logger;
      private readonly IRetryManager retryManager;
      private readonly HttpClient client;

      private const string TimeSeries = "time_series";

      private static readonly string InvalidTickerError = "Cannot find ticker code";
      private static readonly  int maxRetryAttempts = 10;
      private static readonly TimeSpan pauseBetweenFailures = TimeSpan.FromSeconds(60);

      public TwelveDataService(ILogger<TwelveDataService> logger, IRetryManager retryManager, HttpClient client)
      {
         this.logger = logger;
         this.retryManager = retryManager;
         this.client = client;
      }

      public  async  Task<QueryResultsModel> Quote(string apiKey, string symbol, string exchange)
      {
         return await RunTimeSeriesQuery(apiKey, symbol, EnumDataSize.Quote, exchange, period: "1min");
      }
      
      public async Task<QueryResultsModel> GetTimeSeriesDaily(string apiKey, string symbol, EnumDataSize dataSize, string exchange)
      {
         return await RunTimeSeriesQuery(apiKey, symbol, dataSize, exchange, period: "1day");
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
            body = await MakeApiCall(request, symbol);
         });

         return JsonConvert.DeserializeObject<QueryResultsModel>(body);
      }

      private async Task<string> MakeApiCall(HttpRequestMessage request, string symbol)
      {
         string body;
         using (HttpResponseMessage response = await this.client.SendAsync(request))
         {
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();

            //{"code":429,"message":"You have run out of API credits for the current minute. 18 API credits were used, with the current limit being 8. Wait for the next minute or consider switching to a higher tier plan at https://twelvedata.com/pricing","status":"error"}
            if (body.Contains("\"code\":429") && body.Contains("\"status\":\"error\""))
            {
               throw new Exception("Rate Limiting Hit");
            }
            
            if (body.Contains("\"code\":400") && body.ToLower().Contains("not found:"))
            {
               throw new Exception($"{InvalidTickerError} '{symbol}'");
            }
         }

         return body;
      }
   }
}