using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TwelveData.Services.Models
{
   public partial class QueryResultsModel
   {
      [JsonProperty("meta")] public SymbolMetaDataModel Meta { get; set; }

      [JsonProperty("values")] public StockValueModel[] Values { get; set; }

      public static QueryResultsModel FromJson(string json) =>
         JsonConvert.DeserializeObject<QueryResultsModel>(json, Converter.Settings);
   }

   //public static class Serialize
   //{
   //    public static string ToJson(this QueryResultsModel self) => JsonConvert.SerializeObject(self, Converter.Settings);
   //}

   internal static class Converter
   {
      public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
      {
         MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
         DateParseHandling = DateParseHandling.None,
         Converters =
         {
            new IsoDateTimeConverter {DateTimeStyles = DateTimeStyles.AssumeUniversal}
         },
      };
   }

   internal class ParseStringConverter : Newtonsoft.Json.JsonConverter
   {
      public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

      public override object ReadJson(JsonReader reader, Type t, object existingValue,
         Newtonsoft.Json.JsonSerializer serializer)
      {
         if (reader.TokenType == JsonToken.Null) return null;
         string value = serializer.Deserialize<string>(reader);
         long l;
         if (Int64.TryParse(value, out l))
         {
            return l;
         }

         throw new Exception("Cannot unmarshal type long");
      }

      public override void WriteJson(JsonWriter writer, object untypedValue, Newtonsoft.Json.JsonSerializer serializer)
      {
         if (untypedValue == null)
         {
            serializer.Serialize(writer, null);
            return;
         }

         long value = (long) untypedValue;
         serializer.Serialize(writer, value.ToString());
         return;
      }

      public static readonly ParseStringConverter Singleton = new ParseStringConverter();
   }
}