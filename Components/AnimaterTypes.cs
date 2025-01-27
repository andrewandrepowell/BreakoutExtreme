using MonoGame.Extended.Graphics;

namespace BreakoutExtreme.Components
{
    public partial class Animater
    {
        public enum Atlases
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
        private class Node(AnimatedSprite animatedSprite, SpriteSheet spriteSheet)
        {
            public readonly AnimatedSprite AnimatedSprite = animatedSprite;
            public readonly SpriteSheet SpriteSheet = spriteSheet;
        }
    }
}
