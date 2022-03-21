namespace PouleSimulator
{
    /// <summary>
    /// Interface for configurations loaded by <see cref="ConfigLoader{T}"/>
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// Allows the commandline to override loaded config values
        /// </summary>
        void ProcessCommandLineArguments(string[] args);

        /// <summary>
        /// Ensures all loaded/overridden values are within the application's allowed perimiters
        /// </summary>
        void Validate();
    }
}
