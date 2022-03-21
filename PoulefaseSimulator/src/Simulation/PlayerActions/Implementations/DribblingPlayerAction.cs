using System;

namespace PouleSimulator
{
    public class DribblingPlayerAction : IPlayerAction
    {
        public double ActionDurationInSeconds => tweakConfig.DribblingDurationInSeconds;

        private readonly TweakConfig tweakConfig;
        private readonly Random random;

        public DribblingPlayerAction(Random random, TweakConfig tweakConfig) {
            this.random = random ?? throw new ArgumentNullException(nameof(random));
            this.tweakConfig = tweakConfig ?? throw new ArgumentNullException(nameof(tweakConfig));
        }

        public bool CanPerformAction(SoccerPlayer player) {
            return true;
        }

        public double GetActionScore(ref MatchState matchState) {
            switch(matchState.PlayerWithBallPossesion.Position) {
                case ESoccerPlayerPosition.Attacker: return tweakConfig.DribblingAsAttackerScore;
                case ESoccerPlayerPosition.Midfielder: return tweakConfig.DribblingAsMidfielderScore;
                case ESoccerPlayerPosition.Defender: return tweakConfig.DribblingAsDefenderScore;
                case ESoccerPlayerPosition.Goalkeeper: return tweakConfig.DribblingAsGoalkeeperScore;
                default: throw new NotImplementedException(matchState.PlayerWithBallPossesion.Position.ToString());
            }
        }

        public PlayerActionResult Execute(ref MatchState matchState) {
            // Each 'Dribble' we allow a defensive player to perform an interception attempt
            SoccerPlayer dribblingPlayer = matchState.PlayerWithBallPossesion;
            SoccerPlayer interceptingPlayer = matchState.DefendingTeam.GetRandomOpposingPlayer(random, dribblingPlayer.Position);

            // The defensive player will attempt a 'intercept', let's see if he/she succeeds
            double dribblingPlayerSkill = dribblingPlayer.OffensiveSkillIndex * tweakConfig.DribblingOffensiveSkillModifier;
            double interceptingPlayerSkill = interceptingPlayer.DefensiveSkillIndex * tweakConfig.DribblingDefensiveSkillModifier;
            double totalSkill = dribblingPlayerSkill + interceptingPlayerSkill;

            bool success = (random.NextDouble() * totalSkill) < dribblingPlayerSkill;
            
            if(success) {
                return new PlayerActionResult(dribblingPlayer, true);
            } else {
                return new PlayerActionResult(interceptingPlayer, false);
            }
        }
    }
}
