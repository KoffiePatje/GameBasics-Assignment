using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PouleSimulator
{
    public class Program
    {
        public static void Main(string[] args) {
            // Load the Simulation Config
            SimulationConfig simulationConfig = ConfigLoader<SimulationConfig>.LoadFromDisk("./simconfig.json", true);
            Console.WriteLine($"[Info] Loaded config:\n{simulationConfig}\n");

            // Load the Tweak Config
            TweakConfig tweakConfig = ConfigLoader<TweakConfig>.LoadFromDisk("./tweakconfig.json", true);
            Console.WriteLine($"[Info] Loaded config:\n{tweakConfig}\n");

            // Set up the seed for the simulation (check if the config specified one, if so use it, otherwise use the default time based random constructor)
            Random simulationRandom = simulationConfig.Seed > 0 ? new Random(simulationConfig.Seed) : new Random();

            // Create the Soccer Teams
            SoccerTeamFactory teamFactory = new SoccerTeamFactory(simulationRandom);
            Task<SoccerTeam[]> teamCreateTask = teamFactory.CreateRandomTeams(simulationConfig.NumberOfTeams);
            Task.WaitAll(teamCreateTask);

            SoccerTeam[] teams = teamCreateTask.Result;
            Console.WriteLine($"[Info] Created soccer teams ({teams.Length});\n\n{string.Join("\n\n", (IEnumerable<SoccerTeam>)teams)}\n");

            // Create the Matches
            MatchDistributionFactory matchDistributionFactory = new MatchDistributionFactory(simulationRandom);
            IMatchDistributionStrategy matchDistributionStrategy = matchDistributionFactory.CreateMatchDistributionStrategy(simulationConfig.MatchDistributionStrategy);

            Match[] matches = matchDistributionStrategy.CreateMatches(teamCreateTask.Result);
            Console.WriteLine($"[Info] Created matches ({matches.Length});\n\n{string.Join("\n", matches)}\n");

            // Let's Simulate the Matches
            SoccerMatchSimulator simulator = new SoccerMatchSimulator(tweakConfig, simulationRandom);
            SimulationResult[] simulationResults = new SimulationResult[simulationConfig.NumberOfSimulations];

            Console.WriteLine("[Info] Simulating matches...");
            for(int i = 0; i < simulationConfig.NumberOfSimulations; i++) {
                simulationResults[i] = simulator.SimulateMatches(matches);
            }
            Console.WriteLine("[Info] Matches simulated, press a key to go to the results");
            Console.ReadKey();

            // Now process those results into scores and display them
            Scoreboard[] scoreboards = simulationResults.Select(res => Scorer.ComputeScore(res)).ToArray();
            SimulationResultViewer simulationViewer = new SimulationResultViewer(scoreboards);
            simulationViewer.Display();
        }
    }
}
