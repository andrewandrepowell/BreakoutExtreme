using BreakoutExtreme.Shaders;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Diagnostics;

namespace BreakoutExtreme.Features
{
    public class Shake : Feature
    {
        private const float _shiftPeriod = (float)1 / 30;
        private const float _shiftMaxDistance = 4;
        private float _time = 0;
        private float _shiftTime = 0;
        private Vector2 _shiftVector = Vector2.Zero;
        public bool Running => _time > 0;
        public void Start(float time)
        {
            Debug.Assert(time >= 0);
            _shiftTime = 0;
            _time = time;
        }
        public void Stop()
        {
            _time = 0;
        }
        public override bool UpdateDrawOffset(ref Vector2 drawPosition)
        {
            if (!Running)
                return false;
            drawPosition.X += _shiftVector.X;
            drawPosition.Y += _shiftVector.Y;
            return true;
        }
        public override void Update()
        {
            if (Running)
            {
                {
                    if (_shiftTime <= 0)
                    {
                        _shiftVector.X = _shiftMaxDistance * (Globals.Random.NextSingle() - 0.5f);
                        _shiftVector.Y = _shiftMaxDistance * (Globals.Random.NextSingle() - 0.5f);
                        _shiftTime += _shiftPeriod;
                    }
                }
                {
                    var timeElapsed = Globals.GameTime.GetElapsedSeconds();
                    _time -= timeElapsed;
                    _shiftTime -= timeElapsed;
                }
            }
        }
    }
}
