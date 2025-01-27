using BreakoutExtreme.Shaders;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace BreakoutExtreme.Features
{
    public class Vanish : Feature
    {
        private const float _period = 1;
        private float _time = 0;
        public override bool UpdateVisibility(ref float visibility)
        {
            if (!Running)
                return false;
            if (_time < 0)
            {
                _time = 0;
                Running = false;
            }    
            visibility *= (_time / _period);
            return true;
        }
        public bool Running { get; private set; } = false;
        public void Start()
        {
            _time = _period;
            Running = true;
        }
        public void Stop()
        {
            _time = 0;
            Running = false;
        }
        public override void Update()
        {
            if (Running)
            {
                _time -= Globals.GameTime.GetElapsedSeconds();
            }
        }
    }
}
