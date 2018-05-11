using System;
using System.Linq;
using System.Threading.Tasks;
using TeamCityTheatre.Core.DataServices;
using TeamCityTheatre.Core.QueryServices.Models;

namespace TeamCityTheatre.Core.QueryServices {
  public interface IViewService {
    Task<ViewData> GetLatestViewDataAsync(string viewId);
  }

  public class ViewService : IViewService {
    readonly IViewDataService _viewDataService;
    readonly ITileService _tileService;

    public ViewService(IViewDataService viewDataService, ITileService tileService) {
      _viewDataService = viewDataService ?? throw new ArgumentNullException(nameof(viewDataService));
      _tileService = tileService;
    }

    public async Task<ViewData> GetLatestViewDataAsync(string viewId) {
      var view = _viewDataService.GetViewById(Guid.Parse(viewId));
      var tiles = await Task.WhenAll(view.Tiles.Select(t => _tileService.GetLatestTileDataAsync(view, t)).ToList());
      return new ViewData {
        Id = view.Id,
        Tiles = tiles
      };
    }
  }
}