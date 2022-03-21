using Newtonsoft.Json;
using System;

namespace PouleSimulator
{
    [Serializable]
    public class TweakConfig : IConfig
    {
        #region Dribbling
        [JsonProperty]
        public double DribblingAsAttackerScore { get; private set; } = 0.4;
        [JsonProperty]
        public double DribblingAsMidfielderScore { get; private set; } = 0.4;
        [JsonProperty]
        public double DribblingAsDefenderScore { get; private set; } = 0.4;
        [JsonProperty]
        public double DribblingAsGoalkeeperScore { get; private set; } = 0.1;

        [JsonProperty]
        public double DribblingOffensiveSkillModifier { get; private set; } = 1.0;
        [JsonProperty]
        public double DribblingDefensiveSkillModifier { get; private set; } = 0.5;

        [JsonProperty]
        public double DribblingDurationInSeconds { get; private set; } = 10.0;
        #endregion

        #region ShootAtGoal
        [JsonProperty]
        public double ShootAtGoalAsAttackerScore { get; private set; } = 1.0;
        [JsonProperty]
        public double ShootAtGoalAsMidfielderScore { get; private set; } = 0.25;
        [JsonProperty]
        public double ShootAtGoalAsDefenderScore { get; private set; } = 0.01;
        [JsonProperty]
        public double ShootAtGoalAsGoalkeeperScore { get; private set; } = 0.001;

        [JsonProperty]
        public double ShootAtGoalOffensiveSkillModifier { get; private set; } = 1.0;
        [JsonProperty]
        public double ShootAtGoalDefensiveSkillModifier { get; private set; } = 22.5;

        [JsonProperty]
        public double ShootAtGoalDurationInSeconds { get; private set; } = 5.0;
        #endregion

        #region Pass Back
        [JsonProperty]
        public double PassBackAsAttackerScore { get; private set; } = 0.3;
        [JsonProperty]
        public double PassBackAsMidfielderScore { get; private set; } = 0.4;
        [JsonProperty]
        public double PassBackAsDefenderScore { get; private set; } = 0.3;

        [JsonProperty]
        public double PassBackOffensiveSkillModifier { get; private set; } = 1.0;
        [JsonProperty]
        public double PassBackDefensiveSkillModifier { get; private set; } = 1.0;

        [JsonProperty]
        public double PassBackDurationInSeconds { get; private set; } = 5.0;
        #endregion

        #region Pass Forward
        [JsonProperty]
        public double PassForwardAsMidfielderScore { get; private set; } = 0.7;
        [JsonProperty]
        public double PassForwardAsDefenderScore { get; private set; } = 0.8;
        [JsonProperty]
        public double PassForwardAsGoalkeeperScore { get; private set; } = 0.9;

        [JsonProperty]
        public double PassForwardOffensiveSkillModifier { get; private set; } = 1.0;
        [JsonProperty]
        public double PassForwardDefensiveSkillModifier { get; private set; } = 1.0;

        [JsonProperty]
        public double PassForwardDurationInSeconds { get; private set; } = 5.0;
        #endregion

        #region Pass Horizontal
        [JsonProperty]
        public double PassHorizontalAsAttackerScore { get; private set; } = 0.5;
        [JsonProperty]
        public double PassHorizontalAsMidfielderScore { get; private set; } = 0.6;
        [JsonProperty]
        public double PassHorizontalAsDefenderScore { get; private set; } = 0.5;

        [JsonProperty]
        public double PassHorizontalOffensiveSkillModifier { get; private set; } = 1.0;
        [JsonProperty]
        public double PassHorizontalDefensiveSkillModifier { get; private set; } = 1.0;

        [JsonProperty]
        public double PassHorizontalDurationInSeconds { get; private set; } = 3.0;
        #endregion

        void IConfig.ProcessCommandLineArguments(string[] args) { }

        void IConfig.Validate() {
            Math.Max(0.0, DribblingAsAttackerScore);
            Math.Max(0.0, DribblingAsMidfielderScore);
            Math.Max(0.0, DribblingAsDefenderScore);
            Math.Max(0.0, DribblingAsGoalkeeperScore);
            Math.Max(0.0, DribblingOffensiveSkillModifier);
            Math.Max(0.0, DribblingDefensiveSkillModifier);
            Math.Max(0.0, DribblingDurationInSeconds);

            Math.Max(0.0, ShootAtGoalAsAttackerScore);
            Math.Max(0.0, ShootAtGoalAsMidfielderScore);
            Math.Max(0.0, ShootAtGoalAsDefenderScore);
            Math.Max(0.0, ShootAtGoalAsGoalkeeperScore);
            Math.Max(0.0, ShootAtGoalOffensiveSkillModifier);
            Math.Max(0.0, ShootAtGoalDefensiveSkillModifier);
            Math.Max(0.0, ShootAtGoalDurationInSeconds);

            Math.Max(0.0, PassBackAsAttackerScore);
            Math.Max(0.0, PassBackAsMidfielderScore);
            Math.Max(0.0, PassBackAsDefenderScore);
            Math.Max(0.0, PassBackOffensiveSkillModifier);
            Math.Max(0.0, PassBackDefensiveSkillModifier);
            Math.Max(0.0, PassBackDurationInSeconds);

            Math.Max(0.0, PassForwardAsMidfielderScore);
            Math.Max(0.0, PassForwardAsDefenderScore);
            Math.Max(0.0, PassForwardAsGoalkeeperScore);
            Math.Max(0.0, PassForwardOffensiveSkillModifier);
            Math.Max(0.0, PassForwardDefensiveSkillModifier);
            Math.Max(0.0, PassForwardDurationInSeconds);

            Math.Max(0.0, PassHorizontalAsAttackerScore);
            Math.Max(0.0, PassHorizontalAsMidfielderScore);
            Math.Max(0.0, PassHorizontalAsDefenderScore);
            Math.Max(0.0, PassHorizontalOffensiveSkillModifier);
            Math.Max(0.0, PassHorizontalDefensiveSkillModifier);
            Math.Max(0.0, PassHorizontalDurationInSeconds);
        }

        /// <summary>
        /// Returns a human readable JSON structure of the config
        /// </summary>
        public override string ToString() {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
