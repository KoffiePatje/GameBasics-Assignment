using System;

namespace PouleSimulator
{
    public static class MathUtility
    {
        /// <summary>
        /// Source: https://www.desmos.com/calculator/aksjkh9das
        /// </summary>
        public static double NormalizedTunableSigmoidFunction(double x, double k) {
            return (x - x * k) / (k - (Math.Abs(x) * 2.0 * k) + 1);
        }

        public static double Lerp(double start, double end, double percentage) {
            return Math.Clamp(start + ((end - start) * percentage), start, end); 
        }

        public static double LerpUnclamped(double start, double end, double percentage) {
            return start + ((end - start) * percentage);
        }

        public static double InverseLerp(double start, double end, double value) {
            return Math.Clamp((value - start) / (end - start), 0.0, 1.0);
        }

        public static double InverseLerpUnclamped(double start, double end, double value) {
            return (value - start) / (end - start);
        }
    }
}
