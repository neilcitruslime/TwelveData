// <copyright file="AbstractTwelveDataService.cs" company="CitrusLime Ltd">
// Copyright (c) CitrusLime Ltd. All rights reserved.
// </copyright>

namespace TwelveData.Services.Services.Abstract;

public class AbstractTwelveDataService
{      
    private static readonly string InvalidTickerError = "Cannot find ticker code";

    protected async Task<string> MakeApiCall(HttpClient client, HttpRequestMessage request, string symbol)
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
            
            if (body.Contains("\"code\":404") && body.ToLower().Contains("not found:"))
            {
                throw new Exception($"{InvalidTickerError} '{symbol}'");
            }
        }

        return body;
    }
}