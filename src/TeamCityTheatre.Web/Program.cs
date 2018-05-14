using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace TeamCityTheatre.Web {
  public class Program {
    public static void Main(string[] args) {
      var environment = Environment();
      var configuration = BuildConfiguration(args, environment);
      var logger = BuildLogger(configuration);

      BuildWebHost(args, configuration, logger).Run();
    }

    public static IWebHost BuildWebHost(string[] args, IConfiguration configuration, ILogger logger) {
      return new WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(Directory.GetCurrentDirectory())
        .UseConfiguration(configuration)
        .UseIISIntegration()
        .CaptureStartupErrors(true)
        .UseSetting(WebHostDefaults.DetailedErrorsKey, "True")
        .UseDefaultServiceProvider((context, options) => options.ValidateScopes = context.HostingEnvironment.IsDevelopment())
        .ConfigureServices(sc => sc.AddSingleton(logger))
        .UseStartup<Startup>()
        .UseSerilog(logger, dispose: true)
        .Build();
    }

    static ILogger BuildLogger(IConfiguration configuration) => new LoggerConfiguration()
      .ReadFrom.Configuration(configuration)
      .CreateLogger();

    static IConfigurationRoot BuildConfiguration(string[] args, string environment) {
      var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory());

      config.AddJsonFile("appsettings.json", false, true);

      config.AddEnvironmentVariables();

      if (string.Equals(environment, EnvironmentName.Development, StringComparison.OrdinalIgnoreCase)) {
        config.AddUserSecrets<Startup>();
      }

      if (args != null) {
        config.AddCommandLine(args);
      }

      return config.Build();
    }

    static string Environment() => System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                                   ?? EnvironmentName.Production;
  }
}