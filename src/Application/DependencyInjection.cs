using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Retryrr.Application;

public static class DependencyInjection
{
   public static IHostApplicationBuilder ConfigureApplication(this IHostApplicationBuilder builder)
   {
      // Do something with the builder's services and configuration
      builder.Services.AddHostedService<RetryrrService>();

      return builder;
   }
}