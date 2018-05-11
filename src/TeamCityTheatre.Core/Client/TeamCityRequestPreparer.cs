using RestSharp;

namespace TeamCityTheatre.Core.Client {
  public interface ITeamCityRequestPreparer {
    void Prepare(IRestRequest request);
  }

  public class TeamCityRequestPreparer : ITeamCityRequestPreparer {
    public void Prepare(IRestRequest request) {
      request.DateFormat = "yyyyMMdd'T'HHmmsszzz";
    }
  }
}