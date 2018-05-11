using System;
using System.Collections.Generic;
using System.Linq;
using TeamCityTheatre.Core.Client.Responses;
using TeamCityTheatre.Core.Models;

namespace TeamCityTheatre.Core.Client.Mappers {
  public interface IProjectMapper {
    Project Map(ProjectResponse project);
    IReadOnlyCollection<Project> Map(ProjectsResponse projects);
  }

  public class ProjectMapper : IProjectMapper {
    readonly Lazy<IBuildConfigurationMapper> _buildConfigurationMapper;

    public ProjectMapper(Lazy<IBuildConfigurationMapper> buildConfigurationMapper) {
      _buildConfigurationMapper = buildConfigurationMapper;
    }

    public Project Map(ProjectResponse project) {
      if (project == null) return null;
      return new Project {
        BuildConfigurations = _buildConfigurationMapper.Value.Map(project.BuildTypes),
        Description = project.Description,
        Href = project.Href,
        Id = project.Id,
        IsArchived = project.IsArchived,
        Name = project.Name,
        ParentProject = Map(project.ParentProject),
        ParentProjectId = project.ParentProjectId,
        WebUrl = project.WebUrl
      };
    }

    public IReadOnlyCollection<Project> Map(ProjectsResponse projects) {
      if (projects == null || projects.Project == null)
        return new List<Project>();
      return projects.Project.Select(Map).ToList();
    }
  }
}