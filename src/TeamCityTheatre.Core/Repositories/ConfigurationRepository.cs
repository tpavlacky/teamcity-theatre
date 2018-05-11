using System.IO;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TeamCityTheatre.Core.ApplicationModels;
using TeamCityTheatre.Core.Options;

namespace TeamCityTheatre.Core.Repositories {
  public interface IConfigurationRepository {
    Configuration GetConfiguration();
    void SaveConfiguration(Configuration configuration);
  }

  public class ConfigurationRepository : IConfigurationRepository {
    readonly DirectoryInfo _workspace;
    readonly FileInfo _configurationFile;

    public ConfigurationRepository(IOptionsSnapshot<StorageOptions> storageOptionsSnapshot) {
      var storageOptions = storageOptionsSnapshot.Value;
      _configurationFile = new FileInfo(storageOptions.ConfigurationFile);
      _workspace = _configurationFile.Directory;
    }

    public Configuration GetConfiguration() {
      EnsureConfigurationFileExists();
      return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(_configurationFile.FullName)) ?? new Configuration();
    }

    public void SaveConfiguration(Configuration configuration) {
      EnsureConfigurationFileExists();
      File.WriteAllText(_configurationFile.FullName, JsonConvert.SerializeObject(configuration, Formatting.Indented));
    }

    void EnsureConfigurationFileExists() {
      if (!_workspace.Exists)
        _workspace.Create();
      if (!_configurationFile.Exists)
        _configurationFile.Create().Dispose();
    }
  }
}