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

        public static MatchState CreateFromKickOff(Match match, SoccerTeam attackingTeam, Random random) {
            return new MatchState(
                match, 
                attackingTeam, 
                attackingTeam.GetRandomMidfielder(random), 
                0.0, 
                0, 
                0
            );
        }

        public MatchState Halftime(SoccerTeam attackingTeam, Random random, double halfTime = 45.0 * 60.0) {
            return new MatchState(
                match,
                attackingTeam,
                attackingTeam.GetRandomMidfielder(random),
                halfTime,
                HomeTeamGoals,
                AwayTeamGoals
            );
        }

        public MatchState ActionProgress(SoccerTeam teamWithBallPossesion, SoccerPlayer playerWithBallPosession, double passedTimeInSeconds) {
            return new MatchState(
                match, 
                teamWithBallPossesion,
                playerWithBallPosession, 
                MatchTime + passedTimeInSeconds,
                HomeTeamGoals,
                AwayTeamGoals
            );
        }

        public MatchState Score(Random random, double passedTimeInSeconds) {
            return new MatchState(
                match,
                DefendingTeam,
                DefendingTeam.GetRandomMidfielder(random),
                MatchTime + passedTimeInSeconds,
                AttackingTeam == HomeTeam ? HomeTeamGoals + 1 : HomeTeamGoals,
                AttackingTeam == AwayTeam ? AwayTeamGoals + 1 : AwayTeamGoals
            );
        }

        public MatchResult GetMatchResult() {
            return new MatchResult(match, HomeTeamGoals, AwayTeamGoals);
        }
    }
}
