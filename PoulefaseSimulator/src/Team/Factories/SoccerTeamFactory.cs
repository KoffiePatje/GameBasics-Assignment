using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PouleSimulator
{
    public class SoccerTeamFactory
    {
        private readonly RandomPlayerNameFactory playerNameFactory;
        private readonly RandomSoccerClubNameFactory clubNameFactory;
        private readonly Random random;

        public SoccerTeamFactory(Random random = null) {
            this.random = random ?? new Random();
            this.playerNameFactory = new RandomPlayerNameFactory(random);
            this.clubNameFactory = new RandomSoccerClubNameFactory(random);
        }

        public async Task<SoccerTeam[]> CreateRandomTeams(int teamCount) {
            SoccerTeam[] teams = new SoccerTeam[teamCount];
            string[] clubNames = clubNameFactory.CreateRandomSoccerTeamNames(teamCount);

            RandomPlayerNameFactory.RandomName[] playerNames = await playerNameFactory.CreateRandomPlayerNames(teamCount * TeamFormation.FormationPlayerCount);
            int playerNameIndex = 0;

            const double MaxTeamSkillDeviation = 0.2;

            for(int j = 0; j < teamCount; j++) {
                TeamFormation formation = TeamFormation.GetRandomTeamFormation(random);
                double teamSkillIndex = (random.NextDouble() * (1.0 - (2 * MaxTeamSkillDeviation))) + MaxTeamSkillDeviation;

                SoccerPlayer[] attackers = new SoccerPlayer[formation.AttackerCount];
                for(int i = 0; i < attackers.Length; i++) attackers[i] = CreateAttacker(playerNames[playerNameIndex++], teamSkillIndex, MaxTeamSkillDeviation);

                SoccerPlayer[] midfielders = new SoccerPlayer[formation.MidfielderCount];
                for(int i = 0; i < midfielders.Length; i++) midfielders[i] = CreateMidfielder(playerNames[playerNameIndex++], teamSkillIndex, MaxTeamSkillDeviation);

                SoccerPlayer[] defenders = new SoccerPlayer[formation.DefenderCount];
                for(int i = 0; i < defenders.Length; i++) defenders[i] = CreateDefender(playerNames[playerNameIndex++], teamSkillIndex, MaxTeamSkillDeviation);

                SoccerPlayer goalkeeper = CreateGoalkeeper(playerNames[playerNameIndex++], teamSkillIndex, MaxTeamSkillDeviation);

                teams[j] = new SoccerTeam(clubNames[j], goalkeeper, defenders, midfielders, attackers);
            }

            return teams;
        }

        /// <summary>
        /// Creates an 'Offensive' Player, this player's deviation is applied positively on the offensive side, and negatively on the defensive side
        /// </summary>
        private SoccerPlayer CreateAttacker(string name, double teamSkillIndex, double playerSkillDeviation) {
            return CreateSoccerPlayer(
                name,
                ESoccerPlayerPosition.Attacker,
                Math.Clamp(teamSkillIndex + (random.NextDouble() * playerSkillDeviation), Double.Epsilon, 1.0),
                Math.Clamp(teamSkillIndex - (random.NextDouble() * playerSkillDeviation), Double.Epsilon, 1.0)
            );
        }

        /// <summary>
        /// Creates a 'Midfield' player, this player's deviation is avaraged on both the offensive and defensive skill index
        /// </summary>
        private SoccerPlayer CreateMidfielder(string name, double teamSkillIndex, double playerSkillDeviation) {
            return CreateSoccerPlayer(
                name,
                ESoccerPlayerPosition.Midfielder,
                Math.Clamp(teamSkillIndex + (random.NextDouble() * playerSkillDeviation) - (0.5 * playerSkillDeviation), Double.Epsilon, 1.0),
                Math.Clamp(teamSkillIndex + (random.NextDouble() * playerSkillDeviation) - (0.5 * playerSkillDeviation), Double.Epsilon, 1.0)
            );
        }

        /// <summary>
        /// Creates a 'Defensive' Player, this player's deviation is applied positively on the defensive side, and negatively on the offensive side
        /// </summary>
        private SoccerPlayer CreateDefender(string name, double teamSkillIndex, double playerSkillDeviation) {
            return CreateSoccerPlayer(
                name,
                ESoccerPlayerPosition.Defender,
                teamSkillIndex - (random.NextDouble() * playerSkillDeviation),
                teamSkillIndex + (random.NextDouble() * playerSkillDeviation)
            );
        }

        /// <summary>
        /// Creates a Goal Keeper, this player's deviation is applied positively on the defensive side, and negatively on the offensive side
        /// </summary>
        private SoccerPlayer CreateGoalkeeper(string name, double teamSkillIndex, double playerSkillDeviation) {
            return CreateSoccerPlayer(
                name,
                ESoccerPlayerPosition.Goalkeeper,
                teamSkillIndex - (random.NextDouble() * playerSkillDeviation),
                teamSkillIndex + (random.NextDouble() * playerSkillDeviation)
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static SoccerPlayer CreateSoccerPlayer(string name, ESoccerPlayerPosition playerPosition, double attackSkillIndex, double defensiveSkillIndex) {
            return new SoccerPlayer(
                name,
                playerPosition,
                attackSkillIndex,
                defensiveSkillIndex
            );
        }
    }
}
