using MonoGame.Extended;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public partial class Paddle
    {
        private class Empower(Paddle parent)
        {
            private readonly Paddle _parent = parent;
            private bool _running = false;
            private bool _initialized = false;
            private float _period = 15;
            private float _time;
            public bool Running => _running;
            public void Reset()
            {
                Debug.Assert(!_initialized);
                _running = false;
                _initialized = true;
            }
            public void RemoveEntity()
            {
                Debug.Assert(_initialized);
                if (_running) Stop();
                _initialized = false;
            }
            public void Start()
            {
                Debug.Assert(_initialized);
                _time = _period;
                _parent._particler.Play(Particler.Particles.Empowered);
                _parent._particler.Start();
                _parent._laserGlower.Empowered = true;
                _running = true;
            }
            public void Stop()
            {
                Debug.Assert(_initialized);
                _parent._particler.Stop();
                _parent._laserGlower.Empowered = false;
                _running = false;
            }
            public void Update()
            {
                if (_running)
                {
                    if (_time <= 0)
                        Stop();
                    else
                        _time -= Globals.GameTime.GetElapsedSeconds();
                }
            }
        }
    }
}
