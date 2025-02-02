using BreakoutExtreme.Shaders;
using MonoGame.Extended;
using System.Diagnostics;

namespace BreakoutExtreme.Features
{
    public class Vanish : Feature
    {
        private float _period = 1;
        private float _time = 0;
        public override bool UpdateVisibility(ref float visibility)
        {
            if (!Running)
                return false;
            visibility *= (_time / _period);
            return true;
        }
        public bool Running { get; private set; } = false;
        public float Period
        {
            get => _period;
            set
            {
                Debug.Assert(!Running);
                Debug.Assert(_period > 0);
                _period = value;
            }
        }
        public void Start()
        {
            _time = _period;
            Running = true;
        }
        public void Stop()
        {
            Running = false;
        }
        public override void Update()
        {
            if (Running)
            {
                if (_time <= 0)
                    Stop();
                _time -= Globals.GameTime.GetElapsedSeconds();
            }
        }
    }
}
