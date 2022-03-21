using System.Collections.Generic;

namespace PouleSimulator
{
    public struct Statistics
    {
        public IReadOnlyList<TeamStatistics> StatisticsPerTeam { get; }

        public Statistics(TeamStatistics[] statistics) {
            this.StatisticsPerTeam = statistics;
        }
    }
}
