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
  public interface IProjectDataService {
    Task<IEnumerable<IBasicProject>> GetAllProjectsAsync();
    Task<IDetailedProject> GetProjectDetailsAsync(IProjectLocator projectLocator);
  }

  public class ProjectDataService : IProjectDataService {
    readonly IProjectMapper _projectMapper;
    readonly ITeamCityClient _teamCityClient;

    public ProjectDataService(ITeamCityClient teamCityClient, IProjectMapper projectMapper) {
      _teamCityClient = teamCityClient ?? throw new ArgumentNullException(nameof(teamCityClient));
      _projectMapper = projectMapper ?? throw new ArgumentNullException(nameof(projectMapper));
    }

    public async Task<IEnumerable<IBasicProject>> GetAllProjectsAsync() {
      var request = new RestRequest("projects", Method.GET);
      var response = await _teamCityClient.ExecuteRequestAsync<ProjectsResponse>(request);
      return _projectMapper.Map(response);
    }

    public async Task<IDetailedProject> GetProjectDetailsAsync(IProjectLocator projectLocator) {
      var request = new RestRequest("projects/{locator}", Method.GET);
      request.AddUrlSegment("locator", projectLocator.Serialize());
      var response = await _teamCityClient.ExecuteRequestAsync<ProjectResponse>(request);
      return _projectMapper.Map(response);
    }
  }
}