using System.Collections.Generic;

namespace PouleSimulator
{
    public struct Statistics
    {
        /// <summary>
        /// Ordered list of <see cref="Statistics"/> for each Team in this Simulation
        /// </summary>
        public IReadOnlyList<TeamStatistics> StatisticsPerTeam { get; }

        public Statistics(TeamStatistics[] statistics) {
            this.StatisticsPerTeam = statistics;
        }
    }
}
