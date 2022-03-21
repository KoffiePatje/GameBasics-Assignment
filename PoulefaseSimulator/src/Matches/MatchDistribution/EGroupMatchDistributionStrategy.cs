namespace PouleSimulator
{
    /// <summary>
    /// Identifies the available options in <see cref="IMatchDistributionStrategy"/> implementations supported by the <see cref="MatchDistributionFactory"/>
    /// </summary>
    public enum EGroupMatchDistributionStrategy
    {
        /// <summary>
        /// Distributes matches so that each opponent faces another opponent only once
        /// </summary>
        RoundRobin,

        /// <summary>
        /// Distributes matches so that each opponent faces each other opponent twice, once in a 'Home' environment and once in an 'Away' environment
        /// </summary>
        DoubleRoundRobin
    }
}
