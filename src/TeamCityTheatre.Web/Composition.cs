using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamCityTheatre.Core.Client;
using TeamCityTheatre.Core.Client.Mappers;
using TeamCityTheatre.Core.DataServices;
using TeamCityTheatre.Core.Options;
using TeamCityTheatre.Core.QueryServices;
using TeamCityTheatre.Core.Repositories;

namespace TeamCityTheatre.Web {
  public static class Composition {

    public static IServiceCollection AddViewManagement(this IServiceCollection services, IConfiguration configuration) {
      services.Configure<StorageOptions>(configuration.GetSection("Storage"));

      services.AddSingleton<IConfigurationRepository, ConfigurationRepository>();

      services.AddSingleton<ITileDataService, TileDataService>();
      services.AddSingleton<IViewDataService, ViewDataService>();

      services.AddSingleton<ITileService, TileService>();
      services.AddSingleton<IViewService, ViewService>();

      return services;
    }

    public static IServiceCollection AddTeamCityServices(this IServiceCollection services, IConfiguration configuration) {
      /* stuff necessary to call TeamCity REST API */
      services.Configure<ConnectionOptions>(configuration.GetSection("Connection"));
      services.Configure<ApiOptions>(configuration.GetSection("Api"));
      services.AddSingleton<ITeamCityRestClientFactory, TeamCityRestClientFactory>();
      services.AddSingleton<IResponseValidator, ResponseValidator>();
      services.AddSingleton<ITeamCityRequestPreparer, TeamCityRequestPreparer>();
      services.AddSingleton<TeamCityClient, TeamCityClient>();
      services.AddSingleton<ITeamCityClient, LoggingTeamCityClient<TeamCityClient>>();

      /* mappers that translate TeamCity response objects to our own models */ 
      services.AddSingleton<IAgentMapper, AgentMapper>();
      services.AddSingleton<IAgentRequirementMapper, AgentRequirementMapper>();
      services.AddSingleton<IArtifactDependencyMapper, ArtifactDependencyMapper>();
      services.AddSingleton<IBuildChangeMapper, BuildChangeMapper>();
      services.AddSingleton<IBuildConfigurationMapper, BuildConfigurationMapper>();
      services.AddSingleton<IBuildMapper, BuildMapper>();
      services.AddSingleton<IBuildStatusMapper, BuildStatusMapper>();
      services.AddSingleton<IBuildStepMapper, BuildStepMapper>();
      services.AddSingleton<IBuildTriggerMapper, BuildTriggerMapper>();
      services.AddSingleton<IProjectMapper, ProjectMapper>();
      services.AddSingleton<IPropertyMapper, PropertyMapper>();
      services.AddSingleton<ISnapshotDependencyMapper, SnapshotDependencyMapper>();
      services.AddSingleton<IVcsRootEntryMapper, VcsRootEntryMapper>();
      services.AddSingleton<IVcsRootMapper, VcsRootMapper>();

      services.AddSingleton(sp => new Lazy<IProjectMapper>(sp.GetRequiredService<IProjectMapper>));
      services.AddSingleton(sp => new Lazy<IBuildConfigurationMapper>(sp.GetRequiredService<IBuildConfigurationMapper>));

      /* data services which call the TeamCity REST client and the corresponding response mappers */ 
      services.AddSingleton<IBuildConfigurationDataService, BuildConfigurationDataService>();
      services.AddSingleton<IBuildDataService, BuildDataService>();
      services.AddSingleton<IProjectDataService, ProjectDataService>();
      
      return services;
    }



  }
}