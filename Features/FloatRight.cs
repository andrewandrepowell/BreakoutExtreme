using BreakoutExtreme.Shaders;
using BreakoutExtreme.Utility;
using MonoGame.Extended;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace BreakoutExtreme.Features
{
    public class FloatRight : Feature
    {
        private float _period = 1;
        private float _maxDistance = 8;
        private float _offset;
        private float _time;
        private float _delayPeriod = 0;
        private float _delayTime;
        private bool _smooth = false;
        private RunningStates _state = RunningStates.Waiting;
        private float Lerp(float start, float end, float amount)
        {
            if (_smooth)
                return MathHelper.SmoothStep(start, end, amount);
            return MathHelper.Lerp(start, end, amount);
        }
        public float DelayPeriod
        {
            get => _delayPeriod;
            set
            {
                Debug.Assert(_state == RunningStates.Waiting ||  _state == RunningStates.Running);
                Debug.Assert(value >= 0);
                _delayPeriod = value;
            }
        }
        public bool Smooth 
        {
            get => _smooth; 
            set
            {
                Debug.Assert(_state == RunningStates.Waiting || _state == RunningStates.Running);
                _smooth = value;
            }
        }
        public RunningStates State => _state;
        public float Period
        {
            get => _period;
            set
            {
                Debug.Assert(_state == RunningStates.Waiting || _state == RunningStates.Running);
                Debug.Assert(value >= 0);
                _period = value;
            }
        }
        public float MaxDistance { get => _maxDistance; set => _maxDistance = value; }
        public void Start()
        {
            _offset = 0;
            _time = _period;
            _delayTime = _delayPeriod;
            _state = RunningStates.Starting;
        }
        public void ForceStart()
        {
            _offset = _maxDistance;
            _state = RunningStates.Running;
        }
        public void Stop()
        {
            _offset = _maxDistance;
            _time = _period;
            _delayTime = _delayPeriod;
            _state = RunningStates.Stopping;
        }
        public void ForceStop()
        {
            _offset = 0;
            _state = RunningStates.Waiting;
        }
        public override bool UpdateDrawOffset(ref Vector2 drawPosition)
        {
            if (State == RunningStates.Waiting)
                return false;
            drawPosition.X += _offset;
            return true;
        }
        public override void Update()
        {
            if (_state == RunningStates.Starting)
            {
                if (_time > 0)
                    _offset = Lerp(_maxDistance, 0, MathHelper.Max(0, _time) / _period);
                else
                    ForceStart();
            }

            if (_state == RunningStates.Stopping)
            {
                if (_time > 0)
                    _offset = Lerp(0, _maxDistance, MathHelper.Max(0, _time) / _period);
                else
                    ForceStop();
            }

            if (_state == RunningStates.Starting || _state == RunningStates.Stopping)
            {
                var timeElapsed = Globals.GameTime.GetElapsedSeconds();
                if (_delayTime > 0)
                    _delayTime -= timeElapsed;
                else
                    _time -= timeElapsed;
            }
        }
    }
}
