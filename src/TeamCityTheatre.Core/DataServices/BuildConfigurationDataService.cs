using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using TeamCityTheatre.Core.Client;
using TeamCityTheatre.Core.Client.Mappers;
using TeamCityTheatre.Core.Client.Responses;
using TeamCityTheatre.Core.DataServices.Locators;
using TeamCityTheatre.Core.Models;

namespace TeamCityTheatre.Core.DataServices {
  public interface IBuildConfigurationDataService {
    Task<IEnumerable<IBasicBuildConfiguration>> GetAllBuildConfigurationsAsync();
    Task<IEnumerable<IBasicBuildConfiguration>> GetBuildConfigurationsAsync(IBuildConfigurationLocator buildConfigurationLocator);
    Task<IDetailedBuildConfiguration> GetBuildConfigurationDetailsAsync(IBuildConfigurationLocator buildConfigurationLocator);
  }

  public class BuildConfigurationDataService : IBuildConfigurationDataService {
    readonly ITeamCityClient _teamCityClient;
    readonly IBuildConfigurationMapper _buildConfigurationMapper;

    public BuildConfigurationDataService(ITeamCityClient teamCityClient, IBuildConfigurationMapper buildConfigurationMapper) {
      _teamCityClient = teamCityClient ?? throw new ArgumentNullException(nameof(teamCityClient));
      _buildConfigurationMapper = buildConfigurationMapper ?? throw new ArgumentNullException(nameof(buildConfigurationMapper));
    }

    public async Task<IEnumerable<IBasicBuildConfiguration>> GetAllBuildConfigurationsAsync() {
      var request = new RestRequest("buildTypes");
      var response = await _teamCityClient.ExecuteRequestAsync<BuildTypesResponse>(request);
      return _buildConfigurationMapper.Map(response);
    }

    public async Task<IEnumerable<IBasicBuildConfiguration>> GetBuildConfigurationsAsync(IBuildConfigurationLocator buildConfigurationLocator) {
      var request = new RestRequest("buildTypes/{locator}");
      request.AddUrlSegment("locator", buildConfigurationLocator.Serialize());
      var response = await _teamCityClient.ExecuteRequestAsync<BuildTypesResponse>(request);
      return _buildConfigurationMapper.Map(response);
    }

    public async Task<IDetailedBuildConfiguration> GetBuildConfigurationDetailsAsync(IBuildConfigurationLocator buildConfigurationLocator) {
      var request = new RestRequest("buildTypes/{locator}");
      request.AddUrlSegment("locator", buildConfigurationLocator.Serialize());
      var response = await _teamCityClient.ExecuteRequestAsync<BuildTypeResponse>(request);
      return _buildConfigurationMapper.Map(response);
    }
  }
}