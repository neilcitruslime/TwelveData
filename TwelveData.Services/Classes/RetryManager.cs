namespace TwelveData.Services.Classes;

using Interfaces;
using Microsoft.Extensions.Logging;
public class RetryManager : IRetryManager
{
   private readonly ILogger<RetryManager> logger;

   public RetryManager(ILogger<RetryManager> logger)
   {
      this.logger = logger;
   }
   
   public async Task RetryOnExceptionAsync(
      int times, TimeSpan delay, Func<Task> operation)
   {
      await RetryOnExceptionAsync<Exception>(times, delay, operation);
   }

   private async Task RetryOnExceptionAsync<TException>(
      int times, TimeSpan delay, Func<Task> operation) where TException : Exception
   {
      if (times <= 0)
         throw new ArgumentOutOfRangeException(nameof(times));

      var attempts = 0;
      do
      {
         try
         {
            attempts++;
            await operation();
            break;
         }
         catch (TException ex)
         {
            if (attempts == times || ex.Message.Contains("Rate Limiting Hit") == false)
               throw;

            await CreateDelayForException(times, attempts, delay, ex);
         }
      } while (true);
   }

   private Task CreateDelayForException(
      int times, int attempts, TimeSpan delay, Exception ex)
   {
      this.logger.LogWarning($"Exception on attempt {attempts} of {times}. " +
                             "Will retry after sleeping for {delay}.", ex);
      return Task.Delay(delay);
   }
}