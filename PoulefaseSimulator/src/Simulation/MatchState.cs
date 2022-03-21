using System;

namespace PouleSimulator
{
    public struct MatchState
    {
        public SoccerTeam HomeTeam { get => match.Home; }
        public SoccerTeam AwayTeam { get => match.Away; }

        public SoccerTeam AttackingTeam { get; }
        public SoccerTeam DefendingTeam { get; }

        public SoccerPlayer PlayerWithBallPossesion { get; }
        
        public double MatchTime { get; }
        
        public int HomeTeamGoals { get; }
        public int AwayTeamGoals { get; }

        private readonly Match match;

        private MatchState(Match match, SoccerTeam teamWithBallPossesion, SoccerPlayer playerWithBallPossesion, double matchTime, int homeTeamGoals, int awayTeamGoals) {
            this.match = match;

            this.AttackingTeam = teamWithBallPossesion;
            this.DefendingTeam = teamWithBallPossesion == match.Home ? match.Away : match.Home;

            this.PlayerWithBallPossesion = playerWithBallPossesion;
            this.MatchTime = matchTime;

            this.HomeTeamGoals = homeTeamGoals;
            this.AwayTeamGoals = awayTeamGoals;
        }

        public static MatchState KickOff(Match match, SoccerTeam attackingTeam, Random random) {
            return new MatchState(
                match, 
                attackingTeam, 
                attackingTeam.GetRandomMidfielder(random), 
                0.0, 
                0, 
                0
            );
        }

        public static MatchState Halftime(MatchState previousState, SoccerTeam attackingTeam, Random random, double halfTime = 45.0 * 60.0) {
            return new MatchState(
                previousState.match,
                attackingTeam,
                attackingTeam.GetRandomMidfielder(random),
                halfTime,
                previousState.HomeTeamGoals,
                previousState.AwayTeamGoals
            );
        }

        public static MatchState ActionProgress(MatchState previousState, SoccerTeam teamWithBallPossesion, SoccerPlayer playerWithBallPosession, double passedTimeInSeconds) {
            return new MatchState(
                previousState.match, 
                teamWithBallPossesion,
                playerWithBallPosession, 
                previousState.MatchTime + passedTimeInSeconds,
                previousState.HomeTeamGoals,
                previousState.AwayTeamGoals
            );
        }

        public static MatchState Score(MatchState previousState, Random random, double passedTimeInSeconds) {
            return new MatchState(
                previousState.match,
                previousState.DefendingTeam,
                previousState.DefendingTeam.GetRandomMidfielder(random),
                previousState.MatchTime + passedTimeInSeconds,
                previousState.AttackingTeam == previousState.HomeTeam ? previousState.HomeTeamGoals + 1 : previousState.HomeTeamGoals,
                previousState.AttackingTeam == previousState.AwayTeam ? previousState.AwayTeamGoals + 1 : previousState.AwayTeamGoals
            );
        }

        public MatchResult GetMatchResult() {
            return new MatchResult(match, HomeTeamGoals, AwayTeamGoals);
        }
    }
}
