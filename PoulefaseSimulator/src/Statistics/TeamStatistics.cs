namespace PouleSimulator
{
    public struct TeamStatistics
    {
        public SoccerTeam Team { get; }
        public double AverageOffensiveSkillIndex { get; }
        public double AverageDefensiveSkillIndex { get; }

        public double AveragePosition { get; set; }
        public double AveragePoints { get; set; }
        
        public double AverageWins { get; set; }
        public double AverageDraws { get; set; }
        public double AverageLosses { get; set; }
        
        public double AverageGoalDifferential { get; set; }
        public double AverageGoalsScored { get; set; }
        public double AverageGoalsReceived { get; set; }

        public TeamStatistics(SoccerTeam team, double averageOffensiveSkillIndex, double averageDefensiveSkillIndex) {
            this.Team = team;
            this.AverageOffensiveSkillIndex = averageOffensiveSkillIndex;
            this.AverageDefensiveSkillIndex = averageDefensiveSkillIndex;

            this.AveragePosition = 0;
            this.AveragePoints = 0;

            this.AverageWins = 0;
            this.AverageDraws = 0;
            this.AverageLosses = 0;

            this.AverageGoalDifferential = 0;
            this.AverageGoalsScored = 0;
            this.AverageGoalsReceived = 0;
        }
    }
}
