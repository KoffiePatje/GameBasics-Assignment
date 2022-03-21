using System;
using System.Collections.Generic;
using System.IO;

namespace PouleSimulator
{
    public class SoccerMatchSimulator
    {
        public event Action<int> OnMatchSimulationStartedEvent;
        public event Action<int> OnMatchSimulationStoppedEvent;
        public event Action<IPlayerAction, MatchState> OnMatchSimulationStepEvent;

        private readonly List<IPlayerAction> allActions;
        private readonly List<IPlayerAction> validActions;
        private readonly List<double> validActionScores;

        private readonly Random random;
        private readonly DirectoryInfo reportDirectory;
        
        private int simulatedMatchesCount = 0;

        public SoccerMatchSimulator(TweakConfig tweakConfig, Random random = null) {
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

            reportDirectory = new DirectoryInfo($"./Simulation-{DateTime.Now:yyyy-MM-dd_HH-mm-ss-ffff}");
            reportDirectory.Create();
        }

        public SimulationResult SimulateMatches(Match[] matches) {
            MatchResult[] matchResults = new MatchResult[matches.Length];

            for(int i = 0; i < matches.Length; i++) {
                matchResults[i] = SimulateMatch(matches[i]);
            }

            return new SimulationResult(matchResults);
        }

        public MatchResult SimulateMatch(Match match) {
            const double matchHalfTime = 45.0 * 60.0;
            const double matchEndTime = 90.0 * 60.0;

            // Construct a map that allows translating a SoccerPlayer to it's corresponding Team
            Dictionary<SoccerPlayer, SoccerTeam> playerToTeamMap = new Dictionary<SoccerPlayer, SoccerTeam>();
            foreach(SoccerPlayer homePlayer in match.Home.AllPlayers) { playerToTeamMap[homePlayer] = match.Home; }
            foreach(SoccerPlayer awayPlayer in match.Away.AllPlayers) { playerToTeamMap[awayPlayer] = match.Away; }

            int currentMatchSimulationId = simulatedMatchesCount++;
            OnMatchSimulationStartedEvent?.Invoke(currentMatchSimulationId);

            using(StreamWriter writer = File.CreateText(Path.Combine(reportDirectory.FullName, $"Match-{currentMatchSimulationId}.record"))) {
                // We give the ball to the home team in the first half, and to the away team in the second half;
                MatchState currentMatchState = MatchState.KickOff(match, match.Home, random);
                OnMatchSimulationStepEvent?.Invoke(null, currentMatchState);

                // Let's simulate the first half
                while(currentMatchState.MatchTime < matchHalfTime) {
                    IPlayerAction action = DetermineNextAction(ref currentMatchState);

                    PlayerActionResult actionResult = action.Execute(ref currentMatchState);
                    if(actionResult.HasScored) {
                        currentMatchState = MatchState.Score(currentMatchState, random, action.ActionDurationInSeconds);
                    } else {
                        currentMatchState = MatchState.ActionProgress(currentMatchState, playerToTeamMap[actionResult.NewPlayerWithBallPosession], actionResult.NewPlayerWithBallPosession, action.ActionDurationInSeconds);
                    }

                    writer.WriteLine($"[{currentMatchState.MatchTime}] {action}: Success: {actionResult.Success}, BallPosession: {(currentMatchState.AttackingTeam == currentMatchState.HomeTeam ? "Home" : "Away")}");
                }

                currentMatchState = MatchState.Halftime(currentMatchState, match.Away, random, matchHalfTime);

                // Now for the second half
                while(currentMatchState.MatchTime < matchEndTime) {
                    IPlayerAction action = DetermineNextAction(ref currentMatchState);

                    PlayerActionResult actionResult = action.Execute(ref currentMatchState);
                    if(actionResult.HasScored) {
                        currentMatchState = MatchState.Score(currentMatchState, random, action.ActionDurationInSeconds);
                    } else {
                        currentMatchState = MatchState.ActionProgress(currentMatchState, playerToTeamMap[actionResult.NewPlayerWithBallPosession], actionResult.NewPlayerWithBallPosession, action.ActionDurationInSeconds);
                    }

                    writer.WriteLine($"[{currentMatchState.MatchTime}] {action}: Success: {actionResult.Success}, BallPosession: {(currentMatchState.AttackingTeam == currentMatchState.HomeTeam ? "Home" : "Away")}");
                }

                return currentMatchState.GetMatchResult();
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
                    //Console.WriteLine($"[Info] Chosen action '{validActions[i]}' at matchTime: {matchState.MatchTime}");
                    return validActions[i];
                }

                actionChoice -= actionScore;
            }

            throw new InvalidOperationException("The loop above should return an action in all scenario's");
        }
    }
}
