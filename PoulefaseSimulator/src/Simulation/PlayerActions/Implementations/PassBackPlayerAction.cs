using System;

namespace PouleSimulator
{
    public class PassBackPlayerAction : IPlayerAction
    {
        public double ActionDurationInSeconds => tweakConfig.PassBackDurationInSeconds;

        private readonly TweakConfig tweakConfig;
        private readonly Random random;

        public PassBackPlayerAction(Random random, TweakConfig tweakConfig) {
            this.random = random ?? throw new ArgumentNullException(nameof(random));
            this.tweakConfig = tweakConfig ?? throw new ArgumentNullException(nameof(tweakConfig));
        }

        public bool CanPerformAction(SoccerPlayer player) {
            return player.Position != ESoccerPlayerPosition.Goalkeeper;
        }

        public double GetActionScore(ref MatchState matchState) {
            switch(matchState.PlayerWithBallPossesion.Position) {
                case ESoccerPlayerPosition.Attacker: return tweakConfig.PassBackAsAttackerScore;
                case ESoccerPlayerPosition.Midfielder: return tweakConfig.PassBackAsMidfielderScore;
                case ESoccerPlayerPosition.Defender: return tweakConfig.PassBackAsDefenderScore;
                default: throw new NotImplementedException(matchState.PlayerWithBallPossesion.Position.ToString());
            }
        }

        public PlayerActionResult Execute(ref MatchState matchState) {
            SoccerPlayer ballPlayer = matchState.PlayerWithBallPossesion;
            ESoccerPlayerPosition passedToPlayerPosition = ballPlayer.Position.Previous(false);
            SoccerPlayer interceptingPlayer = matchState.DefendingTeam.GetRandomOpposingPlayer(random, passedToPlayerPosition);

            // The defensive player will attempt a 'intercept', let's see if he/she succeeds
            double ballPlayerSkill = ballPlayer.OffensiveSkillIndex * tweakConfig.PassBackOffensiveSkillModifier;
            double interceptingPlayerSkill = interceptingPlayer.DefensiveSkillIndex * tweakConfig.PassBackDefensiveSkillModifier;
            double totalSkill = ballPlayerSkill + interceptingPlayerSkill;

            bool passSucceeded = random.NextDouble() * totalSkill < ballPlayerSkill;

            if(!passSucceeded)
                return new PlayerActionResult(interceptingPlayer, false);

            SoccerPlayer passedToPlayer = matchState.AttackingTeam.GetRandomPlayer(random, passedToPlayerPosition);
            return new PlayerActionResult(passedToPlayer, true);
        }
    }
}
