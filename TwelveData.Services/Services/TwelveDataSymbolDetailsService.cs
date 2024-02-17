namespace TwelveData.Services.Services;

using Builders;
using Interfaces;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;

public class TwelveDataSymbolDetailsService
{
   private readonly HttpClient client;
   private readonly ILogger<TwelveDataSymbolDetailsService> logger;
   private readonly IRetryManager retryManager;
   
   private static readonly int maxRetryAttempts = 10;
   private static readonly TimeSpan pauseBetweenFailures = TimeSpan.FromSeconds(60);
   
   public TwelveDataSymbolDetailsService(ILogger<TwelveDataSymbolDetailsService> logger, IRetryManager retryManager, HttpClient client)
   {
      this.client = client;
      this.logger = logger;
      this.retryManager = retryManager;
   }
   
   public async Task<List<SymbolDetailsModel>> GetSymbolDetails(string apiKey, string symbol, string queryType, string exchange)
   {
      if (queryType == null || queryType.ToLower() != "etf" && queryType.ToLower() != "stocks" && queryType.ToLower() != "indices")
      {
         throw new ArgumentOutOfRangeException(nameof(queryType), "Query type must be either 'etf', 'stock' or 'indices'");
      }

      string body = string.Empty;

      await this.retryManager.RetryOnExceptionAsync(maxRetryAttempts, pauseBetweenFailures, async () =>
      {
         RequestBuilder requestBuilder = new RequestBuilder();

         HttpRequestMessage request = new HttpRequestMessage
         {
            Method = HttpMethod.Get,
            RequestUri = requestBuilder.BuildRequestSymbolDetailsUri(queryType, apiKey, symbol, exchange),
         };
         body = await this.MakeApiCall(request, symbol);
      });

      SymbolDetailsModelContainer container = JsonConvert.DeserializeObject<SymbolDetailsModelContainer>(body);

      if (queryType.ToLower() == "etf")
      {
         container.SymbolsDetails.ForEach(x => x.Type = "ETF");
      }

      if (queryType.ToLower() == "indices")
      {
         container.SymbolsDetails.ForEach(x => x.Type = "Indices");
      }
      
      return container.SymbolsDetails;
   }

   private async Task<string> MakeApiCall(HttpRequestMessage request, string symbol)
   {
      string body;
      using (var response = await this.client.SendAsync(request))
      {
         body = await response.Content.ReadAsStringAsync();
         if (response.IsSuccessStatusCode == false)
         {
            this.logger.LogError($"Error calling TwelveData TwelveDataSymbolDetailsService API. Status code: {response.StatusCode}. Body: {body}");
            throw new Exception($"Error calling TwelveData TwelveDataSymbolDetailsService API. Status code: {response.StatusCode}. Body: {body}");
         }
      }

      return body;
   }

   private class SymbolDetailsModelContainer
   {
      [JsonProperty("status")]
      public string Status { get; set; }
      
      [JsonProperty("data")] 
      public List<SymbolDetailsModel> SymbolsDetails { get; set; }
   }
}