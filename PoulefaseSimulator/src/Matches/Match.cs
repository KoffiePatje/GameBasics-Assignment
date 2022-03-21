namespace PouleSimulator
{
    /// <summary>
    /// Represents a match to be played between 2 teams in a competition.
    /// </summary>
    public struct Match
    {
        /// <summary>
        /// The team playing in a Home environment 
        /// </summary>
        public SoccerTeam Home { get; }

        /// <summary>
        /// The team playing in an Away environment
        /// </summary>
        public SoccerTeam Away { get; }

        /// <summary>
        /// Initializes an instance of a <see cref="Match>"/> struct
        /// </summary>
        public Match(SoccerTeam homeTeam, SoccerTeam awayTeam) {
            this.Home = homeTeam;
            this.Away = awayTeam;
        }

        /// <summary>
        /// Returns a human readable representation of the match in the form of 'x vs. y'
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return $"'{Home.Name}' vs. '{Away.Name}'";
        }
    }
}
