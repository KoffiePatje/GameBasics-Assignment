using System;

namespace PouleSimulator
{
    public class RomanNumeralUtility
    {
        private static readonly char[] romanValues = new char[] {
            'I', // 1
            'V', // 5
            'X', // 10
            'L', // 50,
            'C', // 100,
            'D', // 500,
            'M', // 1000
        };

        /// <summary>
        /// Converts 'num' to Roman Numeral notation, supports numbers in the range 1 >= x <= 3999
        /// </summary>
        public static string ToRomanNumerals(int num) {
            if(num <= 0 || num >= 4000) throw new ArgumentOutOfRangeException(nameof(num));

            string romanString = string.Empty;
            int romanIndex = 0;

            do {
                int value = num % 10;

                switch(value) {
                    case 1: romanString = $"{romanValues[romanIndex]}{romanString}"; break;
                    case 2: romanString = $"{romanValues[romanIndex]}{romanValues[romanIndex]}{romanString}"; break;
                    case 3: romanString = $"{romanValues[romanIndex]}{romanValues[romanIndex]}{romanValues[romanIndex]}{romanString}"; break;
                    case 4: romanString = $"{romanValues[romanIndex]}{romanValues[romanIndex + 1]}{romanString}"; break;
                    case 5: romanString = $"{romanValues[romanIndex + 1]}{romanString}"; break;
                    case 6: romanString = $"{romanValues[romanIndex + 1]}{romanValues[romanIndex]}{romanString}"; break;
                    case 7: romanString = $"{romanValues[romanIndex + 1]}{romanValues[romanIndex]}{romanValues[romanIndex]}{romanString}"; break;
                    case 8: romanString = $"{romanValues[romanIndex + 1]}{romanValues[romanIndex]}{romanValues[romanIndex]}{romanValues[romanIndex]}{romanString}"; break;
                    case 9: romanString = $"{romanValues[romanIndex]}{romanValues[romanIndex + 2]}{romanString}"; break;
                }

                num /= 10;
                romanIndex += 2;
            } while(num > 0);

            return romanString;
        }
    }
}
