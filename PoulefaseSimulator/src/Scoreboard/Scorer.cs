using System;
using System.Collections.Generic;
using System.Linq;

namespace PouleSimulator
{
    public static class Scorer
    {
        public static Scoreboard ComputeScore(SimulationResult simulationResult) {
            Dictionary<SoccerTeam, TeamScore> scoreByTeam = new Dictionary<SoccerTeam, TeamScore>();

            for(int i = 0; i < simulationResult.Results.Length; ++i) {
                MatchResult matchResult = simulationResult.Results[i];

                SoccerTeam homeTeam = matchResult.Match.Home;
                SoccerTeam awayTeam = matchResult.Match.Away;

                if(!scoreByTeam.TryGetValue(homeTeam, out TeamScore scoreHomeTeam)) {
                    scoreHomeTeam = new TeamScore { Team = homeTeam };
                }

                if(!scoreByTeam.TryGetValue(awayTeam, out TeamScore scoreAwayTeam)) {
                    scoreAwayTeam = new TeamScore { Team = awayTeam };
                }

                int result = matchResult.GoalsScoredByHomeTeam - matchResult.GoalsScoredByAwayTeam;

                if(result > 0) {
                    scoreHomeTeam.NumberOfWins++;
                    scoreAwayTeam.NumberOfLosses++;
                    scoreHomeTeam.Points += 3;
                } else if(result < 0) {
                    scoreHomeTeam.NumberOfLosses++;
                    scoreAwayTeam.NumberOfWins++;
                    scoreAwayTeam.Points += 3;
                } else {
                    scoreHomeTeam.NumberOfDraws++;
                    scoreAwayTeam.NumberOfDraws++;
                    scoreHomeTeam.Points++;
                    scoreAwayTeam.Points++;
                }

                scoreHomeTeam.GoalsScored += matchResult.GoalsScoredByHomeTeam;
                scoreHomeTeam.GoalsReceived += matchResult.GoalsScoredByAwayTeam;
                scoreHomeTeam.GoalDifferential = scoreHomeTeam.GoalsScored - scoreHomeTeam.GoalsReceived;

                scoreAwayTeam.GoalsScored += matchResult.GoalsScoredByAwayTeam;
                scoreAwayTeam.GoalsReceived += matchResult.GoalsScoredByHomeTeam;
                scoreAwayTeam.GoalDifferential = scoreAwayTeam.GoalsScored - scoreAwayTeam.GoalsReceived;

                scoreByTeam[homeTeam] = scoreHomeTeam;
                scoreByTeam[awayTeam] = scoreAwayTeam;
            }

            TeamScore[] scores = scoreByTeam.Values.ToArray();

            for(int i = 0; i < scores.Length; i++) {
                double totalOffensiveIndex = 0.0;
                double totalDefensiveIndex = 0.0;
                int playerCount = 0;

                foreach(SoccerPlayer player in scores[i].Team.AllPlayers) {
                    totalOffensiveIndex += player.OffensiveSkillIndex;
                    totalDefensiveIndex += player.DefensiveSkillIndex;
                    playerCount++;
                }

                scores[i].AverageOffensiveSkillIndex = totalOffensiveIndex / playerCount;
                scores[i].AverageDefensiveSkillIndex = totalDefensiveIndex / playerCount;
            }

            Array.Sort(scores, (lhs, rhs) => {
                if(lhs.Points != rhs.Points)
                    return rhs.Points.CompareTo(lhs.Points);

                if(lhs.GoalDifferential != rhs.GoalDifferential)
                    return rhs.GoalDifferential.CompareTo(lhs.GoalDifferential);

                if(lhs.GoalsScored != rhs.GoalsScored)
                    return rhs.GoalsScored.CompareTo(lhs.GoalsScored);

                if(lhs.GoalsReceived != rhs.GoalsReceived)
                    return lhs.GoalsReceived.CompareTo(rhs.GoalsReceived); // These are reversed since more received goals is worse

                // Not too pretty, but this should only happen on a very rare occasion
                MatchResult matchResult = simulationResult.Results.Where((result) => (result.Match.Home == lhs.Team || result.Match.Home == rhs.Team) && (result.Match.Away == lhs.Team || result.Match.Away == rhs.Team)).First();
                return matchResult.GoalsScoredByHomeTeam - matchResult.GoalsScoredByAwayTeam;
            });

            return new Scoreboard(scores, simulationResult.Results);
        }
    }
}
