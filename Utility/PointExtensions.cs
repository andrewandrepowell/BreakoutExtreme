using Microsoft.Xna.Framework;

namespace BreakoutExtreme.Utility
{
    public static class PointExtensions
    {
        public static Vector2 ToPosition(this Point point)
        {
            return new Vector2(point.X * Globals.GameBlockSize, point.Y * Globals.GameBlockSize);
        }
    }
}
