using System;
using System.Collections.Generic;
using System.Linq;
using TeamCityTheatre.Core.Client.Responses;
using TeamCityTheatre.Core.Models;

namespace TeamCityTheatre.Core.Client.Mappers {
  public interface IArtifactDependencyMapper {
    ArtifactDependency Map(ArtifactDependencyResponse artifactDependency);
    IReadOnlyCollection<ArtifactDependency> Map(ArtifactDependenciesResponse artifactDependencies);
  }

  public class ArtifactDependencyMapper : IArtifactDependencyMapper {
    readonly IPropertyMapper _propertyMapper;

    public ArtifactDependencyMapper(IPropertyMapper propertyMapper) {
      _propertyMapper = propertyMapper ?? throw new ArgumentNullException("propertyMapper");
    }

    public ArtifactDependency Map(ArtifactDependencyResponse artifactDependency) {
      if (artifactDependency == null)
        return null;
      return new ArtifactDependency {
        Id = artifactDependency.Id,
        Type = artifactDependency.Type,
        Properties = _propertyMapper.Map(artifactDependency.Properties)
      };
    }

    public IReadOnlyCollection<ArtifactDependency> Map(ArtifactDependenciesResponse artifactDependencies) {
      if (artifactDependencies == null || artifactDependencies.ArtifactDependency == null)
        return new List<ArtifactDependency>();
      return artifactDependencies.ArtifactDependency.Select(Map).ToList();
    }
  }
}