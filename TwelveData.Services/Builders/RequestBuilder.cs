using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwelveData.Services.Enums;

namespace TwelveData.Services.Builders
{
   public class RequestBuilder
   {
      private string uri = "https://api.twelvedata.com/{0}?apikey={1}&interval={2}&symbol={3}&exchange={4}&previous_close=true";
      private string symbolsUri = "https://api.twelvedata.com/{0}?apikey={1}&&symbol={2}&exchange={3}{4}";

      private string startDate = "&start_date=2001-01-01 00:00:00";
      private string lastNRecords = "&outputsize={0}";

      public Uri BuildRequestTimeServiesUri(string queryType,
         string apiKey,
         string interval,
         string symbol, 
         EnumDataSize enumDataSize,
         string exchange)
      {
         string url = string.Format(this.uri,
            queryType,
            apiKey,
            interval,
            symbol,
            exchange ?? string.Empty);
         switch (enumDataSize)
         {
            case EnumDataSize.Compact:
               url += string.Format(lastNRecords, "30");
               break;
            case EnumDataSize.Quote:
               url += string.Format(lastNRecords, "1");
               break;
            case EnumDataSize.Full:
               url += startDate;
               break;
            default:
               throw new ArgumentOutOfRangeException(
                  $"{nameof(enumDataSize)} is {enumDataSize.ToString()} which is not supported");
         }

         return new Uri(url);
      }
      
      public Uri BuildRequestSymbolDetailsUri(string queryType,
         string apiKey,
         string symbol,
         string exchange)
      {
         string countryQuery = string.IsNullOrEmpty(exchange) ? "&country=United%20States" : string.Empty;
         
         string url = string.Format(this.symbolsUri,
            queryType,
            apiKey,
            symbol,
            exchange ?? string.Empty, 
            countryQuery);
         return new Uri(url);
      }
   }
}