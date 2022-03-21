using System;

namespace PouleSimulator
{
    /// <summary>
    /// Factory object responsible for the initialization of <see cref="IMatchDistributionStrategy"/>
    /// </summary>
    public class MatchDistributionFactory
    {
        private readonly Random random;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchDistributionFactory"/> class.
        /// </summary>
        public MatchDistributionFactory(Random random = null) {
            this.random = random ?? new Random();
        }

        /// <summary>
        /// Returns a new instance of an <see cref="IMatchDistributionStrategy"/> for the given <see cref="EGroupMatchDistributionStrategy"/>
        /// </summary>
        public IMatchDistributionStrategy CreateMatchDistributionStrategy(EGroupMatchDistributionStrategy strategy) {
            switch(strategy) {
                case EGroupMatchDistributionStrategy.RoundRobin: return new RoundRobinGroupMatchDistributionStrategy(random);
                case EGroupMatchDistributionStrategy.DoubleRoundRobin: return new DoubleRoundRobinGroupMatchDistributionStrategy();
                default: throw new NotImplementedException(strategy.ToString());
            }
        }
    }
}
