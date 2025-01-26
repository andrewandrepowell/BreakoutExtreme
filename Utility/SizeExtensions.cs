using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace BreakoutExtreme.Utility
{
    public static class SizeExtensions
    {
        public static SizeF ToBounds(this Size size)
        {
            return new SizeF(size.Width * Globals.GameBlockSize, size.Height * Globals.GameBlockSize);
        }
        public static Vector2 ToVector2(this Size size)
        {
            return new Vector2(size.Width, size.Height);
        }
    }
}
