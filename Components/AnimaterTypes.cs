using MonoGame.Extended.Graphics;

namespace BreakoutExtreme.Components
{
    public partial class Animater
    {
        public enum Spriters
        {
            Ball,
            Paddle,
            BrickLarge,
            Cracks
        }
        public enum Animations
        {
            Ball,
            Paddle,
            BrickLarge,
            BrickLargeDead,
            CrackSmall,
            CrackMedium,
            CrackLarge
        }
        public enum Layers
        {
            Shadow,
            Ground
        }
    }
}
