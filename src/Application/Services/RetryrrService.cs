using System.Net;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Retryrr.Application;

public sealed class RetryrrService : BackgroundService, IRetryrrService
{
   private readonly ILogger<RetryrrService> _logger;

   private readonly IHostApplicationLifetime _applicationLifetime;

   private IEnumerable<IRequestHandler> _handlers;

   public RetryrrService(
      IEnumerable<IRequestHandler> handlers,
      ILogger<RetryrrService> logger,
      IHostApplicationLifetime applicationLifetime)
   {
      this._logger = logger;
      this._handlers = handlers;

      // Start & stop messages
      applicationLifetime.ApplicationStarted.Register(() =>
         this._logger.LogInformation("Retryrr Service started."));
      applicationLifetime.ApplicationStopped.Register(() =>
         this._logger.LogInformation("Retryrr Service stopping."));
      this._applicationLifetime = applicationLifetime;
   }

   public override Task StartAsync(CancellationToken cancellationToken)
   {
      // Change the handlers to a queue
      this._handlers = this._handlers.ToQueue();
      return base.StartAsync(cancellationToken);
   }

   protected override async Task ExecuteAsync(CancellationToken cancellationToken)
   {
      await Task.Run(() => DoSomething(cancellationToken));
   }

   public void DoSomething(CancellationToken cancellationToken)
   {
      var listener = new HttpListener();
      listener.Prefixes.Add("http://*:8888/");

      try
      {
         listener.Start();
         listener.BeginGetContext(new AsyncCallback(this.HandleRequest), listener);
         cancellationToken.WaitHandle.WaitOne();
      }
      catch (HttpListenerException ex)
      {
         this._logger.LogError(ex.ToString());
         this._applicationLifetime.StopApplication();
      }
      finally
      {
         listener.Close();
      }
   }

   public void HandleRequest(IAsyncResult result)
   {
      if (result.AsyncState is not HttpListener listener)
      {
         _logger.LogError("Could not handle request, AsyncState is invalid.");
         return;
      }

      var context = listener.EndGetContext(result);

      if (context is null)
      {
         this._logger.LogError("Invalid context for request.");
         return;
      }

      var url = context.Request?.Url?.AbsolutePath.ToString();

      _logger.LogInformation($"Handling request : {url}");

      context.Response.OutputStream.Write(Encoding.UTF8.GetBytes("Ayyyy!"));
      context.Response.OutputStream.Close();
   }
}