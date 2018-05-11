using System;
using System.Collections.Generic;
using System.Linq;
using TeamCityTheatre.Core.Client.Responses;
using TeamCityTheatre.Core.Models;

namespace TeamCityTheatre.Core.Client.Mappers {
  public interface IVcsRootEntryMapper {
    VcsRootEntry Map(VcsRootEntryResponse vcsRootEntry);
    IReadOnlyCollection<VcsRootEntry> Map(VcsRootEntriesResponse vcsRootEntries);
  }

  public class VcsRootEntryMapper : IVcsRootEntryMapper {
    readonly IVcsRootMapper _vcsRootMapper;

    public VcsRootEntryMapper(IVcsRootMapper vcsRootMapper) {
      _vcsRootMapper = vcsRootMapper ?? throw new ArgumentNullException(nameof(vcsRootMapper));
    }

    public VcsRootEntry Map(VcsRootEntryResponse vcsRootEntry) {
      if (vcsRootEntry == null) return null;
      return new VcsRootEntry {
        CheckoutRules = vcsRootEntry.CheckoutRules,
        VcsRoot = _vcsRootMapper.Map(vcsRootEntry.VcsRoot)
      };
    }

    public IReadOnlyCollection<VcsRootEntry> Map(VcsRootEntriesResponse vcsRootEntries) {
      if (vcsRootEntries == null || vcsRootEntries.VcsRootEntry == null)
        return new List<VcsRootEntry>();
      return vcsRootEntries.VcsRootEntry.Select(Map).ToList();
    }
  }
}