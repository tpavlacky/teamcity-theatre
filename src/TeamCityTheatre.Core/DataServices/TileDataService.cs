using System;
using System.Linq;
using TeamCityTheatre.Core.ApplicationModels;
using TeamCityTheatre.Core.Repositories;

namespace TeamCityTheatre.Core.DataServices {
  public interface ITileDataService {
    Tile GetTileById(Guid id);
  }

  public class TileDataService : ITileDataService {
    readonly IConfigurationRepository _configurationRepository;

    public TileDataService(IConfigurationRepository configurationRepository) {
      _configurationRepository = configurationRepository ?? throw new ArgumentNullException(nameof(configurationRepository));
    }

    public Tile GetTileById(Guid id) {
      return _configurationRepository.GetConfiguration()
        .Views.SelectMany(v => v.Tiles)
        .SingleOrDefault(t => t.Id == id);
    }
  }
}