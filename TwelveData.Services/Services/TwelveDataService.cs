using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TwelveData.Services.Builders;
using TwelveData.Services.Enums;
using TwelveData.Services.Models;

namespace TwelveData.Services.Services
{
   public class TwelveDataService
   {
      private readonly ILogger<TwelveDataService> logger;
      private static HttpClient client = new HttpClient();

      private const string TimeSeries = "time_series";

      private static readonly string InvalidTickerError = "Cannot find ticker code";

      public TwelveDataService(ILogger<TwelveDataService> logger)
      {
         this.logger = logger;
      }

      public  async  Task<QueryResultsModel> Quote(string apiKey, string symbol, string exchange)
      {
         string body = string.Empty;

         var maxRetryAttempts = 10;
         var pauseBetweenFailures = TimeSpan.FromSeconds(60);

         await RetryOnExceptionAsync(maxRetryAttempts, pauseBetweenFailures, async () =>
         {
            RequestBuilder requestBuilder = new RequestBuilder();

            HttpRequestMessage request = new HttpRequestMessage
            {
               Method = HttpMethod.Get,
               RequestUri = requestBuilder.BuildRequestTimeServiesUri(TimeSeries, apiKey, "1min", symbol, EnumDataSize.Quote,  exchange),
            };
            body = await MakeApiCall(request, symbol);
         });

         return JsonConvert.DeserializeObject<QueryResultsModel>(body);
      }
      
      public async Task<QueryResultsModel> GetTimeSeriesDaily(string apiKey, string symbol, EnumDataSize dataSize, string exchange)
      {
         string body = string.Empty;

         var maxRetryAttempts = 10;
         var pauseBetweenFailures = TimeSpan.FromSeconds(60);

         await RetryOnExceptionAsync(maxRetryAttempts, pauseBetweenFailures, async () =>
         {
            RequestBuilder requestBuilder = new RequestBuilder();

            HttpRequestMessage request = new HttpRequestMessage
            {
               Method = HttpMethod.Get,
               RequestUri = requestBuilder.BuildRequestTimeServiesUri(TimeSeries, apiKey, "1day", symbol, dataSize, exchange),
            };
            body = await MakeApiCall(request, symbol);
         });

         return JsonConvert.DeserializeObject<QueryResultsModel>(body);
      }

      private async Task<string> MakeApiCall(HttpRequestMessage request, string symbol)
      {
         string body;
         using (HttpResponseMessage response = await client.SendAsync(request))
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

      public async Task RetryOnExceptionAsync(
         int times, TimeSpan delay, Func<Task> operation)
      {
         await RetryOnExceptionAsync<Exception>(times, delay, operation);
      }

      public async Task RetryOnExceptionAsync<TException>(
         int times, TimeSpan delay, Func<Task> operation) where TException : Exception
      {
         if (times <= 0)
            throw new ArgumentOutOfRangeException(nameof(times));

         var attempts = 0;
         do
         {
            try
            {
               attempts++;
               await operation();
               break;
            }
            catch (TException ex)
            {
               if (attempts == times || ex.Message.Contains("Rate Limiting Hit") == false)
                  throw;

               await CreateDelayForException(times, attempts, delay, ex);
            }
         } while (true);
      }

      private Task CreateDelayForException(
         int times, int attempts, TimeSpan delay, Exception ex)
      {
         this.logger.LogWarning($"Exception on attempt {attempts} of {times}. " +
                  "Will retry after sleeping for {delay}.", ex);
         return Task.Delay(delay);
      }

   }
}