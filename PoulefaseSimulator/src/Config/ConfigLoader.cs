using Newtonsoft.Json;
using System;

namespace PouleSimulator
{
    /// <summary>
    /// This helper class ensures the correct loading and processing of any <see cref="IConfig"/>
    /// </summary>
    public static class ConfigLoader<T> where T : class, IConfig, new()
    {
        /// <summary>
        /// Attemtps to load an <see cref="IConfig"/> instance from disk, optionally writing the default instance to disk if the file did not yet exist.
        /// </summary>
        public static T LoadFromDisk(string path, bool createIfNotExists = false) {
            T config;

            if(!string.IsNullOrEmpty(path)) {
                if(!FileUtility.TryGetFileContentsFromJsonFilePath(path, out T loadedConfig)) {
                    config = new T();

                    if(createIfNotExists)
                        FileUtility.TrySaveObjectToPath(path, config, Formatting.Indented);
                } else {
                    config = loadedConfig;
                }
            } else {
                config = new T();
            }

            return ProcessLoadedConfig(config);
        }

        /// <summary>
        /// Returns a default instance of the <see cref="IConfig"/> adjusted by command line arguments if applicable
        /// </summary>
        public static T LoadDefault() {
            return ProcessLoadedConfig(new T());
        }

        private static T ProcessLoadedConfig(T config) {
            config.ProcessCommandLineArguments(Environment.GetCommandLineArgs());
            config.Validate();
            return config;
        }
    }
}
