using System;
using System.Collections.Generic;
using System.Linq;
using TeamCityTheatre.Core.Client.Responses;
using TeamCityTheatre.Core.Models;

namespace TeamCityTheatre.Core.Client.Mappers {
  public interface IBuildMapper {
    Build Map(BuildResponse build);
    IReadOnlyCollection<Build> Map(BuildsResponse builds);
  }

  public class BuildMapper : IBuildMapper {
    readonly IAgentMapper _agentMapper;
    readonly IBuildStatusMapper _buildStatusMapper;
    readonly IBuildChangeMapper _buildChangeMapper;
    readonly IBuildConfigurationMapper _buildConfigurationMapper;
    readonly IPropertyMapper _propertyMapper;

    public BuildMapper(
      IBuildConfigurationMapper buildConfigurationMapper, IBuildChangeMapper buildChangeMapper,
      IPropertyMapper propertyMapper,
      IAgentMapper agentMapper, IBuildStatusMapper buildStatusMapper) {
      _buildConfigurationMapper = buildConfigurationMapper ?? throw new ArgumentNullException(nameof(buildConfigurationMapper));
      _buildChangeMapper = buildChangeMapper ?? throw new ArgumentNullException(nameof(buildChangeMapper));
      _propertyMapper = propertyMapper ?? throw new ArgumentNullException(nameof(propertyMapper));
      _agentMapper = agentMapper ?? throw new ArgumentNullException(nameof(agentMapper));
      _buildStatusMapper = buildStatusMapper ?? throw new ArgumentNullException(nameof(buildStatusMapper));
    }

    public Build Map(BuildResponse build) {
      if (build == null) return null;
      return new Build {
        Id = build.Id,
        BuildConfigurationId = build.BuildTypeId,
        Agent = _agentMapper.Map(build.Agent),
        ArtifactDependencies = Map(build.ArtifactDependencies),
        BranchName = build.BranchName,
        BuildConfiguration = _buildConfigurationMapper.Map(build.BuildType),
        FinishDate = build.FinishDate,
        Href = build.Href,
        IsDefaultBranch = build.DefaultBranch || string.Equals(build.BranchName, "develop") || string.Equals(build.BranchName, "master"),
        LastChanges = _buildChangeMapper.Map(build.LastChanges),
        Number = build.Number,
        PercentageComplete = build.PercentageComplete ?? build.RunningInfo?.PercentageComplete,
        ElapsedSeconds = build.RunningInfo?.ElapsedSeconds,
        EstimatedTotalSeconds = build.RunningInfo?.EstimatedTotalSeconds,
        CurrentStageText = build.RunningInfo?.CurrentStageText,
        Properties = _propertyMapper.Map(build.Properties),
        QueuedDate = build.QueuedDate,
        SnapshotDependencies = Map(build.SnapshotDependencies),
        StartDate = build.StartDate,
        State = build.State,
        Status = _buildStatusMapper.Map(build.Status),
        StatusText = build.StatusText,
        WebUrl = build.WebUrl
      };
    }

    public IReadOnlyCollection<Build> Map(BuildsResponse builds) {
      if (builds?.Build == null)
        return new List<Build>();
      return builds.Build.Select(Map).ToList();
    }
  }
}