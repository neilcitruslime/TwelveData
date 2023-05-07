namespace TwelveData.Services.Interfaces;

public interface IRetryManager
{
   Task RetryOnExceptionAsync(
      int times, TimeSpan delay, Func<Task> operation);
}