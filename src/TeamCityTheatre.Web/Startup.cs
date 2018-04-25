using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamCityTheatre.Core;
using TeamCityTheatre.Core.Options;

namespace TeamCityTheatre.Web {
  public class Startup {
    public Startup(IConfigurationRoot configuration) {
      Configuration = configuration;
    }

    public IConfigurationRoot Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public IServiceProvider ConfigureServices(IServiceCollection services) {
      services.AddRouting(options => options.LowercaseUrls = true);
      // Add framework services.
      services.AddMvc();

      // Add appsettings
      services.Configure<ConnectionOptions>(Configuration.GetSection("Connection"));
      services.Configure<StorageOptions>(Configuration.GetSection("Storage"));
      
      // Create the AutoFac container builder.
      var builder = new ContainerBuilder();

      // Register dependencies, populate the services from
      // the collection, and build the container. If you want
      // to dispose of the container at the end of the app,
      // be sure to keep a reference to it as a property or field.
      builder.RegisterModule<CoreModule>();

      // Copy entries from prepared IServiceCollection
      builder.Populate(services);

      var applicationContainer = builder.Build();

      // Create the IServiceProvider based on the container.
      return new AutofacServiceProvider(applicationContainer);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
        app.UseBrowserLink();
      } else {
        app.UseExceptionHandler("/Home/Error");
      }

      app.UseStaticFiles();

      app.UseMvc(routes => {
        routes.MapRoute(
          name: "action",
          template: "{action=Index}",
          defaults: new { controller = "Home" });
        routes.MapRoute(
          name: "controller+action",
          template: "{controller}/{action}/{id?}");
      });
    }
  }
}