using System;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using Serilog;

namespace TeamCityTheatre.Core.Client {
  public class LoggingTeamCityClient<T> : ITeamCityClient where  T: class, ITeamCityClient {
    readonly T _inner;
    readonly ILogger _logger;

    public LoggingTeamCityClient(ILogger logger, T inner) {
      _logger = logger.ForContext<ITeamCityClient>() ?? throw new ArgumentNullException(nameof(logger));
      _inner = inner ?? throw new ArgumentNullException(nameof(inner));
    }

    public async Task<TResponse> ExecuteRequestAsync<TResponse>(IRestRequest request) where TResponse : new() {
      var parameters = string.Join(", ", request.Parameters.Select(p => $"{p.Name} = {p.Value}"));
      _logger.Information("[REQUEST ] {Method} {Url} with parameters {Parameters}", request.Method, request.Resource, parameters);
      var response = await _inner.ExecuteRequestAsync<TResponse>(request);
      _logger.Information("[RESPONSE] {Data}", response);
      return response;
    }
  }
}