﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TeamCityTheatre.Core.ApplicationModels;
using TeamCityTheatre.Core.DataServices;
using TeamCityTheatre.Core.Models;
using TeamCityTheatre.Core.QueryServices.Models;

namespace TeamCityTheatre.Core.QueryServices {
  public interface ITileService {
    Task<TileData> GetLatestTileDataAsync(View view, Tile tile);
  }

  public class TileService : ITileService {
    readonly IBuildDataService _buildDataService;

    public TileService(IBuildDataService buildDataService) {
      _buildDataService = buildDataService ?? throw new ArgumentNullException(nameof(buildDataService));
    }

    public async Task<TileData> GetLatestTileDataAsync(View view, Tile tile) {
      var rawBuilds = await _buildDataService.GetBuildsOfBuildConfigurationAsync(tile.BuildConfigurationId, 20);
			if (string.IsNullOrEmpty(view.BranchFilter))
			{
				return CreateDefaultTileData(view, tile, rawBuilds);
			}
			else
			{
				return CreateTileDataByBranchFilter(view, tile, rawBuilds);
			}
    }

		private static TileData CreateDefaultTileData(View view, Tile tile, IEnumerable<IDetailedBuild> rawBuilds)
		{
			var buildsOrderByStartDate = rawBuilds.OrderByDescending(b => b.StartDate).ToList();
			var defaultBranchBuild = buildsOrderByStartDate.FirstOrDefault(b => b.IsDefaultBranch);
			var nonDefaultBranchBuilds = buildsOrderByStartDate.Where(b => !b.IsDefaultBranch)
				.GroupBy(b => b.BranchName)
				.Select(buildsPerBranch => buildsPerBranch.OrderByDescending(b => b.StartDate).First())
				.Take(defaultBranchBuild != null
					? view.DefaultNumberOfBranchesPerTile - 1
					: view.DefaultNumberOfBranchesPerTile);
			var builds = defaultBranchBuild != null
				? new[] { defaultBranchBuild }.Concat(nonDefaultBranchBuilds)
				: nonDefaultBranchBuilds;
			return new TileData
			{
				Id = tile.Id,
				Label = tile.Label,
				Builds = builds.ToList()
			};
		}

		private static TileData CreateTileDataByBranchFilter(View view, Tile tile, IEnumerable<IDetailedBuild> rawBuilds)
		{
			var buildsOrderByStartDate = rawBuilds.OrderByDescending(b => b.StartDate).ToList();
			var filteredBuilds = buildsOrderByStartDate
				.GroupBy(b => b.BranchName)
				.Where((grp) => BranchBuildsPredicate(grp, view))
				.Select(buildsPerBranch => buildsPerBranch.OrderByDescending(b => b.StartDate).First())
				.OrderBy(build => !build.IsDefaultBranch)
				.ThenBy(build => build.BranchName)
				.Take(view.DefaultNumberOfBranchesPerTile);
			return new TileData
			{
				Id = tile.Id,
				Label = tile.Label,
				Builds = filteredBuilds.ToList()
			};
		}

		private static bool BranchBuildsPredicate(IGrouping<string, IDetailedBuild> arg, View view)
		{
			if (string.IsNullOrEmpty(view.BranchFilter))
			{
				return true;
			}
			else
			{
				return BranchFilterPredicate(arg, view.BranchFilter);
			}
		}

		private static bool BranchFilterPredicate(IGrouping<string, IDetailedBuild> grouping, string branchFilter)
		{
			var build = grouping.FirstOrDefault();
			if (string.IsNullOrEmpty(build?.BranchName))
			{
				return false;
			}

			return Regex.IsMatch(build.BranchName, branchFilter, RegexOptions.IgnoreCase);
		}
	}
}