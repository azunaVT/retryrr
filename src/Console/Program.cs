using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Retryrr.Application;
using Retryrr.Infra;

var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
   {
      ApplicationName = "Retryrr",
   });

// Add configuration from environment variables
builder.Configuration.AddEnvironmentVariables(prefix: "RETRYRR_");

// Configure the app
builder.ConfigureInfra();
builder.ConfigureApplication();

// Build and run
var host = builder.Build();
await host.RunAsync();