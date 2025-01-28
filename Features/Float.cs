using BreakoutExtreme.Shaders;
using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace BreakoutExtreme.Features
{
    public class Float : Feature
    {
        private readonly float _maxHeight = 8;
        private readonly float _period = 1; 
        private float _time;
        private float _offset;
        private Directions _direction = Directions.Up;
        private bool _running = false;
        public override bool UpdateDrawOffset(ref Vector2 drawPosition)
        {
            if (!_running)
                return false;
            drawPosition.Y += _offset;
            return true;
        }
        public bool Running => _running;
        public void Start()
        {
            _offset = _maxHeight;
            _time = _period;
            _direction = Directions.Up;
            _running = true;
        }
        public void Stop()
        {
            _running = false;
        }
        public override void Update()
        {
            if (_running)
            {
                if (_direction == Directions.Up)
                    _offset = MathHelper.Lerp(-_maxHeight, _maxHeight, MathHelper.Max(0, _time) / _period);
                else
                    _offset = MathHelper.Lerp(_maxHeight, -_maxHeight, MathHelper.Max(0, _time) / _period);
                while (_time <= 0)
                {
                    _direction = (_direction == Directions.Up) ? Directions.Down : Directions.Up;
                    _time += _period;
                }
                _time -= Globals.GameTime.GetElapsedSeconds();
            }
        }
    }
}
