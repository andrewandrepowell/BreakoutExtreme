using BreakoutExtreme.Shaders;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Diagnostics;
using System;

namespace BreakoutExtreme.Features
{
    public class Shake : Feature
    {
        private const float _shiftPeriod = (float)1 / 30;
        private float _shiftMaxDistance = 4;
        private float _period = 1;
        private float _time = 0;
        private float _delayPeriod = 0;
        private float _delayTime;
        private float _shiftTime = 0;
        private Vector2 _shiftVector = Vector2.Zero;
        public bool Running => _time > 0;
        public float Period
        {
            get => _period;
            set
            {
                Debug.Assert(!Running);
                Debug.Assert(_period >= 0);
                _period = value;
            }
        }
        public float DelayPeriod
        {
            get => _delayPeriod;
            set
            {
                Console.WriteLine($"{_time}");
                Debug.Assert(!Running);
                Debug.Assert(value >= 0);
                _delayPeriod = value;
            }
        }
        public void Start()
        {
            _shiftTime = 0;
            _delayTime = _delayPeriod;
            _time = _period;
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
                    if (_shiftTime <= 0 && _delayTime <= 0)
                    {
                        _shiftVector.X = _shiftMaxDistance * (Globals.Random.NextSingle() - 0.5f);
                        _shiftVector.Y = _shiftMaxDistance * (Globals.Random.NextSingle() - 0.5f);
                        _shiftTime += _shiftPeriod;
                    }
                }


                {
                    var timeElapsed = Globals.GameTime.GetElapsedSeconds();
                    if (_delayTime <= 0)
                    {
                        _time -= timeElapsed;
                        _shiftTime -= timeElapsed;
                    }
                    else
                    {
                        _delayTime -= timeElapsed;
                    }
                }
            }
        }
    }
}
