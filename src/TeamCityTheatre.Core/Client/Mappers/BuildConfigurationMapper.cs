using System;
using System.Collections.Generic;
using System.Linq;
using TeamCityTheatre.Core.Client.Responses;
using TeamCityTheatre.Core.Models;

namespace TeamCityTheatre.Core.Client.Mappers {
  public interface IBuildConfigurationMapper {
    BuildConfiguration Map(BuildTypeResponse buildType);
    IReadOnlyCollection<BuildConfiguration> Map(BuildTypesResponse buildTypes);
  }

  public class BuildConfigurationMapper : IBuildConfigurationMapper {
    readonly IAgentRequirementMapper _agentRequirementMapper;
    readonly IArtifactDependencyMapper _artifactDependencyMapper;
    readonly IBuildStepMapper _buildStepMapper;
    readonly IBuildTriggerMapper _buildTriggerMapper;
    readonly IPropertyMapper _propertyMapper;
    readonly ISnapshotDependencyMapper _snapshotDependencyMapper;
    readonly IVcsRootEntryMapper _vcsRootEntryMapper;

    public BuildConfigurationMapper(
      IAgentRequirementMapper agentRequirementMapper,
      IArtifactDependencyMapper artifactDependencyMapper,
      IPropertyMapper propertyMapper,
      ISnapshotDependencyMapper snapshotDependencyMapper,
      IBuildStepMapper buildStepMapper,
      IBuildTriggerMapper buildTriggerMapper,
      IVcsRootEntryMapper vcsRootEntryMapper) {
      _agentRequirementMapper = agentRequirementMapper ?? throw new ArgumentNullException(nameof(agentRequirementMapper));
      _artifactDependencyMapper = artifactDependencyMapper ?? throw new ArgumentNullException(nameof(artifactDependencyMapper));
      _propertyMapper = propertyMapper ?? throw new ArgumentNullException(nameof(propertyMapper));
      _snapshotDependencyMapper = snapshotDependencyMapper ?? throw new ArgumentNullException(nameof(snapshotDependencyMapper));
      _buildStepMapper = buildStepMapper ?? throw new ArgumentNullException(nameof(buildStepMapper));
      _buildTriggerMapper = buildTriggerMapper ?? throw new ArgumentNullException(nameof(buildTriggerMapper));
      _vcsRootEntryMapper = vcsRootEntryMapper ?? throw new ArgumentNullException(nameof(vcsRootEntryMapper));
    }

    public BuildConfiguration Map(BuildTypeResponse buildType) {
      if (buildType == null) return null;
      return new BuildConfiguration {
        Id = buildType.Id,
        Href = buildType.Href,
        Name = buildType.Name,
        ProjectId = buildType.ProjectId,
        WebUrl = buildType.WebUrl,
        AgentRequirements = _agentRequirementMapper.Map(buildType.AgentRequirements),
        ArtifactDependencies = _artifactDependencyMapper.Map(buildType.ArtifactDependencies),
        Parameters = _propertyMapper.Map(buildType.Parameters),
        Settings = _propertyMapper.Map(buildType.Settings),
        SnapshotDependencies = _snapshotDependencyMapper.Map(buildType.SnapshotDependencies),
        Steps = _buildStepMapper.Map(buildType.Steps),
        Triggers = _buildTriggerMapper.Map(buildType.Triggers),
        VcsRootEntries = _vcsRootEntryMapper.Map(buildType.VcsRootEntries)
      };
    }

    public IReadOnlyCollection<BuildConfiguration> Map(BuildTypesResponse buildType) {
      if (buildType == null || buildType.BuildType == null)
        return new List<BuildConfiguration>();
      return buildType.BuildType.Select(Map).ToList();
    }
  }
}