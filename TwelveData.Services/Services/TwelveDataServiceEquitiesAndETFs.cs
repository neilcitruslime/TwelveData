// <copyright file="TwelveDataServiceEquitiesAndETFs.cs" company="CitrusLime Ltd">
// Copyright (c) CitrusLime Ltd. All rights reserved.
// </copyright>

namespace TwelveData.Services.Services;

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TwelveData.Services.Builders;
using TwelveData.Services.Interfaces;
using TwelveData.Services.Models;
using TwelveData.Services.Services.Abstract;

public class TwelveDataServiceEquitiesAndETFs : AbstractTwelveDataService
{
    private readonly ILogger<TwelveDataServiceEquitiesAndETFs> logger;
    private readonly IRetryManager retryManager;
    private readonly HttpClient client;
    private static readonly  int maxRetryAttempts = 10;
    private static readonly TimeSpan pauseBetweenFailures = TimeSpan.FromSeconds(60);

    public TwelveDataServiceEquitiesAndETFs(
        ILogger<TwelveDataServiceEquitiesAndETFs> logger,
            IRetryManager retryManager, 
            HttpClient client)
    {
        this.logger = logger;
        this.retryManager = retryManager;
        this.client = client;
    }

    public async Task<List<SymbolDetailsModel>> GetEquities(string apiKey, string exchange, string country, string type)
    {
        return await this.Get(apiKey, exchange, country, type, "stocks");
    } 
    
    public async Task<List<SymbolDetailsModel>> GetEtfs(string apiKey, string exchange, string country)
    {
        return await this.Get(apiKey, exchange, country, string.Empty, "etf");
    } 
        
    private async Task<List<SymbolDetailsModel>> Get(string apiKey, string exchange, string country, string type, string queryType)
    {
        string body = string.Empty;

        await this.retryManager.RetryOnExceptionAsync(maxRetryAttempts, pauseBetweenFailures, async () =>
        {
            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://api.twelvedata.com/{queryType}?apikey={apiKey}&exchange={exchange}&country={country}&type={type}"),
            };
            body = await this.MakeApiCall(this.client, request, string.Empty);
        });

        return JsonConvert.DeserializeObject<DataModel<List<SymbolDetailsModel>>>(body).Data;
    }
}