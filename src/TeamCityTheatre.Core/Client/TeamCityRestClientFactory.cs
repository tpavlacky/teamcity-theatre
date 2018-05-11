using System;
using RestSharp;
using RestSharp.Authenticators;
using TeamCityTheatre.Core.Options;

namespace TeamCityTheatre.Core.Client {
  public interface ITeamCityRestClientFactory {
    IRestClient Create(ConnectionOptions connectionOptions);
  }

  public class TeamCityRestClientFactory : ITeamCityRestClientFactory {
    public IRestClient Create(ConnectionOptions connectionOptions) {
      if (connectionOptions == null) throw new ArgumentNullException(nameof(connectionOptions));
      return new RestClient {
        BaseUrl = new Uri(new Uri(connectionOptions.Url), new Uri("httpAuth/app/rest", UriKind.Relative)),
        Authenticator = new HttpBasicAuthenticator(connectionOptions.Username, connectionOptions.Password),
        DefaultParameters = {
          new Parameter {Type = ParameterType.HttpHeader, Name = "Accept", Value = "application/json"}
        }
      };
    }
  }
}