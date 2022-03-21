using System.Collections.Generic;

namespace PouleSimulator
{
    public struct Scoreboard
    {
        public IReadOnlyList<TeamScore> Scores { get; }
        public IReadOnlyList<MatchResult> Matches { get; }

        public Scoreboard(TeamScore[] scores, MatchResult[] matches) {
            this.Scores = scores;
            this.Matches = matches;
        }
    }
}
