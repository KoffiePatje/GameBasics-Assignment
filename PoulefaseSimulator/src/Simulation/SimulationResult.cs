namespace PouleSimulator
{
    public struct SimulationResult
    {
        public MatchResult[] Results { get; }

        public SimulationResult(MatchResult[] results) {
            this.Results = results;
        }
    }
}
