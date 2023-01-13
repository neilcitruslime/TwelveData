# TwelveData
C# Project to get data from the TwelveDataApi currently supports only TimeSeries data call. 

new TwelveDataService().GetTimeSeries(apiKey, "APPL", EnumDataSize.Compact)) // Last 30 data points
new TwelveDataService().GetTimeSeries(apiKey, "APPL", EnumDataSize.Full)) // All data points


