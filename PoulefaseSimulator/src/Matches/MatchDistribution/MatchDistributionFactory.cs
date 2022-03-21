using System;

namespace PouleSimulator
{
    public class MatchDistributionFactory
    {
        private readonly Random random;

        public MatchDistributionFactory(Random random = null) {
            this.random = random ?? new Random();
        }

        public IMatchDistributionStrategy CreateMatchDistributionStrategy(EGroupMatchDistributionStrategy strategy) {
            switch(strategy) {
                case EGroupMatchDistributionStrategy.RoundRobin: return new RoundRobinGroupMatchDistributionStrategy(random);
                case EGroupMatchDistributionStrategy.DoubleRoundRobin: return new DoubleRoundRobinGroupMatchDistributionStrategy();
                default: throw new NotImplementedException(strategy.ToString());
            }
        }
    }
}
