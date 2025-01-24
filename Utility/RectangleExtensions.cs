using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace BreakoutExtreme.Utility
{
    public static class RectangleExtensions
    {
        public static RectangleF ToBounds(this Rectangle rect)
        {
            return new RectangleF(
                rect.X * Globals.GameBlockSize, 
                rect.Y * Globals.GameBlockSize, 
                rect.Width * Globals.GameBlockSize, 
                rect.Height * Globals.GameBlockSize);
        }
    }
}
