using System;
using System.Collections.Generic;
using System.Linq;
using TeamCityTheatre.Core.Client.Responses;
using TeamCityTheatre.Core.Models;

namespace TeamCityTheatre.Core.Client.Mappers {
  public interface IBuildStepMapper {
    BuildStep Map(BuildStepResponse buildStep);
    IReadOnlyCollection<BuildStep> Map(BuildStepsResponse buildStep);
  }

  internal class BuildStepMapper : IBuildStepMapper {
    readonly IPropertyMapper _propertyMapper;

    public BuildStepMapper(IPropertyMapper propertyMapper) {
      _propertyMapper = propertyMapper ?? throw new ArgumentNullException(nameof(propertyMapper));
    }

    public BuildStep Map(BuildStepResponse buildStep) {
      if (buildStep == null) return null;
      return new BuildStep {
        Id = buildStep.Id,
        Name = buildStep.Name,
        Type = buildStep.Type,
        Properties = _propertyMapper.Map(buildStep.Properties)
      };
    }

    public IReadOnlyCollection<BuildStep> Map(BuildStepsResponse buildStep) {
      if (buildStep == null || buildStep.BuildStep == null)
        return new List<BuildStep>();
      return buildStep.BuildStep.Select(Map).ToList();
    }
  }
}