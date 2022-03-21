namespace PouleSimulator
{
    /// <summary>
    /// Interface used for a match distribution strategy implementation
    /// </summary>
    public interface IMatchDistributionStrategy
    {
        /// <summary>
        /// Returns a list of <see cref="Match"/> objects that should be played for the current tournament distribution
        /// </summary>
        Match[] CreateMatches(SoccerTeam[] teams);
    }
}
