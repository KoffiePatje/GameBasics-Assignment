using System.Collections.Generic;

namespace PouleSimulator
{
    public struct Scoreboard
    {
        /// <summary>
        /// Ordered <see cref="TeamScore"/> for this board
        /// </summary>
        public IReadOnlyList<TeamScore> Scores { get; }

        /// <summary>
        /// Match results used to compute this <see cref="Scoreboard"/>
        /// </summary>
        public IReadOnlyList<MatchResult> Matches { get; }

        public Scoreboard(IReadOnlyList<TeamScore> scores, IReadOnlyList<MatchResult> matches) {
            this.Scores = scores;
            this.Matches = matches;
        }
    }
}
