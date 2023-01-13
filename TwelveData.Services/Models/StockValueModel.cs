using System.Text.Json;
using Newtonsoft.Json;

namespace TwelveData.Services.Models
{
   public partial class StockValueModel
   {
      [JsonProperty("datetime")] public DateTime Datetime { get; set; }

      [JsonProperty("open")] public decimal Open { get; set; }

      [JsonProperty("high")] public decimal High { get; set; }

      [JsonProperty("low")] public decimal Low { get; set; }

      [JsonProperty("close")] public decimal Close { get; set; }

      [JsonProperty("volume")]
      [Newtonsoft.Json.JsonConverter(typeof(ParseStringConverter))]
      public long Volume { get; set; }
   }
}