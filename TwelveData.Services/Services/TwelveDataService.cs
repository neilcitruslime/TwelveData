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

      public TwelveDataService(ILogger<TwelveDataService> logger)
      {
         this.logger = logger;
      }

      public async Task<QueryResultsModel> GetTimeSeries(string apiKey, string symbol, EnumDataSize dataSize)
      {
         RequestBuilder requestBuilder = new RequestBuilder();

         HttpRequestMessage request = new HttpRequestMessage
         {
            Method = HttpMethod.Get,
            RequestUri = requestBuilder.BuildRequestTimeServiesUri(TimeSeries, apiKey, "1day", symbol, dataSize),
         };
         
         string body = string.Empty;
         using (HttpResponseMessage response = await client.SendAsync(request))
         {
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();

            //{"code":429,"message":"You have run out of API credits for the current minute. 18 API credits were used, with the current limit being 8. Wait for the next minute or consider switching to a higher tier plan at https://twelvedata.com/pricing","status":"error"}
            Console.WriteLine(body);

            return JsonConvert.DeserializeObject<QueryResultsModel>(body);
         }
      }
   }
}