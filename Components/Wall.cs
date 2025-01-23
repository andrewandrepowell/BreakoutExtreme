using MonoGame.Extended;

namespace BreakoutExtreme.Components
{
    public class Wall
    {
        private Collider _collider;
        public Collider GetCollider() => _collider;
        public Wall(RectangleF bounds)
        {
            _collider = new(bounds, this);
        }
    }
}
