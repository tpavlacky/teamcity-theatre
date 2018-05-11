using System;
using System.Collections.Generic;
using System.Linq;
using TeamCityTheatre.Core.Client.Responses;
using TeamCityTheatre.Core.Models;

namespace TeamCityTheatre.Core.Client.Mappers {  
  public interface ISnapshotDependencyMapper {
    SnapshotDependency Map(SnapshotDependencyResponse snapshotDependency);
    IReadOnlyCollection<SnapshotDependency> Map(SnapshotDependenciesResponse snapshotDependencies);
  }

  public class SnapshotDependencyMapper : ISnapshotDependencyMapper {
    readonly IPropertyMapper _propertyMapper;

    public SnapshotDependencyMapper(IPropertyMapper propertyMapper) {
      _propertyMapper = propertyMapper ?? throw new ArgumentNullException(nameof(propertyMapper));
    }

    public SnapshotDependency Map(SnapshotDependencyResponse snapshotDependency) {
      if (snapshotDependency == null)
        return null;
      return new SnapshotDependency {
        Id = snapshotDependency.Id,
        Properties = _propertyMapper.Map(snapshotDependency.Properties)
      };
    }

    public IReadOnlyCollection<SnapshotDependency> Map(SnapshotDependenciesResponse snapshotDependencies) {
      if (snapshotDependencies == null || snapshotDependencies.SnapshotDependency == null)
        return new List<SnapshotDependency>();
      return snapshotDependencies.SnapshotDependency.Select(Map).ToList();
    }
  }
}