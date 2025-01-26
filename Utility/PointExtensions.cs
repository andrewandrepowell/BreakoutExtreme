using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace BreakoutExtreme.Utility
{
    public static class PointExtensions
    {
        public static Vector2 ToPosition(this Point point)
        {
            return new Vector2(point.X * Globals.GameBlockSize, point.Y * Globals.GameBlockSize);
        }
        public static Size ToSize(this Point point)
        {
            return new Size(point.X, point.Y);
        }
    }
}
