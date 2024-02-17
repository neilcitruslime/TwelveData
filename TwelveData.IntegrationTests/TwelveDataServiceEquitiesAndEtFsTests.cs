// <copyright file="TwelveDataServiceEquitiesAndEtFsTests.cs" company="CitrusLime Ltd">
// Copyright (c) CitrusLime Ltd. All rights reserved.
// </copyright>

namespace TwelveData.IntegrationTests;

using Microsoft.Extensions.Logging.Abstractions;
using TwelveData.Services.Classes;
using TwelveData.Services.Models;
using TwelveData.Services.Services;

public class TwelveDataServiceEquitiesAndEtFsTests : TestBase
{
    private string apiKey;
    private TwelveDataServiceEquitiesAndETFs twelveDataServiceEquitiesAndEtFs;

    [SetUp]
    public void Setup()
    {
        this.apiKey = InitConfiguration()["TwelveDataApiKey"];
        this.twelveDataServiceEquitiesAndEtFs = new TwelveDataServiceEquitiesAndETFs(new NullLogger<TwelveDataServiceEquitiesAndETFs>(), new RetryManager(new NullLogger<RetryManager>()), new HttpClient());
    }
    
    [Test]
    public void GetLseTickers()
    {
        List<SymbolDetailsModel> queryResult = this.twelveDataServiceEquitiesAndEtFs.GetEquities(this.apiKey, "LSE", "UK", "Common Stock").GetAwaiter().GetResult();

        Assert.That(queryResult.Count, Is.GreaterThan(0));
    }
    
    [Test]
    public void GetNasdaqTickers()
    {
        List<SymbolDetailsModel> queryResult = this.twelveDataServiceEquitiesAndEtFs.GetEquities(this.apiKey, "NASDAQ", "US", "Common Stock").GetAwaiter().GetResult();

        Assert.That(queryResult.Count, Is.GreaterThan(0));
    }
    
    [Test]
    public void GetNYSEEtfs()
    {
        List<SymbolDetailsModel> queryResult = this.twelveDataServiceEquitiesAndEtFs.GetEtfs(this.apiKey, "NYSE", "US").GetAwaiter().GetResult();

        Assert.That(queryResult.Count, Is.GreaterThan(0));
    }
    
    [Test]
    public void GetLSEEtfs()
    {
        List<SymbolDetailsModel> queryResult = this.twelveDataServiceEquitiesAndEtFs.GetEtfs(this.apiKey, "LSE", "UK").GetAwaiter().GetResult();

        Assert.That(queryResult.Count, Is.GreaterThan(0));
    }
    
    [Test]
    public void GetNasdaqEtfs()
    {
        List<SymbolDetailsModel> queryResult = this.twelveDataServiceEquitiesAndEtFs.GetEtfs(this.apiKey, "NASDAQ", "US").GetAwaiter().GetResult();

        Assert.That(queryResult.Count, Is.GreaterThan(0));
    }

}