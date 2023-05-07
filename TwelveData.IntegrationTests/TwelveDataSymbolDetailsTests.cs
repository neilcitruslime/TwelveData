namespace TwelveData.IntegrationTests;

using Microsoft.Extensions.Logging.Abstractions;
using Services.Classes;
using Services.Models;
using Services.Services;

[TestFixture]
public class TwelveDataSymbolDetailsTests : TestBase
{
   private string apiKey;
   private TwelveDataSymbolDetailsService twelveDataSymbolDetailsService;

   [SetUp]
   public void Setup()
   {
      this.apiKey = InitConfiguration()["TwelveDataApiKey"];
      this.twelveDataSymbolDetailsService = new TwelveDataSymbolDetailsService(new NullLogger<TwelveDataSymbolDetailsService>(), new RetryManager(new NullLogger<RetryManager>()), new HttpClient());
   }

   [Test]
   public void Apple()
   {
      List<SymbolDetailsModel> symbols = this.twelveDataSymbolDetailsService.GetSymbolDetails(this.apiKey, "AAPL", "stocks", "").GetAwaiter().GetResult();
      
      Assert.That(symbols.Count, Is.EqualTo(1));
      Assert.That(symbols[0].Symbol, Is.EqualTo("AAPL"));
      Assert.That(symbols[0].Name, Is.EqualTo("Apple Inc"));
      Assert.That(symbols[0].Exchange, Is.EqualTo("NASDAQ"));
      Assert.That(symbols[0].Country, Is.EqualTo("United States"));
      Assert.That(symbols[0].Currency, Is.EqualTo("USD"));
      Assert.That(symbols[0].Type, Is.EqualTo("Common Stock"));
   }


   [Test]
   public void InvalidSymbol()
   {
      List<SymbolDetailsModel> symbols = this.twelveDataSymbolDetailsService.GetSymbolDetails(this.apiKey, "NOTASYMBOL", "stocks", "").GetAwaiter().GetResult();
      Assert.That(symbols.Count, Is.EqualTo(0));
   }

   [Test]
   public void GetUkTicker()
   {
      List<SymbolDetailsModel> symbols = this.twelveDataSymbolDetailsService.GetSymbolDetails(this.apiKey, "AV", "stocks", "LSE").GetAwaiter().GetResult();
      Assert.That(symbols.Count, Is.EqualTo(1));
      Assert.That(symbols[0].Symbol, Is.EqualTo("AV"));
      Assert.That(symbols[0].Name, Is.EqualTo("Aviva plc"));
      Assert.That(symbols[0].Exchange, Is.EqualTo("LSE"));
      Assert.That(symbols[0].Country, Is.EqualTo("United Kingdom"));
      Assert.That(symbols[0].Currency, Is.EqualTo("GBp"));
      Assert.That(symbols[0].Type, Is.EqualTo("Common Stock"));
   }

   [Test]
   public void GetQqqEtfTicker()
   {
      List<SymbolDetailsModel> symbols = this.twelveDataSymbolDetailsService.GetSymbolDetails(this.apiKey, "QQQ", "etf", "").GetAwaiter().GetResult();
      Assert.That(symbols.Count, Is.EqualTo(1));
      Assert.That(symbols[0].Symbol, Is.EqualTo("QQQ"));
      Assert.That(symbols[0].Name, Is.EqualTo("Invesco QQQ Trust"));
      Assert.That(symbols[0].Exchange, Is.EqualTo("NASDAQ"));
      Assert.That(symbols[0].Country, Is.EqualTo("United States"));
      Assert.That(symbols[0].Currency, Is.EqualTo("USD"));
      Assert.That(symbols[0].Type, Is.EqualTo("ETF"));
   }

   [Test]
   public void GetIsfEtfTicker()
   {
      List<SymbolDetailsModel> symbols = this.twelveDataSymbolDetailsService.GetSymbolDetails(this.apiKey, "ISF", "etf", "LSE").GetAwaiter().GetResult();
      Assert.That(symbols.Count, Is.EqualTo(1));
      Assert.That(symbols[0].Symbol, Is.EqualTo("ISF"));
      Assert.That(symbols[0].Name.Trim(), Is.EqualTo("iShares Core FTSE 100 UCITS ETF GBP"));
      Assert.That(symbols[0].Exchange, Is.EqualTo("LSE"));
      Assert.That(symbols[0].Country, Is.EqualTo("United Kingdom"));
      Assert.That(symbols[0].Currency, Is.EqualTo("GBp"));
      Assert.That(symbols[0].Type, Is.EqualTo("ETF"));
   }
}