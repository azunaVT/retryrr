using Microsoft.Extensions.Hosting;

namespace Retryrr.Infra;

public static class DependencyInjection
{
   public static IHostApplicationBuilder ConfigureInfra(this IHostApplicationBuilder builder)
   {
      // Do something with the builder's services and configuration
      var services = builder.Services;
      var config = builder.Configuration;

      return builder;
   }
}