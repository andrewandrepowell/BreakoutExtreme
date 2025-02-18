using BreakoutExtreme.Components;
using BreakoutExtreme.Shaders;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Diagnostics;

namespace BreakoutExtreme.Features
{
    public class Pulse : Feature
    {
        private float _period = 1;
        private float _halfPeriod = 0.5f;
        private float _time;
        private float _minVisibility = 0.25f;
        private float _maxVisibility = 0.75f;
        private bool _brightening = true;
        private bool _running = false;
        private bool _repeating = true;
        private bool _startDark = true;
        public bool StartDark
        {
            get => _startDark;
            set
            {
                Debug.Assert(!_running);
                _startDark = value;
            }
        }
        public float MaxVisibility
        {
            get => _maxVisibility;
            set
            {
                Debug.Assert(!_running);
                _maxVisibility = value;
            }
        }
        public float MinVisibility
        {
            get => _minVisibility;
            set
            {
                Debug.Assert(!_running);
                _minVisibility = value;
            }
        }
        public float Period
        {
            get => _period;
            set
            {
                Debug.Assert(!_running);
                Debug.Assert(_period >= 0);
                _period = value;
                _halfPeriod = value / 2;
            }
        }
        public bool Repeating
        {
            get => _repeating;
            set
            {
                Debug.Assert(!_running);
                _repeating = value;
            }
        }
        public bool Running => _running;
        public override bool UpdateVisibility(ref float visibility)
        {
            if (!_running)
                return false;

            if (_brightening)
                visibility *= MathHelper.Lerp(_maxVisibility, _minVisibility, MathHelper.Max(0, _time) / _halfPeriod);
            else
                visibility *= MathHelper.Lerp(_minVisibility, _maxVisibility, MathHelper.Max(0, _time) / _halfPeriod);

            return true;
        }
        public void Start()
        {
            Debug.Assert(_maxVisibility >= _minVisibility);
            Debug.Assert(_maxVisibility <= 1);
            Debug.Assert(_maxVisibility >= _minVisibility);
            Debug.Assert(_minVisibility >= 0);
            _time = _halfPeriod;
            _brightening = _startDark;
            _running = true;
        }
        public void Stop()
        {
            _running = false;
        }
        public override void Update()
        {
            if (!_running)
                return;

            while (_time <= 0)
            {
                _brightening = !_brightening;
                if (!_repeating &&  _brightening == _startDark)
                {
                    _running = false;
                    return;
                }
                else
                {
                    _time += _halfPeriod;
                }
            }

            _time -= Globals.GameTime.GetElapsedSeconds();
        }
    }
}
