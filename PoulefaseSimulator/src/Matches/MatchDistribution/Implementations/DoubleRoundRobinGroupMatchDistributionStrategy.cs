using System;

namespace PouleSimulator
{
    /// <summary>
    /// Match distribution strategy implementation that sets up matches so that each team faces eachother twice, once in a 'Home' environment and once in an 'Away' environment 
    /// </summary>
    public class DoubleRoundRobinGroupMatchDistributionStrategy : IMatchDistributionStrategy
    {
        /// <summary>
        /// Returns an array of matches wherin each team faces another team twice, once in a 'Home' environment, once in an 'Away' environment
        /// </summary>
        public Match[] CreateMatches(SoccerTeam[] teams) {
            int numberOfMatches = teams.Length * (teams.Length - 1);
            Match[] matches = new Match[numberOfMatches];

            int matchIndex = 0;
            for(int j = 0; j < teams.Length; j++) {
                for(int i = j + 1; i < teams.Length; i++) {
                    matches[matchIndex++] = new Match(teams[j], teams[i]);
                    matches[matchIndex++] = new Match(teams[i], teams[j]);
                }
            }
            
            if(matchIndex != matches.Length) throw new InvalidOperationException($"The {nameof(matchIndex)} did not equal the number of pre-allocated matches");
            return matches;
        }
    }
}
