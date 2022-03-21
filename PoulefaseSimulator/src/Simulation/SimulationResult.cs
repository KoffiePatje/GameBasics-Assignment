using System.Collections.Generic;

namespace PouleSimulator
{
    public struct SimulationResult
    {
        public IReadOnlyList<MatchResult> Results { get; }

        public SimulationResult(MatchResult[] results) {
            this.Results = results;
        }
    }
}
