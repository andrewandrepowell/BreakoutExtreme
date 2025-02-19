using MonoGame.Extended;
using Microsoft.Xna.Framework;
using BreakoutExtreme.Utility;

namespace BreakoutExtreme.Components
{
    public partial class Brick
    {
        public enum Bricks
        {
            Power,
            Small,
            Large
        }
        public enum States
        {
            Spawning,
            Active,
            Destroying,
            Destroyed
        }
        private class BrickConfig(
            RectangleF bounds, 
            Animater.Animations activeAnimation, 
            Animater.Animations deadAnimation, 
            int totalHP, 
            Color? tint = null, 
            Color? glow = null)
        {
            public readonly RectangleF Bounds = bounds;
            public readonly Animater.Animations ActiveAnimation = activeAnimation;
            public readonly Animater.Animations DeadAnimation = deadAnimation;
            public readonly int TotalHP = totalHP;
            public readonly Color Tint = (tint.HasValue ? tint.Value : Color.White);
            public readonly Color? Glow = glow;
        }
    }
}
