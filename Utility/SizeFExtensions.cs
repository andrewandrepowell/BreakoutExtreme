using MonoGame.Extended;
using System;

namespace BreakoutExtreme.Utility
{
    public static class SizeFExtensions
    {
        public enum RoundingModes { Floor, Round, Ceil }
        public static Size ToSize(this SizeF sizeF, RoundingModes mode = RoundingModes.Floor)
        {
            switch (mode)
            {
                default:
                case RoundingModes.Floor:
                    return new Size((int)sizeF.Width, (int)sizeF.Height);
                case RoundingModes.Round:
                    return new Size((int)Math.Round(sizeF.Width), (int)Math.Round(sizeF.Height));
                case RoundingModes.Ceil:
                    return new Size((int)Math.Ceiling(sizeF.Width), (int)Math.Round(sizeF.Height));
            }
        }
    }
}
