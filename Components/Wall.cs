using MonoGame.Extended;

namespace BreakoutExtreme.Components
{
    public class Wall
    {
        private Collider _collider;
        private Sounder _sounder;
        public Collider GetCollider() => _collider;
        public void RunBounceEffects()
        {
            _sounder.Play(Sounder.Sounds.Wall);
        }
        public Wall(RectangleF bounds)
        {
            _collider = new(bounds, this);
            _sounder = Globals.Runner.GetSounder();
        }
    }
}
