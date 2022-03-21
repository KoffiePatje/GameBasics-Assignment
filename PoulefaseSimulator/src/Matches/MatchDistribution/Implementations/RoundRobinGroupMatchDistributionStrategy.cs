using System;

namespace PouleSimulator
{
    /// <summary>
    /// Match distribution strategy implementation that sets up matches so that each team faces eachoter just once
    /// </summary>
    public class RoundRobinGroupMatchDistributionStrategy : IMatchDistributionStrategy
    {
        private readonly Random random;

        /// <summary>
        /// Constructs a new instance of the <see cref="RoundRobinGroupMatchDistributionStrategy"/> class.
        /// </summary>
        public RoundRobinGroupMatchDistributionStrategy(Random random = null) {
            this.random = random ?? new Random();
        }

        /// <summary>
        /// Returns an array of matches wherin each team faces another team only once, wether a team plays 'Home' or 'Away' is decided at random
        /// </summary>
        public Match[] CreateMatches(SoccerTeam[] teams) {
            int numberOfMatches = (teams.Length * (teams.Length - 1)) / 2;
            Match[] matches = new Match[numberOfMatches];

            int matchIndex = 0;
            for(int j = 0; j < teams.Length; j++) {
                for(int i = j + 1; i < teams.Length; i++) {
                    // Determine if the match is played at home or away on a random roll
                    int diceRoll = random.Next(0, 2);

                    if(diceRoll == 0) {
                        matches[matchIndex++] = new Match(teams[i], teams[j]);
                    } else {
                        matches[matchIndex++] = new Match(teams[j], teams[i]);
                    }
                }
            }

            if(matchIndex != matches.Length) throw new InvalidOperationException($"The {nameof(matchIndex)} did not equal the number of pre-allocated matches");
            return matches;
        }
    }
}
