using System;
using System.Collections.Generic;
using System.Linq;
using TeamCityTheatre.Core.Client.Responses;
using TeamCityTheatre.Core.Models;

namespace TeamCityTheatre.Core.Client.Mappers {
  public interface IAgentRequirementMapper {
    AgentRequirement Map(AgentRequirementResponse agentRequirement);
    IReadOnlyCollection<AgentRequirement> Map(AgentRequirementsResponse agentRequirements);
  }

  public class AgentRequirementMapper : IAgentRequirementMapper {
    readonly IPropertyMapper _propertyMapper;

    public AgentRequirementMapper(IPropertyMapper propertyMapper) {
      _propertyMapper = propertyMapper ?? throw new ArgumentNullException(nameof(propertyMapper));
    }

    public AgentRequirement Map(AgentRequirementResponse agentRequirement) {
      if (agentRequirement == null)
        return null;
      return new AgentRequirement {
        Id = agentRequirement.Id,
        Type = agentRequirement.Type,
        Properties = _propertyMapper.Map(agentRequirement.Properties)
      };
    }

    public IReadOnlyCollection<AgentRequirement> Map(AgentRequirementsResponse agentRequirements) {
      if (agentRequirements == null || agentRequirements.AgentRequirement == null)
        return new List<AgentRequirement>();
      return agentRequirements.AgentRequirement.Select(Map).ToList();
    }
  }
}