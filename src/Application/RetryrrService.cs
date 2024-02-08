using System.Net;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Retryrr.Application.Utils;

namespace Retryrr.Application;

public sealed class RetryrrService : BackgroundService
{
   private readonly ILogger<RetryrrService> _logger;

   private readonly IHostApplicationLifetime _applicationLifetime;

   private IEnumerable<IRequestHandler> _handlers;
   private HttpListener _listener;

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
         this._logger.LogInformation("Retryrr Service stopped."));
      this._applicationLifetime = applicationLifetime;
   }

   public override Task StartAsync(CancellationToken cancellationToken)
   {
      this._logger.LogInformation("Retryrr Service is starting...");
      this._listener = new HttpListener();
      this._listener.Prefixes.Add("http://*:8888/");
      
      // Change the handlers to a queue
      this._handlers = this._handlers.ToQueue();
      return base.StartAsync(cancellationToken);
   }

   public override Task StopAsync(CancellationToken cancellationToken)
   {
      this._logger.LogInformation("Retryrr Service is stopping...");
      return base.StopAsync(cancellationToken);
   }

   protected override async Task ExecuteAsync(CancellationToken cancellationToken)
   {
      try
      {
         this._listener.Start();
         this._logger.LogInformation("Listening on port 8888...");

         while (!cancellationToken.IsCancellationRequested)
         {
            this._listener
               .BeginGetContext(this.HandleRequest, this._listener)
               .AsyncWaitHandle
               .WaitOne();
         }
      }
      catch (HttpListenerException ex)
      {
         this._logger.LogError(ex.ToString());
         this._applicationLifetime.StopApplication();
      }
      finally
      {
         this._listener.Close();
      }
   }

   public void HandleRequest(IAsyncResult result)
   {
      if (!this._listener.IsListening)
      {
         this._logger.LogWarning("Will not process request, listener is not listening.");
         return;
      }

      var context = this._listener.EndGetContext(result);
      this._listener.BeginGetContext(this.HandleRequest);
      var url = context.Request.Url?.AbsolutePath;

      this._logger.LogInformation($"Handling request : {url}");
      
      context.Response.OutputStream.Write("Ayyyy!"u8);
      context.Response.OutputStream.Close();
   }
}