using TeamCityTheatre.Core.Models;

namespace TeamCityTheatre.Core.Client.Mappers {
  public interface IBuildStatusMapper {
    BuildStatus Map(string buildStatus);
  }

  public class BuildStatusMapper : IBuildStatusMapper {
    public BuildStatus Map(string buildStatus) {
      switch (buildStatus) {
        case "SUCCESS": return BuildStatus.Success;
        case "FAILURE": return BuildStatus.Failure;
        case "ERROR": return BuildStatus.Error;
        default: return BuildStatus.Unknown;
      }
    }
  }
}