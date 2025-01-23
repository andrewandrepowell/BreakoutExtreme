using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace BreakoutExtreme.Components
{
    public class Ball
    {
        private static readonly CircleF _bounds = new CircleF(Vector2.Zero, 8);
        private Animater _animater;
        public Collider _collider;
        public Animater GetAnimater() => _animater;
        public Collider GetCollider() => _collider;
        public Ball()
        {
            _animater = new();
            _animater.Play(Animater.Animations.Ball);
            _collider = new(_bounds, this);
        }

        public void Update()
        {
        }
    }
}
