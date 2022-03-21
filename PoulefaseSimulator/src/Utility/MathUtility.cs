using System;

namespace PouleSimulator
{
    public static class MathUtility
    {
        /// <summary>
        /// Source: https://www.desmos.com/calculator/aksjkh9das
        /// </summary>
        /// <returns></returns>
        public static double NormalizedTunableSigmoidFunction(double x, double k) {
            return (x - x * k) / (k - (Math.Abs(x) * 2.0 * k) + 1);
        }

        /// <summary>
        /// Linearly interpolates between to numbers
        /// </summary>
        /// <param name="start">Start Value (returned at t=0)</param>
        /// <param name="end">End Value (returned at t=1)</param>
        /// <param name="percentage">Value from 0 - 1</param>
        public static double Lerp(double start, double end, double percentage) {
            return Math.Clamp(start + ((end - start) * percentage), start, end); 
        }

        public static double LerpUnclamped(double start, double end, double percentage) {
            return start + ((end - start) * percentage);
        }

        /// <summary>
        /// Returns the percentage value between the start (a) and end (b)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double InverseLerp(double start, double end, double value) {
            return Math.Clamp((value - start) / (end - start), 0.0, 1.0);
        }

        public static double InverseLerpUnclamped(double start, double end, double value) {
            return (value - start) / (end - start);
        }
    }
}
