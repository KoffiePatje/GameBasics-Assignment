using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace PouleSimulator
{
    /// <summary>
    /// Configuration representation of all tweakable Simulation specific values
    /// </summary>
    [Serializable]
    public class SimulationConfig : IConfig
    {
        /// <summary>
        /// The number of Teams that will be placed into the poule simulation
        /// </summary>
        [JsonProperty]
        public int NumberOfTeams { get; private set; } = 4;

        /// <summary>
        /// The number of simulations to run in a single application run
        /// </summary>
        [JsonProperty]
        public int NumberOfSimulations { get; private set; } = 100;

        /// <summary>
        /// The method of setting up matches between the teams
        /// </summary>
        [JsonProperty, JsonConverter(typeof(StringEnumConverter))]
        public EGroupMatchDistributionStrategy MatchDistributionStrategy { get; private set; } = EGroupMatchDistributionStrategy.RoundRobin;

        /// <summary>
        /// The seed inserted into the application wide random instance, offering the same seed should yield the exact same results every run.
        /// When the seed is set to a number equal or smaller than 0 the simulation does not use the seed and instead opts for a time based seed.
        /// </summary>
        [JsonProperty]
        public int Seed { get; private set; } = 0;

        /// <summary>
        /// Configures wether the Simulator should write it's reports to disk
        /// </summary>
        [JsonProperty]
        public bool LogSimulationReportsToDisk { get; private set; }

        // We implement this interface function explicitly to discourage usage through non interface references
        void IConfig.ProcessCommandLineArguments(string[] args) {
            for(int i = 0; i < args.Length; ++i) {
                string id = args[i];

                switch(id) {
                    case "-teams":
                    case "-numTeams":
                    case "-numberOfTeams": { 
                        string value = args[i++];

                        if(!int.TryParse(value, out int commandLineNumberOfTeams)) {
                            Console.WriteLine($"[Error] Unable to parse the value of '{id}' ({value}) to a valid number");
                            break;
                        }

                        if(commandLineNumberOfTeams < 2) {
                            Console.WriteLine($"[Error] The number of teams per group should at least be larger than or equal to 2");
                            break;
                        }

                        NumberOfTeams = commandLineNumberOfTeams;
                    }
                    break;

                    case "-sims":
                    case "-simulations":
                    case "-numSimulations":
                    case "-numberOfSimulations": {
                        string value = args[i++];

                        if(!int.TryParse(value, out int commandLineNumberOfSimulations)) {
                            Console.WriteLine($"[Error] Unable to parse the value of '{id}' ({value}) to a valid number");
                            break;
                        }

                        if(commandLineNumberOfSimulations < 1) {
                            Console.WriteLine($"[Error] The number of groups should at least be larger than or equal to 1");
                            break;
                        }

                        NumberOfSimulations = commandLineNumberOfSimulations;
                    }
                    break;
                }
            }
        }

        // We implement this interface function explicitly to discourage usage through non interface references
        void IConfig.Validate() {
            NumberOfTeams = Math.Max(2, NumberOfTeams);
            NumberOfSimulations = Math.Max(1, NumberOfSimulations);
           
            if(!Enum.IsDefined(typeof(EGroupMatchDistributionStrategy), MatchDistributionStrategy)) {
                MatchDistributionStrategy = EGroupMatchDistributionStrategy.RoundRobin;
            }

            Seed = Math.Max(0, Seed);
        }

        /// <summary>
        /// Returns a human readable JSON structure of the config
        /// </summary>
        public override string ToString() {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
