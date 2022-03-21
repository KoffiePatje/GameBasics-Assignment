using System;
using System.Linq;

namespace PouleSimulator
{
    public class StatisticsSimulationView : ISimulationResultView
    {
        private readonly Statistics statistics;

        public StatisticsSimulationView(Scoreboard[] results) {
            statistics =  StatisticProcessor.ComputeStatistics(results);
        }

        public void Display(ConsoleKeyInfo keyPress) {
            int largestTeamName = statistics.StatisticsPerTeam.Max((teamScore) => teamScore.Team.Name.Length);
            largestTeamName += 1;

            Console.WriteLine($"-{new string('-', largestTeamName)}-----------------------------------------------------------------------");
            Console.WriteLine($"|{"Name".PadRight(largestTeamName)}| Off | Def | Pos | Points | Wins | Draws | Loss |  GS  |  GR  |  GD  |");
            Console.WriteLine($"-{new string('-', largestTeamName)}-----------------------------------------------------------------------");

            for(int i = 0; i < statistics.StatisticsPerTeam.Count; i++) {
                TeamStatistics teamStats = statistics.StatisticsPerTeam[i];

                Console.Write($"|{teamStats.Team.Name.PadRight(largestTeamName)}|");
                Console.Write($"{Math.Round(teamStats.AverageOffensiveSkillIndex * 100.0, 1),-5}|");
                Console.Write($"{Math.Round(teamStats.AverageDefensiveSkillIndex * 100.0, 1),-5}|");
                Console.Write($"{Math.Round(teamStats.AveragePosition, 1),-5}|");
                Console.Write($"{Math.Round(teamStats.AveragePoints, 1),-8}|");
                Console.Write($"{Math.Round(teamStats.AverageWins, 1),-6}|");
                Console.Write($"{Math.Round(teamStats.AverageDraws, 1),-7}|");
                Console.Write($"{Math.Round(teamStats.AverageLosses, 1),-6}|");
                Console.Write($"{Math.Round(teamStats.AverageGoalsScored, 1),-6}|");
                Console.Write($"{Math.Round(teamStats.AverageGoalsReceived, 1),-6}|");
                Console.Write($"{((teamStats.AverageGoalDifferential > 0 ? "+" : "") + Math.Round(teamStats.AverageGoalDifferential, 1)),-6}|");
                Console.Write(Environment.NewLine);
            }
            Console.WriteLine($"-{new string('-', largestTeamName)}-----------------------------------------------------------------------");
            Console.WriteLine();
        }
    }
}
