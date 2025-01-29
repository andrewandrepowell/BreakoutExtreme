using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace BreakoutExtreme.Components
{
    public partial class Ball
    {
        private class Destroyer(Ball parent)
        {
            private const float _shakePeriod = 0.5f;
            private const float _vanishPeriod = 1f;
            private static readonly Color _flashColor = Color.Red * 0.5f;
            private readonly Ball _parent = parent;
            private bool _running = false;
            public bool Running => _running;
            public void Start()
            {
                Debug.Assert(!_parent._animater.Running);
                Debug.Assert(!_parent._vanish.Running);
                Debug.Assert(!_parent._shake.Running);
                Debug.Assert(!_parent._flash.Running);
                Debug.Assert(!_parent._shadow.VanishRunning);
                Debug.Assert(!_running);
                _parent._animater.Play(Animater.Animations.BallDead);
                _parent._vanish.Start(_vanishPeriod);
                _parent._shake.Start(_shakePeriod);
                _parent._flash.Start();
                _parent._shadow.VanishStart();
                _running = true;
            }
            public void Update()
            {
                if (_running && !_parent._vanish.Running && !_parent._shake.Running)
                {
                    _parent._flash.Stop();
                    _running = false;
                }
            }
        }
    }
}
