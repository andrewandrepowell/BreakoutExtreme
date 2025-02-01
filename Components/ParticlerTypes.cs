using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Profiles;
using Microsoft.Xna.Framework;
using System;
using MonoGame.Extended;

namespace BreakoutExtreme.Components
{
    public partial class Particler
    {
        public enum Particles
        {
            BallTrail,
            BrickBreak
        }
        class SprayRandomProfile : SprayProfile
        {
            private static FastRandom _random = new();
            public override void GetOffsetAndHeading(out Vector2 offset, out Vector2 heading)
            {
                float num = (float)Math.Atan2(Direction.Y, Direction.X);
                num = _random.NextSingle(num - Spread / 2f, num + Spread / 2f);
                offset = Vector2.Zero;
                heading = new Vector2((float)Math.Cos(num), (float)Math.Sin(num));
            }
        }
    }
}
