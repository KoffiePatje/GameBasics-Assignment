using System;

namespace PouleSimulator
{
    public class PassForwardPlayerAction : IPlayerAction
    {
        public double ActionDurationInSeconds => tweakConfig.PassForwardDurationInSeconds;

        private readonly TweakConfig tweakConfig;
        private readonly Random random;

        public PassForwardPlayerAction(Random random, TweakConfig tweakConfig) {
            this.random = random ?? throw new ArgumentNullException(nameof(random));
            this.tweakConfig = tweakConfig ?? throw new ArgumentNullException(nameof(tweakConfig));
        }

        public bool CanPerformAction(SoccerPlayer player) {
            return player.Position != ESoccerPlayerPosition.Attacker;
        }

        public double GetActionScore(ref MatchState matchState) {
            switch(matchState.PlayerWithBallPossesion.Position) {
                case ESoccerPlayerPosition.Midfielder: return tweakConfig.PassForwardAsMidfielderScore;
                case ESoccerPlayerPosition.Defender: return tweakConfig.PassForwardAsDefenderScore;
                case ESoccerPlayerPosition.Goalkeeper: return tweakConfig.PassForwardAsGoalkeeperScore;
                default: throw new NotImplementedException(matchState.PlayerWithBallPossesion.Position.ToString());
            }
        }

        public PlayerActionResult Execute(ref MatchState matchState) {
            SoccerPlayer ballPlayer = matchState.PlayerWithBallPossesion;
            ESoccerPlayerPosition passedToPlayerPosition = ballPlayer.Position.Next(false);
            SoccerPlayer interceptingPlayer = matchState.DefendingTeam.GetRandomOpposingPlayer(random, passedToPlayerPosition);

            // The defensive player will attempt a 'intercept', let's see if he/she succeeds
            double ballPlayerSkill = ballPlayer.OffensiveSkillIndex * tweakConfig.PassForwardOffensiveSkillModifier;
            double interceptingPlayerSkill = interceptingPlayer.DefensiveSkillIndex * tweakConfig.PassForwardDefensiveSkillModifier;
            double totalSkill = ballPlayerSkill + interceptingPlayerSkill;

            bool passSucceeded = random.NextDouble() * totalSkill < ballPlayerSkill;

            if(!passSucceeded)
                return new PlayerActionResult(interceptingPlayer, false);

            SoccerPlayer passedToPlayer = matchState.AttackingTeam.GetRandomPlayer(random, passedToPlayerPosition);
            return new PlayerActionResult(passedToPlayer, true);
        }

        private ESoccerPlayerPosition GetOpposingPlayerPosition(ESoccerPlayerPosition playerPosition) {
            switch(playerPosition) {
                case ESoccerPlayerPosition.Attacker: return ESoccerPlayerPosition.Defender;
                case ESoccerPlayerPosition.Midfielder: return ESoccerPlayerPosition.Midfielder;
                case ESoccerPlayerPosition.Defender: return ESoccerPlayerPosition.Attacker;
                case ESoccerPlayerPosition.Goalkeeper: return ESoccerPlayerPosition.Attacker;
                default: throw new NotImplementedException();
            }
        }
    }
}
