using System;

namespace PouleSimulator
{
    public class ShootAtGoalPlayerAction : IPlayerAction
    {
        public double ActionDurationInSeconds => tweakConfig.ShootAtGoalDurationInSeconds;

        private readonly TweakConfig tweakConfig;
        private readonly Random random;

        public ShootAtGoalPlayerAction(Random random, TweakConfig tweakConfig) {
            this.random = random ?? throw new ArgumentNullException(nameof(random));
            this.tweakConfig = tweakConfig ?? throw new ArgumentNullException(nameof(tweakConfig));
        }

        public bool CanPerformAction(SoccerPlayer player) {
            return true;
        }

        public double GetActionScore(ref MatchState matchState) {
            switch(matchState.PlayerWithBallPossesion.Position) {
                case ESoccerPlayerPosition.Attacker: return tweakConfig.ShootAtGoalAsAttackerScore;
                case ESoccerPlayerPosition.Midfielder: return tweakConfig.ShootAtGoalAsMidfielderScore;
                case ESoccerPlayerPosition.Defender: return tweakConfig.ShootAtGoalAsDefenderScore;
                case ESoccerPlayerPosition.Goalkeeper: return tweakConfig.ShootAtGoalAsGoalkeeperScore;
                default: throw new NotImplementedException(matchState.PlayerWithBallPossesion.Position.ToString());
            }
        }

        public PlayerActionResult Execute(ref MatchState matchState) {
            SoccerPlayer shootingPlayer = matchState.PlayerWithBallPossesion;
            SoccerPlayer defendingGoalKeeper = matchState.DefendingTeam.GoalKeeper;

            // The defensive player will attempt a 'intercept', let's see if he/she succeeds
            double shootingPlayerSkill = shootingPlayer.OffensiveSkillIndex * tweakConfig.ShootAtGoalOffensiveSkillModifier;
            double goalkeeperPlayerSkill = defendingGoalKeeper.DefensiveSkillIndex * tweakConfig.ShootAtGoalDefensiveSkillModifier;
            double totalSkill = shootingPlayerSkill + goalkeeperPlayerSkill;

            bool success = random.NextDouble() * totalSkill < shootingPlayerSkill;

            if(success) {
                return new PlayerActionResult(shootingPlayer, true, true);
            } else {
                return new PlayerActionResult(defendingGoalKeeper, false, false);
            }
        }
    }
}
