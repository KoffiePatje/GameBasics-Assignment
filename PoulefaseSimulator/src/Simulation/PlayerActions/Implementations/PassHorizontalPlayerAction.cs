using System;

namespace PouleSimulator
{
    public class PassHorizontalPlayerAction : IPlayerAction
    {
        public double ActionDurationInSeconds => tweakConfig.PassHorizontalDurationInSeconds;

        private readonly TweakConfig tweakConfig;
        private readonly Random random;

        public PassHorizontalPlayerAction(Random random, TweakConfig tweakConfig) {
            this.random = random ?? throw new ArgumentNullException(nameof(random));
            this.tweakConfig = tweakConfig ?? throw new ArgumentNullException(nameof(tweakConfig));
        }

        public bool CanPerformAction(SoccerPlayer player) {
            return player.Position != ESoccerPlayerPosition.Goalkeeper;
        }

        public double GetActionScore(ref MatchState matchState) {
            switch(matchState.PlayerWithBallPossesion.Position) {
                case ESoccerPlayerPosition.Attacker: return tweakConfig.PassHorizontalAsAttackerScore;
                case ESoccerPlayerPosition.Midfielder: return tweakConfig.PassHorizontalAsMidfielderScore;
                case ESoccerPlayerPosition.Defender: return tweakConfig.PassHorizontalAsDefenderScore;
                default: throw new NotImplementedException(matchState.PlayerWithBallPossesion.Position.ToString());
            }
        }

        public PlayerActionResult Execute(ref MatchState matchState) {
            SoccerPlayer ballPlayer = matchState.PlayerWithBallPossesion;
            SoccerPlayer interceptingPlayer = matchState.DefendingTeam.GetRandomOpposingPlayer(random, ballPlayer.Position);

            // The defensive player will attempt a 'intercept', let's see if he/she succeeds
            double ballPlayerSkill = ballPlayer.OffensiveSkillIndex * tweakConfig.PassHorizontalOffensiveSkillModifier;
            double interceptingPlayerSkill = interceptingPlayer.DefensiveSkillIndex * tweakConfig.PassHorizontalDefensiveSkillModifier;
            double totalSkill = ballPlayerSkill + interceptingPlayerSkill;

            bool passSucceeded = random.NextDouble() * totalSkill > ballPlayerSkill;

            if(!passSucceeded)
                return new PlayerActionResult(interceptingPlayer, false);

            SoccerPlayer passedToPlayer = matchState.AttackingTeam.GetRandomPlayer(random, ballPlayer.Position);
            return new PlayerActionResult(passedToPlayer, true);
        }
    }
}
