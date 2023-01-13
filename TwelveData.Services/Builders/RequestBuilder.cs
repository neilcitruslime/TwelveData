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
      private string uri = "https://api.twelvedata.com/{0}?apikey={1}&interval={2}&symbol={3}";

      private string startDate = "&start_date=2001-01-01 00:00:00";
      private string lastNRecords = "&outputsize=30";

      public Uri BuildRequestTimeServiesUri(string queryType,
         string apiKey,
         string interval,
         string symbol,
         EnumDataSize enumDataSize)
      {
         string url = string.Format(this.uri, queryType, apiKey, interval, symbol);
         switch (enumDataSize)
         {
            case EnumDataSize.Compact:
               url += lastNRecords;
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
   }
}