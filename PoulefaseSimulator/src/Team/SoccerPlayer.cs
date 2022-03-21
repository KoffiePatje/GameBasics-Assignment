using Newtonsoft.Json;
using System;

namespace PouleSimulator
{
    public class SoccerPlayer
    {
        public string Name { get; }
        public ESoccerPlayerPosition Position { get; }
        public double OffensiveSkillIndex { get; }
        public double DefensiveSkillIndex { get; }

        public SoccerPlayer(string name, ESoccerPlayerPosition position, double offensiveSkillIndex, double defensiveSkillIndex) {
            this.Name = name;
            this.Position = position;
            this.OffensiveSkillIndex = offensiveSkillIndex;
            this.DefensiveSkillIndex = defensiveSkillIndex;
        }

        public override string ToString() {
            return $"{Name} [Position: {Position}, Offensive: {Math.Round(OffensiveSkillIndex * 100.0, 1)} Defensive: {Math.Round(DefensiveSkillIndex * 100.0, 1)}]";
        }
    }
}
