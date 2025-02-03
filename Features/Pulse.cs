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
        private float _time;
        private float _minVisibility = 0.25f;
        private float _maxVisibility = 0.75f;
        private bool _brightening = true;
        private bool _running = false;
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
            }
        }
        public override bool UpdateVisibility(ref float visibility)
        {
            if (!_running)
                return false;

            if (_brightening)
                visibility *= MathHelper.Lerp(_maxVisibility, _minVisibility, MathHelper.Max(0, _time) / _period);
            else
                visibility *= MathHelper.Lerp(_minVisibility, _maxVisibility, MathHelper.Max(0, _time) / _period);

            return true;
        }
        public void Start()
        {
            Debug.Assert(_maxVisibility >= _minVisibility);
            Debug.Assert(_maxVisibility <= 1);
            Debug.Assert(_maxVisibility >= _minVisibility);
            Debug.Assert(_minVisibility >= 0);
            _time = _period;
            _brightening = true;
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
                _time += _period;
            }

            _time -= Globals.GameTime.GetElapsedSeconds();
        }
    }
}
