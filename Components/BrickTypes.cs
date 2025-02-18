using MonoGame.Extended;

namespace BreakoutExtreme.Components
{
    public partial class Brick
    {
        public enum Bricks
        {
            ThickBrick
        }
        public enum States
        {
            Spawning,
            Active,
            Destroying,
            Destroyed
        }
        private class BrickConfig(RectangleF bounds, Animater.Animations activeAnimation, Animater.Animations deadAnimation, int totalHP)
        {
            public readonly RectangleF Bounds = bounds;
            public readonly Animater.Animations ActiveAnimation = activeAnimation;
            public readonly Animater.Animations DeadAnimation = deadAnimation;
            public readonly int TotalHP = totalHP;
        }
    }
}
