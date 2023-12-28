using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Retryrr.Application;

public sealed class RetryrrService : BackgroundService, IRetryrrService
{
   private readonly ILogger<RetryrrService> _logger;

   public RetryrrService(ILogger<RetryrrService> logger, IHostApplicationLifetime applicationLifetime)
   {
      this._logger = logger;

      // Start & stop messages
      applicationLifetime.ApplicationStarted.Register(() =>
         this._logger.LogInformation("Retryrr Service started."));
      applicationLifetime.ApplicationStopped.Register(() =>
         this._logger.LogInformation("Retryrr Service stopping."));
   }

   protected override async Task ExecuteAsync(CancellationToken cancellationToken)
   {
      await DoSomething(cancellationToken);
   }

   public async Task DoSomething(CancellationToken cancellationToken)
   {
      while (!cancellationToken.IsCancellationRequested)
      {
         await Task.Delay(5000);
         this._logger.LogInformation("Doing stuff.");
      }
   }

   public bool RegisterHandler(IRequestHandler handler)
   {
      throw new NotImplementedException();
   }
}