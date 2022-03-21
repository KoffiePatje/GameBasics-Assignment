namespace PouleSimulator
{
    /// <summary>
    /// Interface used for a match distribution strategy implementation
    /// </summary>
    public interface IMatchDistributionStrategy
    {
        Match[] CreateMatches(SoccerTeam[] teams);
    }
}
