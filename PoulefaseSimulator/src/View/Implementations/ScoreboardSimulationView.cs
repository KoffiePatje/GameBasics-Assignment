using System;
using System.Collections.Generic;
using System.Linq;

namespace PouleSimulator
{
    public class ScoreboardSimulationView : ISimulationResultView
    {
        private Scoreboard CurrentScoreboard { get => scoreboards[selectedScoreboardIndex]; }

        private readonly Scoreboard[] scoreboards;

        private int selectedScoreboardIndex = 0;
        private int selectedTeamScoreIndex = 0;

        public ScoreboardSimulationView(Scoreboard[] scoreboards) {
            this.scoreboards = scoreboards;
        }

        public void Display(ConsoleKeyInfo keyPress) {
            HandleKeyPress(keyPress);

            Scoreboard currentScoreboard = scoreboards[selectedScoreboardIndex];
            SoccerTeam selectedTeam = currentScoreboard.Scores[selectedTeamScoreIndex].Team;

            int largestTeamName = currentScoreboard.Scores.Max((teamScore) => teamScore.Team.Name.Length);
            largestTeamName += 1;

            Console.WriteLine($"Match #{selectedScoreboardIndex + 1}:");

            Console.WriteLine($"  -{new string('-', largestTeamName)}-------------------------------------------");
            Console.WriteLine($"  |{"Name".PadRight(largestTeamName)}| Off | Def | P | W | D | L | GS | GR | GD |");
            Console.WriteLine($"  -{new string('-', largestTeamName)}-------------------------------------------");

            for(int i = 0; i < currentScoreboard.Scores.Count; i++) {
                TeamScore teamScore = currentScoreboard.Scores[i];

                Console.Write($"{(teamScore.Team == selectedTeam ? "\x1b[1m>\x1b[0m " : "  ")}|");
                Console.Write($"{teamScore.Team.Name.PadRight(largestTeamName)}|");
                Console.Write($"{Math.Round(teamScore.AverageOffensiveSkillIndex * 100.0, 1),-5}|");
                Console.Write($"{Math.Round(teamScore.AverageDefensiveSkillIndex * 100.0, 1),-5}|");
                Console.Write($"{teamScore.Points,-3}|");
                Console.Write($"{teamScore.NumberOfWins,-3}|");
                Console.Write($"{teamScore.NumberOfDraws,-3}|");
                Console.Write($"{teamScore.NumberOfLosses,-3}|");
                Console.Write($"{(teamScore.GoalDifferential > 0 ? $"+{teamScore.GoalDifferential}" : $"{teamScore.GoalDifferential}"),-4}|");
                Console.Write($"{teamScore.GoalsScored,-4}|");
                Console.Write($"{teamScore.GoalsReceived,-4}|");
                Console.Write(Environment.NewLine);
            }
            Console.WriteLine($"  -{new string('-', largestTeamName)}-------------------------------------------");

            Console.WriteLine();

            int matchIndex = 0;
            IEnumerable<MatchResult> selectedTeamMatches = currentScoreboard.Matches.Where((matchResult) => matchResult.Match.Home == selectedTeam || matchResult.Match.Away == selectedTeam);

            foreach(MatchResult result in selectedTeamMatches) {
                Console.Write($"#{matchIndex++}: ");
                Console.Write($"{result.Match.Home.Name.PadRight(largestTeamName)} - {result.Match.Away.Name.PadRight(largestTeamName)} ");
                Console.Write($"{result.GoalsScoredByHomeTeam} - {result.GoalsScoredByAwayTeam}");
                Console.Write(Environment.NewLine);
            }
            
            Console.Write(Environment.NewLine);
        }

        private void HandleKeyPress(ConsoleKeyInfo keyPress) {
            switch(keyPress.Key) {
                case ConsoleKey.LeftArrow: {
                    selectedScoreboardIndex--;
                    if(selectedScoreboardIndex < 0) {
                        selectedScoreboardIndex += scoreboards.Length;
                    }
                    selectedTeamScoreIndex = 0;
                }
                break;

                case ConsoleKey.RightArrow: {
                    selectedScoreboardIndex++;
                    if(selectedScoreboardIndex >= scoreboards.Length) {
                        selectedScoreboardIndex -= scoreboards.Length;
                    }
                    selectedTeamScoreIndex = 0;
                }
                break;

                case ConsoleKey.UpArrow: {
                    selectedTeamScoreIndex--;
                    if(selectedTeamScoreIndex < 0) {
                        selectedTeamScoreIndex += CurrentScoreboard.Scores.Count;
                    }
                }
                break;

                case ConsoleKey.DownArrow: {
                    selectedTeamScoreIndex++;
                    if(selectedTeamScoreIndex >= CurrentScoreboard.Scores.Count) {
                        selectedTeamScoreIndex -= CurrentScoreboard.Scores.Count;
                    }
                }
                break;
            }

        }
    }
}
