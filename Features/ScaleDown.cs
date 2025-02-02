using BreakoutExtreme.Shaders;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Diagnostics;

namespace BreakoutExtreme.Features
{
    public class ScaleDown : Feature
    {
        private float _maxScale = 1.25f;
        private float _minScale = 1f;
        private float _period = 1;
        private float _delayPeriod = 0;
        private float _delayTime = 0;
        private float _time;
        private bool _running = false;
        public bool Running => _running;
        public float MaxScale
        {
            get => _maxScale;
            set
            {
                Debug.Assert(!_running);
                Debug.Assert(value >= _minScale);
                _maxScale = value;
            }
        }
        public float MinScale
        {
            get => _minScale;
            set
            {
                Debug.Assert(!_running);
                Debug.Assert(value >= 0);
                Debug.Assert(value <= _maxScale);
                _minScale = value;
            }
        }
        public float Period
        {
            get => _period;
            set
            {
                Debug.Assert(!_running);
                Debug.Assert(value >= 0);
                _period = value;
            }
        }
        public float DelayPeriod
        {
            get => _delayPeriod;
            set
            {
                Debug.Assert(!_running);
                Debug.Assert(value >= 0);
                _delayPeriod = value;
            }
        }
        public void Start()
        {
            _time = _period;
            _delayTime = _delayPeriod;
            _running = true;
        }
        public void Stop()
        {
            _running = false;
        }
        public override bool UpdateScale(ref float scale)
        {
            if (!_running)
                return false;
            scale *= MathHelper.Lerp(_minScale, _maxScale, _time / _period);
            return true;
        }
        public override void Update()
        {
            if (!_running)
                return;

            if (_delayTime <= 0 && _time <= 0)
            {
                _time = 0;
                _running = false;
                return;
            }

            var timeElapsed = Globals.GameTime.GetElapsedSeconds();
            if (_time > 0 && _delayTime <= 0)
                _time -= timeElapsed;
            if (_delayTime > 0)
                _delayTime -= timeElapsed;
        }
    }
}
