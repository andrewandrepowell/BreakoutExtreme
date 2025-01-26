using MonoGame.Extended;

namespace BreakoutExtreme.Utility
{
    public static class SizeExtensions
    {
        public static SizeF ToBounds(this Size size)
        {
            return new SizeF(size.Width * Globals.GameBlockSize, size.Height * Globals.GameBlockSize);
        }
    }
}
