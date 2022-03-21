using System;
using System.Collections.Generic;
using System.Linq;

namespace PouleSimulator
{
    public static class StatisticProcessor
    {
        public static Statistics ComputeStatistics(Scoreboard[] scoreboards) {
            int teamCount = scoreboards[0].Scores.Count;
            Dictionary<SoccerTeam, TeamStatistics> statisticsByTeam = new Dictionary<SoccerTeam, TeamStatistics>(teamCount);

            for(int j = 0; j < scoreboards.Length; j++) {
                IReadOnlyList<TeamScore> scores = scoreboards[j].Scores;

                for(int i = 0; i < scoreboards[j].Scores.Count; i++) {
                    TeamScore teamScore = scoreboards[j].Scores[i];

                    if(!statisticsByTeam.TryGetValue(teamScore.Team, out TeamStatistics teamStatistics)) {
                        teamStatistics = new TeamStatistics(teamScore.Team, teamScore.AverageOffensiveSkillIndex, teamScore.AverageDefensiveSkillIndex);
                    }

                    teamStatistics.AveragePosition += i + 1;
                    teamStatistics.AveragePoints += teamScore.Points;
                    teamStatistics.AverageWins += teamScore.NumberOfWins;
                    teamStatistics.AverageDraws += teamScore.NumberOfDraws;
                    teamStatistics.AverageLosses += teamScore.NumberOfLosses;
                    teamStatistics.AverageGoalDifferential += teamScore.GoalDifferential;
                    teamStatistics.AverageGoalsScored += teamScore.GoalsScored;
                    teamStatistics.AverageGoalsReceived += teamScore.GoalsReceived;

                    statisticsByTeam[teamScore.Team] = teamStatistics;
                }
            }
            
            TeamStatistics[] statistics = statisticsByTeam.Values.ToArray();
            int sampleSize = scoreboards.Length;

            for(int i = 0; i < statistics.Length; i++) {
                TeamStatistics teamStatistic = statistics[i];

                teamStatistic.AveragePosition /= sampleSize;
                teamStatistic.AveragePoints /= sampleSize;
                teamStatistic.AverageWins /= sampleSize;
                teamStatistic.AverageDraws /= sampleSize;
                teamStatistic.AverageLosses /= sampleSize;
                teamStatistic.AverageGoalDifferential /= sampleSize;
                teamStatistic.AverageGoalsScored /= sampleSize;
                teamStatistic.AverageGoalsReceived /= sampleSize;

                statistics[i] = teamStatistic;
            }

            Array.Sort(statistics, (lhs, rhs) => {
                if(lhs.AveragePosition != rhs.AveragePosition)
                    return lhs.AveragePosition.CompareTo(rhs.AveragePosition);

                if(lhs.AveragePoints != rhs.AveragePoints)
                    return lhs.AveragePoints.CompareTo(rhs.AveragePoints);

                if(lhs.AverageWins != rhs.AverageWins)
                    return lhs.AverageWins.CompareTo(rhs.AverageWins);

                if(lhs.AverageDraws != rhs.AverageDraws)
                    return lhs.AverageDraws.CompareTo(rhs.AverageDraws);

                if(lhs.AverageLosses != rhs.AverageLosses)
                    return lhs.AverageLosses.CompareTo(rhs.AverageLosses);

                if(lhs.AverageGoalDifferential != rhs.AverageGoalDifferential)
                    return lhs.AverageGoalDifferential.CompareTo(rhs.AverageGoalDifferential);

                if(lhs.AverageGoalsScored != rhs.AverageGoalsScored)
                    return lhs.AverageGoalsScored.CompareTo(rhs.AverageGoalsScored);

                if(lhs.AverageGoalsReceived != rhs.AverageGoalsReceived)
                    return lhs.AverageGoalsReceived.CompareTo(rhs.AverageGoalsReceived);

                return lhs.Team.Name.CompareTo(rhs.Team.Name);
            });

            return new Statistics(statistics);
        }
    }
}
