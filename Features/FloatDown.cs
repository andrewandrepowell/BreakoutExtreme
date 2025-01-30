using BreakoutExtreme.Utility;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using BreakoutExtreme.Shaders;
using System.Diagnostics;

namespace BreakoutExtreme.Features
{
    public class FloatDown : Feature
    {
        private float _period = 1;
        private float _minHeight = 8;
        private float _offset;
        private float _time;
        private RunningStates _state = RunningStates.Waiting;
        public RunningStates State => _state;
        public float Period 
        { 
            get => _period;
            set
            {
                Debug.Assert(value >= 0);
                _period = value;
            }
        }
        public float MinHeight { get => _minHeight; set => _minHeight = value; }
        public void Start()
        {
            _offset = 0;
            _time = _period;
            _state = RunningStates.Starting;
        }
        public void ForceStart()
        {
            _offset = _minHeight;
            _state = RunningStates.Running;
        }
        public void Stop()
        {
            _offset = _minHeight;
            _time = _period;
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
            drawPosition.Y += _offset;
            return true;
        }
        public override void Update()
        {
            if (_state == RunningStates.Starting)
            {
                if (_time > 0)
                    _offset = MathHelper.Lerp(_minHeight, 0, MathHelper.Max(0, _time) / _period);
                else
                    ForceStart();
                _time -= Globals.GameTime.GetElapsedSeconds();
            }

            if (_state == RunningStates.Stopping)
            {
                if (_time > 0)
                    _offset = MathHelper.Lerp(0, _minHeight, MathHelper.Max(0, _time) / _period);
                else
                    ForceStop();
                _time -= Globals.GameTime.GetElapsedSeconds();
            }
        }
    }
}
