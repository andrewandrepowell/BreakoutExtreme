using System.Diagnostics;


namespace BreakoutExtreme.Components
{
    public partial class Ball
    {
        private class Destroyer(Ball parent)
        {
            private const float _shakePeriod = 0.5f;
            private readonly Ball _parent = parent;
            private bool _running = false;
            public bool Running => _running;
            public void Start()
            {
                Debug.Assert(_parent._initialized);
                Debug.Assert(!_running);
                _parent._animater.Play(Animater.Animations.BallDead);
                _parent._vanish.Start();
                _parent._shake.Period = _shakePeriod;
                _parent._shake.Start();
                _parent._flash.Start();
                _parent._shadow.Start();
                _running = true;
            }
            public void Update()
            {
                Debug.Assert(_parent._initialized);
                if (_running && !_parent._vanish.Running && !_parent._shake.Running)
                {
                    _parent._flash.Stop();
                    _running = false;
                }
            }
        }
    }
}
