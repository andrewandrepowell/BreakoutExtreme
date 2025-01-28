using BreakoutExtreme.Shaders;
using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace BreakoutExtreme.Features
{
    public class FloatAway : Feature
    {
        private const float _maxHeight = 8;
        private const float _period = 1;
        private float _offset;
        private float _time;
        private RunningStates _state = RunningStates.Waiting;
        public RunningStates State => _state;
        public void Start()
        {
            _offset = 0;
            _time = _period;
            _state = RunningStates.Starting;
        }
        public void ForceStart()
        {
            _offset = -_maxHeight;
            _state = RunningStates.Running;
        }
        public void Stop()
        {
            _offset = -_maxHeight;
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
                    _offset = MathHelper.Lerp(-_maxHeight, 0, MathHelper.Max(0, _time) / _period);
                else
                    ForceStart();
                _time -= Globals.GameTime.GetElapsedSeconds();
            }

            if (_state == RunningStates.Stopping)
            {
                if (_time > 0)
                    _offset = MathHelper.Lerp(0, -_maxHeight, MathHelper.Max(0, _time) / _period);
                else
                    ForceStop();
                _time -= Globals.GameTime.GetElapsedSeconds();
            }
        }
    }
}
