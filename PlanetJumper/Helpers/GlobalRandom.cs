using System;

namespace PlanetJumper.Helpers
{
    /// <summary>
    /// Static class to act as global random number generator
    /// Incudes methods for random integers and doubles.
    /// </summary>
    public static class GlobalRandom
    {
        private static Random random = new Random();

        /// <summary>
        /// The instance of Random used by GlobalRandom
        /// </summary>
        public static Random Random { get { return random; } }

        /// <summary>
        /// Returns random integer in interger [0, upper bound[
        /// </summary>
        /// <param name="max">The exclusive upper bound</param>
        public static int Next(int max)
        {
            return random.Next(max);
        }
        /// <summary>
        /// Returns random integer in the interval [lower bound, upper bound[
        /// </summary>
        /// <param name="min">The inclusive lower bound</param>
        /// <param name="max">The exclusive upper bound</param>
        /// <returns></returns>
        public static int Next(int min, int max)
        {
            return random.Next(min, max);
        }
        /// <summary>
        /// Returns random double between 0 and 1
        /// </summary>
        /// <returns></returns>
        public static double NextDouble()
        {
            return random.NextDouble();
        }
        /// <summary>
        /// Returns a random double between min and max.
        /// </summary>
        /// <param name="min">The lower bound</param>
        /// <param name="max">The upper bound</param>
        /// <returns></returns>
        public static double NextDouble(double min, double max)
        {
            return NextDouble() * (max - min) + min;
        }
        /// <summary>
        /// Generates a random double using the standard normal distribution;
        /// </summary>
        /// <returns></returns>
        public static double NormalDouble()
        {
            // Box-Muller
            double u1 = NextDouble();
            double u2 = NextDouble();
            return Math.Sqrt(-2 * Math.Log(u1)) * Math.Cos(2 * Math.PI * u2);
        }
        /// <summary>
        /// Generates a random double using the normal distribution with the given mean and deviation.
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="deviation"></param>
        /// <returns></returns>
        public static double NormalDouble(double mean, double deviation)
        {
            return mean + deviation * NormalDouble();
        }
    }
}