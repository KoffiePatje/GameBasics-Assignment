using System;
using System.Collections.Generic;
using System.IO;

namespace PouleSimulator
{
    public class SoccerMatchSimulator
    {
        private readonly List<IPlayerAction> allActions;
        private readonly List<IPlayerAction> validActions;
        private readonly List<double> validActionScores;

        private readonly Random random;

        private readonly bool writeReportsToDisk;
        private readonly DirectoryInfo reportDirectory;

        public SoccerMatchSimulator(TweakConfig tweakConfig, Random random = null, bool writeReportsToDisk = false) {
            this.random = random ?? new Random();

            allActions = new List<IPlayerAction> {
                new PassForwardPlayerAction(this.random, tweakConfig),
                new PassBackPlayerAction(this.random, tweakConfig),
                new PassHorizontalPlayerAction(this.random, tweakConfig),
                new ShootAtGoalPlayerAction(this.random, tweakConfig),
                new DribblingPlayerAction(this.random, tweakConfig)
            };

            // Pre-allocate the full range of possible actions here to prevent a reallocation from ever happening
            validActions = new List<IPlayerAction>(allActions.Count);
            validActionScores = new List<double>(allActions.Count);

            this.writeReportsToDisk = writeReportsToDisk;
            if(writeReportsToDisk) {
                reportDirectory = new DirectoryInfo($"./Simulation-{DateTime.Now:yyyy-MM-dd_HH-mm-ss-ffff}");
                reportDirectory.Create();
            }
        }

        public SimulationResult SimulateMatches(Match[] matches) {
            MatchResult[] matchResults = new MatchResult[matches.Length];

            if(writeReportsToDisk) {
                for(int i = 0; i < matches.Length; i++) {
                    using(StreamWriter writer = writeReportsToDisk ? File.CreateText(Path.Combine(reportDirectory.FullName, $"Match-{i}.record")) : null) {
                        matchResults[i] = SimulateMatch(matches[i], writer);
                    }
                }
            } else {
                for(int i = 0; i < matches.Length; i++)
                    matchResults[i] = SimulateMatch(matches[i]);
            }

            return new SimulationResult(matchResults);
        }

        public MatchResult SimulateMatch(Match match, StreamWriter writer = null) {
            const double matchHalfTime = 45.0 * 60.0;
            const double matchEndTime = 90.0 * 60.0;

            // Construct a map that allows translating a SoccerPlayer to it's corresponding Team
            Dictionary<SoccerPlayer, SoccerTeam> playerToTeamMap = new Dictionary<SoccerPlayer, SoccerTeam>();
            foreach(SoccerPlayer homePlayer in match.Home.AllPlayers) { playerToTeamMap[homePlayer] = match.Home; }
            foreach(SoccerPlayer awayPlayer in match.Away.AllPlayers) { playerToTeamMap[awayPlayer] = match.Away; }

            // We give the ball to the home team in the first half, and to the away team in the second half;
            MatchState currentMatchState = MatchState.CreateFromKickOff(match, match.Home, random);

            // Let's simulate the first half
            SimulateMatchTillTime(matchHalfTime, playerToTeamMap, ref currentMatchState, writer);

            currentMatchState = currentMatchState.Halftime(match.Away, random, matchHalfTime);

            // Now for the second half
            SimulateMatchTillTime(matchEndTime, playerToTeamMap, ref currentMatchState, writer);

            return currentMatchState.GetMatchResult();
        }

        private void SimulateMatchTillTime(double timeToStop, Dictionary<SoccerPlayer, SoccerTeam> playerToTeamMap, ref MatchState currentMatchState, StreamWriter writer = null) {
            while(currentMatchState.MatchTime < timeToStop) {
                IPlayerAction action = DetermineNextAction(ref currentMatchState);

                PlayerActionResult actionResult = action.Execute(ref currentMatchState);
                if(!actionResult.HasScored) {
                    currentMatchState = currentMatchState.ActionProgress(
                        playerToTeamMap[actionResult.NewPlayerWithBallPosession],
                        actionResult.NewPlayerWithBallPosession,
                        action.ActionDurationInSeconds
                    );
                } else {
                    currentMatchState = currentMatchState.Score(
                        random,
                        action.ActionDurationInSeconds
                    );
                }

                writer?.WriteLine($"[{currentMatchState.MatchTime}] {action}: Success: {actionResult.Success}, BallPosession: {(currentMatchState.AttackingTeam == currentMatchState.HomeTeam ? "Home" : "Away")}");
            }
        }

        private IPlayerAction DetermineNextAction(ref MatchState matchState) {
            validActions.Clear();
            validActionScores.Clear();

            // Gather the possible actions for the current player
            for(int i = 0; i < allActions.Count; i++) {
                if(allActions[i].CanPerformAction(matchState.PlayerWithBallPossesion)) {
                    validActions.Add(allActions[i]);
                }
            }

            // Now allow those actions to compute a score based on the current matchstate
            double totalScore = 0.0;
            for(int i = 0; i < validActions.Count; i++) {
                double actionScore = validActions[i].GetActionScore(ref matchState);

                validActionScores.Add(actionScore);
                totalScore += actionScore;
            }

            // We roll a random number to determine what choice the player makes
            double actionChoice = random.NextDouble() * totalScore;

            // Translate the roll into the correct action and return that
            for(int i = 0; i < validActions.Count; i++) {
                double actionScore = validActionScores[i];

                if(actionChoice <= actionScore) {
                    return validActions[i];
                }

                actionChoice -= actionScore;
            }

            throw new Exception("The loop above should return an action in all scenario's");
        }
    }
}
