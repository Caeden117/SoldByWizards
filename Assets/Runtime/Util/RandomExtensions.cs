using System;

namespace SoldByWizards.Util
{
    public static class RandomExtensions
    {
        public static float NextFloatRange(this Random random, float minimum, float maximum)
        {
            return (float)random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
