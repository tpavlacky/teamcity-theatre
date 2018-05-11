using System.Collections.Generic;
using System.Linq;
using TeamCityTheatre.Core.Client.Responses;
using TeamCityTheatre.Core.Models;

namespace TeamCityTheatre.Core.Client.Mappers {
  public interface IPropertyMapper {
    Property Map(PropertyResponse property);
    IReadOnlyCollection<Property> Map(PropertiesResponse properties);
  }

  public class PropertyMapper : IPropertyMapper {
    public Property Map(PropertyResponse property) {
      if (property == null) return null;
      return new Property {
        Name = property.Name,
        Value = property.Value
      };
    }

    public IReadOnlyCollection<Property> Map(PropertiesResponse properties) {
      if (properties == null || properties.Property == null)
        return new List<Property>();
      return properties.Property.Select(Map).ToList();
    }
  }
}