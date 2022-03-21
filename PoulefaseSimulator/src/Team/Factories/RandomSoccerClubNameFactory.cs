using System;
using System.Collections.Generic;
using System.Linq;

namespace PouleSimulator
{
    public class RandomSoccerClubNameFactory
    {
        private static readonly string[] prefixDataSet = new string[] { "FC", "SC", "RKC" };
        private static readonly string[] postfixDataSet = new string[] { "City", "United" };

        private static readonly string[] cityNameDataSet = new string[] {
            "Rotterdam",
            "Den Haag",
            "Utrecht",
            "Eindhoven",
            "Groningen",
            "Tilburg",
            "Almere",
            "Breda",
            "Nijmegen",
            "Apeldoorn",
            "Arnhem",
            "Haarlem",
            "Enschede",
            "Haarlemmermeer",
            "Amersfoort",
            "Zaanstad",
            "'s-Hertogenbosch",
            "Zwolle",
            "Zoetermeer",
            "Leeuwarden",
            "Leiden",
            "Maastricht",
            "Ede",
            "Dordrecht",
            "Alphen aan den Rijn",
            "Westland",
            "Alkmaar",
            "Emmen",
            "Delft",
            "Venlo",
            "Deventer"
        };

        private readonly Random random;

        public RandomSoccerClubNameFactory(Random random = null) {
            this.random = random ?? new Random();
        }

        public string[] CreateRandomSoccerTeamNames(int count) {
            HashSet<string> names = new HashSet<string>(count);

            for(int i = 0; i < count; i++) {
                string randomTeamName = GetRandomTeamName();
                if(!names.Add(randomTeamName)) {
                    names.Add($"{randomTeamName} {RomanNumeralUtility.ToRomanNumerals(i + 1)}");
                }
            }

            return names.ToArray();
        }

        private string GetRandomTeamName() {
            int diceRoll = random.Next(0, 3);

            switch(diceRoll) {
                case 0: return $"{GetRandomCity()} {GetRandomPostfix()}";
                case 1: return $"{GetRandomPrefix()} {GetRandomCity()}";
                case 2: return $"{GetRandomPrefix()} {GetRandomCity()} {GetRandomPostfix()}";
                default: throw new NotImplementedException(diceRoll.ToString());
            }
        }

        private string GetRandomPrefix() {
            return prefixDataSet[random.Next(0, prefixDataSet.Length)];
        }

        private string GetRandomPostfix() {
            return postfixDataSet[random.Next(0, postfixDataSet.Length)];
        }

        private string GetRandomCity() {
            return cityNameDataSet[random.Next(0, cityNameDataSet.Length)];
        }
    }
}
