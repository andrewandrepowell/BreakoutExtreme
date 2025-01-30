using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;

namespace BreakoutExtreme.Components
{
    public class Spike
    {
        private readonly Animater _animater;
        private readonly Features.FloatDown _floatDown;
        private bool _running = false;
        public Animater GetAnimater() => _animater;
        public bool Running => _running;
        public void Start()
        {
            _floatDown.Stop();
            _running = true;
        }
        public void Stop()
        {
            _floatDown.Start();
            _running = false;
        }
        public Spike(Vector2 position)
        {
            _animater = new();
            _floatDown = new();
            _floatDown.Period = 2;
            _floatDown.MinHeight = Globals.GameBlockSize;
            _floatDown.ForceStart();
            _animater.ShaderFeatures.Add(_floatDown);
            _animater.Play(Animater.Animations.Spike);
            _animater.Position = position;
        }
    }
}
