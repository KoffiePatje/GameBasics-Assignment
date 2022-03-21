using System;

namespace PouleSimulator
{
    public struct TeamFormation
    {
        public const int FormationPlayerCount = 11;

        public static TeamFormation F4_3_3 { get; } = new TeamFormation(4, 3, 3);
        public static TeamFormation F5_3_2 { get; } = new TeamFormation(5, 3, 2);
        public static TeamFormation F4_5_1 { get; } = new TeamFormation(4, 5, 1);
        public static TeamFormation F5_2_3 { get; } = new TeamFormation(5, 2, 3);

        public const int GoalkeeperCount = 1;

        public int DefenderCount { get; }
        public int MidfielderCount { get; }
        public int AttackerCount { get; }

        public int TotalPlayers { get => AttackerCount + MidfielderCount + DefenderCount + GoalkeeperCount; }

        private TeamFormation(int defenderCount, int midfielderCount, int attackerCount) {
            if(defenderCount + midfielderCount + attackerCount + GoalkeeperCount != FormationPlayerCount) throw new InvalidOperationException($"Invalid {nameof(TeamFormation)}, a formation must contain exactly {FormationPlayerCount} players");

            this.DefenderCount = defenderCount;
            this.MidfielderCount = midfielderCount;
            this.AttackerCount = attackerCount;
        }

        public static TeamFormation GetRandomTeamFormation(Random random) {
            int randomFormationIndex = random.Next(0, 3);
            switch(randomFormationIndex) {
                case 0: return TeamFormation.F4_3_3;
                case 1: return TeamFormation.F5_3_2;
                case 2: return TeamFormation.F5_2_3;
                case 3: return TeamFormation.F4_5_1;
                default: throw new NotImplementedException(randomFormationIndex.ToString());
            }
        }
    }
}
