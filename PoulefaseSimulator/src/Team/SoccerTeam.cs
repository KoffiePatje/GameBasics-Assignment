using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace PouleSimulator
{
    public class SoccerTeam
    {
        public string Name { get; }
        public SoccerPlayer[] Players;

        public SoccerPlayer GoalKeeper { get; }
        public IReadOnlyList<SoccerPlayer> Attackers { get; }
        public IReadOnlyList<SoccerPlayer> Midfielders { get; }
        public IReadOnlyList<SoccerPlayer> Defenders { get; }

        public IEnumerable<SoccerPlayer> AllPlayers { get => GetAllTeamMembers(); }

        public SoccerTeam(string name, SoccerPlayer goalKeeper, SoccerPlayer[] defenders, SoccerPlayer[] midfielders, SoccerPlayer[] attackers) {
            this.Name = name;

            this.GoalKeeper = goalKeeper;
            this.Defenders = defenders;
            this.Midfielders = midfielders;
            this.Attackers = attackers;
        }

        public SoccerPlayer GetRandomAttacker(Random random) => GetRandomSoccerPlayer(random, Attackers);
        public SoccerPlayer GetRandomMidfielder(Random random) => GetRandomSoccerPlayer(random, Midfielders);
        public SoccerPlayer GetRandomDefender(Random random) => GetRandomSoccerPlayer(random, Defenders);

        public SoccerPlayer GetRandomPlayer(Random random, ESoccerPlayerPosition playerPosition) {
            switch(playerPosition) {
                case ESoccerPlayerPosition.Attacker: 
                    return GetRandomAttacker(random);
                case ESoccerPlayerPosition.Midfielder:
                    return GetRandomMidfielder(random);
                case ESoccerPlayerPosition.Defender:
                    return GetRandomDefender(random);
                case ESoccerPlayerPosition.Goalkeeper: 
                    return GoalKeeper;
                default: throw new NotImplementedException(playerPosition.ToString());
            }
        }

        public SoccerPlayer GetRandomOpposingPlayer(Random random, ESoccerPlayerPosition playerPosition) {
            switch(playerPosition) {
                case ESoccerPlayerPosition.Attacker:
                    return GetRandomDefender(random);
                case ESoccerPlayerPosition.Midfielder:
                    return GetRandomMidfielder(random);
                case ESoccerPlayerPosition.Defender:
                case ESoccerPlayerPosition.Goalkeeper:
                    return GetRandomAttacker(random);
                default: throw new NotImplementedException(playerPosition.ToString());
            }
        }

        private IEnumerable<SoccerPlayer> GetAllTeamMembers() {
            yield return GoalKeeper;

            for(int i = 0; i < Defenders.Count; i++) {
                yield return Defenders[i];
            }

            for(int i = 0; i < Midfielders.Count; i++) {
                yield return Midfielders[i];
            }

            for(int i = 0; i < Attackers.Count; i++) {
                yield return Attackers[i];
            }
        }

        public override string ToString() {
            return $"Team: {Name}\n\t{String.Join("\n\t", AllPlayers)}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static SoccerPlayer GetRandomSoccerPlayer(Random random, IReadOnlyList<SoccerPlayer> playerList) {
            return playerList[random.Next(0, playerList.Count)];
        }
    }
}
