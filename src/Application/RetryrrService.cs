using System.Net;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Retryrr.Application.Interfaces;
using Retryrr.Application.Utils;

namespace Retryrr.Application;

public sealed class RetryrrService : BackgroundService
{
   private const int Port = 8888;
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
      this._applicationLifetime = applicationLifetime;

      // Start & stop messages
      this._applicationLifetime.ApplicationStarted.Register(() =>
         this._logger.LogInformation("Retryrr Service started."));
      this._applicationLifetime.ApplicationStopping.Register(() =>
      {
         this._logger.LogInformation("Retryrr Service stopped.");
      });
   }

   protected override async Task ExecuteAsync(CancellationToken cancellationToken)
   {
      this.Listen();
      
      while (!cancellationToken.IsCancellationRequested)
         await this.ProcessRequests(cancellationToken);
   }

   private void Listen()
   {
      this._logger.LogInformation("Retryrr Service is starting...");
      this._listener = new HttpListener();
      this._listener.Prefixes.Add($"http://+:{Port}/");
      this._listener.Start();
      this._logger.LogInformation($"Listening on port {Port}...");
   }
   
   private async Task ProcessRequests(CancellationToken cancellationToken) => 
      await this._listener.BeginGetContextAsync(this.HandleRequest, cancellationToken);

   private void HandleRequest(IAsyncResult result)
   {
      if (!this._listener.IsListening)
      {
         this._logger.LogWarning("Will not process request, listener is not listening.");
         return;
      }

      var context = this._listener.EndGetContext(result);

      // Some test stuff with the context.
      var url = context.Request.Url?.AbsolutePath;
      this._logger.LogInformation($"Handling request : {url}");
      context.Response.OutputStream.Write("Request Handled!"u8);
      context.Response.OutputStream.Close();
   }
}