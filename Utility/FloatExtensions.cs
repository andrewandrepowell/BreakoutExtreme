using System;

namespace BreakoutExtreme.Utility
{
    public static class FloatExtensions
    {
        //https://roundwide.com/equality-comparison-of-floating-point-numbers-in-csharp/
        public static bool EqualsWithTolerance(this float x, float y, float tolerance = 1e-5f)
        {
            var diff = Math.Abs(x - y);
            return diff <= tolerance ||
                   diff <= Math.Max(Math.Abs(x), Math.Abs(y)) * tolerance;
        }
    }
}
