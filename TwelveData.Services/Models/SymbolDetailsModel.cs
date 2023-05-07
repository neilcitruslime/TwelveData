namespace TwelveData.Services.Models;

using Newtonsoft.Json;

public class SymbolDetailsModel
{
   [JsonProperty("symbol")] 
   public string Symbol { get; set; }
   [JsonProperty("name")] 
   public string Name { get; set; }
   [JsonProperty("currency")] 
   public string Currency { get; set; }
   [JsonProperty("exchange")] 
   public string Exchange { get; set; }
   [JsonProperty("mic_code")] 
   public string MicCode { get; set; }
   [JsonProperty("country")] 
   public string Country { get; set; }
   [JsonProperty("type")] 
   public string Type { get; set; }
}