using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Particles;

namespace BreakoutExtreme.Components
{
    public partial class Particler
    {
        public enum Particles
        {
            BallTrail
        }
        public class Node(Texture2D texture, ParticleEffect effect)
        {
            public readonly Texture2D Texture = texture;
            public readonly ParticleEffect Effect = effect;
        }
    }
}
