using System;
using TeamCityTheatre.Core.Client.Responses;
using TeamCityTheatre.Core.Models;

namespace TeamCityTheatre.Core.Client.Mappers {
  public interface IVcsRootMapper {
    VcsRoot Map(VcsRootResponse vcsRootResponse);
  }

  public class VcsRootMapper : IVcsRootMapper {
    readonly Lazy<IProjectMapper> _projectMapper;
    readonly IPropertyMapper _propertyMapper;

    public VcsRootMapper(Lazy<IProjectMapper> projectMapper, IPropertyMapper propertyMapper) {
      _projectMapper = projectMapper ?? throw new ArgumentNullException(nameof(projectMapper));
      _propertyMapper = propertyMapper ?? throw new ArgumentNullException(nameof(propertyMapper));
    }

    public VcsRoot Map(VcsRootResponse vcsRoot) {
      if (vcsRoot == null) return null;
      return new VcsRoot {
        Href = vcsRoot.Href,
        Id = vcsRoot.Id,
        Name = vcsRoot.Name,
        VcsName = vcsRoot.VcsName,
        LastChecked = vcsRoot.LastChecked,
        Project = _projectMapper.Value.Map(vcsRoot.Project),
        Properties = _propertyMapper.Map(vcsRoot.Properties)
      };
    }
  }
}