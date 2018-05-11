using System.Collections.Generic;
using System.Linq;
using TeamCityTheatre.Core.Client.Responses;
using TeamCityTheatre.Core.Models;

namespace TeamCityTheatre.Core.Client.Mappers {  
  public interface IBuildChangeMapper {
    BuildChange Map(BuildChangeResponse buildChange);
    IReadOnlyCollection<BuildChange> Map(BuildChangesResponse buildChanges);
  }

  public class BuildChangeMapper : IBuildChangeMapper {
    public BuildChange Map(BuildChangeResponse buildChange) {
      if (buildChange == null)
        return null;
      return new BuildChange {
        Date = buildChange.Date,
        Href = buildChange.Href,
        Id = buildChange.Id,
        Username = buildChange.Username,
        Version = buildChange.Version,
        WebLink = buildChange.WebLink
      };
    }

    public IReadOnlyCollection<BuildChange> Map(BuildChangesResponse buildChanges) {
      if (buildChanges == null || buildChanges.BuildChange == null)
        return new List<BuildChange>();
      return buildChanges.BuildChange.Select(Map).ToList();
    }
  }
}