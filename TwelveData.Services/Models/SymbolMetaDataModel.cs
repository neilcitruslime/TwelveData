using System.Text.Json;
using Newtonsoft.Json;

namespace TwelveData.Services.Models
{
   public partial class SymbolMetaDataModel
   {
      [JsonProperty("symbol")] public string Symbol { get; set; }

      [JsonProperty("interval")] public string Interval { get; set; }

      [JsonProperty("currency")] public string Currency { get; set; }

      [JsonProperty("exchange_timezone")] public string ExchangeTimezone { get; set; }

      [JsonProperty("exchange")] public string Exchange { get; set; }

      [JsonProperty("mic_code")] public string MicCode { get; set; }

      [JsonProperty("type")] public string Type { get; set; }
   }
}