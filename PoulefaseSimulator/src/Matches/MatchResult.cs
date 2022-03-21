namespace PouleSimulator
{
    /// <summary>
    /// The result of played out match between two teams.
    /// </summary>
    public struct MatchResult
    {
        /// <summary>
        /// The simulated match
        /// </summary>
        public Match Match { get; }

        /// <summary>
        /// The number of goals scored by the 'Home' team
        /// </summary>
        public int GoalsScoredByHomeTeam { get; }

        /// <summary>
        /// The number of goals scored by the 'Away' team
        /// </summary>
        public int GoalsScoredByAwayTeam { get; }

        public MatchResult(Match match, int homeTeamGoals, int awayTeamGoals) {
            this.Match = match;
            this.GoalsScoredByHomeTeam = homeTeamGoals;
            this.GoalsScoredByAwayTeam = awayTeamGoals;
        }

        public override string ToString() {
            return $"{Match.Home.Name} vs. {Match.Away.Name} ({GoalsScoredByHomeTeam} - {GoalsScoredByAwayTeam})";
        }
    }
}
