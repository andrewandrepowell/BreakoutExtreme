using BreakoutExtreme.Shaders;
using MonoGame.Extended;
using System.Diagnostics;
using System;

namespace BreakoutExtreme.Features
{
    public class Shine : Feature
    {
        private const float _speed = 0.2f;
        private const float _distortion = 2.0f;
        private bool _running = false;
        private float _delayPeriod = 0;
        private float _delayTime;
        private float _repeatPeriod = 3;
        private float _repeatTime;
        private float _activePeriod = 3;
        private float _activeTime = 0;
        private float _initialGameTimeSeconds;
        public bool Running => _running;
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
        public float RepeatPeriod
        {
            get => _repeatPeriod;
            set
            {
                Debug.Assert(!_running);
                Debug.Assert(value >= 0);
                Debug.Assert(value >= _activePeriod);
                _repeatPeriod = value;
            }
        }
        public float ActivePeriod
        {
            get => _activePeriod;
            set
            {
                Debug.Assert(!_running);
                Debug.Assert(value >= 0);
                Debug.Assert(value <= _repeatPeriod);
                _activePeriod = value;
            }
        }
        public override Scripts? Script => (_running && _activeTime > 0) ? Scripts.HighlightCanvasItem : null;
        public void Start()
        {
            _delayTime = _delayPeriod;
            _repeatTime = 0;
            _activeTime = 0;
            _running = true;
        }
        public void Stop()
        {
            _running = false;
        }
        public override void UpdateShaderNode(HighlightCanvasItemNode node)
        {
            Debug.Assert(_running);
            node.Configure( 
                speed: _speed,
                distortion: _distortion,
                initialGameTimeSeconds: _initialGameTimeSeconds);
        }
        public override void Update()
        {
            if (_running)
            {
                if (_repeatTime <= 0 && _delayTime <= 0)
                {
                    _initialGameTimeSeconds = (float)Globals.GameTime.TotalGameTime.TotalSeconds;
                    _activeTime += _activePeriod;
                    _repeatTime += _repeatPeriod;
                }

                var timeElapsed = Globals.GameTime.GetElapsedSeconds();
                if (_activeTime > 0)
                    _activeTime -= timeElapsed;
                if (_repeatTime > 0)
                    _repeatTime -= timeElapsed;
                if (_delayTime > 0)
                    _delayTime -= timeElapsed;
            }
        }
    }
}
