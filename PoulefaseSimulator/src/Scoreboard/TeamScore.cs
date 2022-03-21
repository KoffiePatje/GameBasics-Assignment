namespace PouleSimulator
{
    public struct TeamScore
    {
        public SoccerTeam Team { get; set; }

        public double AverageOffensiveSkillIndex { get; set; }
        public double AverageDefensiveSkillIndex { get; set; }

        public int Points { get; set; }
        public int NumberOfWins { get; set; }
        public int NumberOfDraws { get; set; }
        public int NumberOfLosses { get; set; }
        public int GoalDifferential { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsReceived { get; set; }
    }
}
