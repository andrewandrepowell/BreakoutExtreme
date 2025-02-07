using BreakoutExtreme.Shaders;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Diagnostics;

namespace BreakoutExtreme.Features
{
    public class Rock : Feature
    {
        private bool _rotateRight = true;
        private float _maxRotation = MathHelper.Pi / 8;
        private float _period = 4;
        private float _halfPeriod = 2;
        private float _time;
        private float _rotation;
        private bool _running = false;
        public bool Running => _running;
        public float Rotation
        {
            get => _rotation;
            set
            {
                Debug.Assert(value > 0);
                _rotation = value;
            }
        }
        public float Period
        {
            get => _period;
            set
            {
                Debug.Assert(value > 0);
                _period = value;
                _halfPeriod = value / 2;
            }
        }
        public void Start()
        {
            _rotateRight = false;
            _rotation = (_rotateRight ? -1 : 1) * _maxRotation;
            _time = _halfPeriod;
            _running = true;
        }
        public void Stop()
        {
            _running = false; 
        }
        public override bool UpdateRotation(ref float rotation)
        {
            if (!_running)
                return false;
            rotation += _rotation;
            return true;
        }
        public override void Update()
        {
            if (_running)
            {
                if (_rotateRight) 
                    _rotation = MathHelper.SmoothStep(_maxRotation, -_maxRotation, MathHelper.Max(_time, 0) / _halfPeriod);
                else
                    _rotation = MathHelper.SmoothStep(-_maxRotation, _maxRotation, MathHelper.Max(_time, 0) / _halfPeriod);

                while (_time <= 0)
                {
                    _rotateRight = !_rotateRight;
                    _time += _halfPeriod;
                }

                _time -= Globals.GameTime.GetElapsedSeconds();
            }
        }
    }
}
