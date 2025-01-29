using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public class RemainingBallsPanel
    {
        private const int _maximumBalls = 3;
        private const float _distanceBetweenEachBall = 20;
        private const float _leftOffset = (_maximumBalls == 0) ? 0 : (((_maximumBalls - 1) * _distanceBetweenEachBall) / 2);
        private const float _floatDisplacementPerBall = (float)1 / _maximumBalls * 0.75f;
        private readonly DisplayBall[] _displayBalls;
        private readonly Vector2 _position;
        private int _remainingBalls = 3;
        private void UpdateDisplayBalls()
        {
            Debug.Assert(_remainingBalls >= 0 || _remainingBalls <= _maximumBalls);
            float _leftX = _position.X - _leftOffset;
            for (var i = 0; i < _maximumBalls; i++)
            {
                var displayBall = _displayBalls[i];
                displayBall.GetAnimater().Position = new Vector2(
                    x: _leftX + i * _distanceBetweenEachBall, 
                    y: _position.Y);
                if (i < _remainingBalls && !displayBall.Running)
                {
                    displayBall.Start();
                }
                else if (i >= _remainingBalls && displayBall.Running)
                {
                    displayBall.Stop();
                }
            }
        }
        public static int MaximumBalls => _maximumBalls;
        public int RemainingBalls
        {
            get => _remainingBalls;
            set
            {
                Debug.Assert(value >= 0 && value <= _maximumBalls);
                if (_remainingBalls == value)
                    return;
                _remainingBalls = value;
                UpdateDisplayBalls();
            }
        }
        public RemainingBallsPanel(Vector2 position)
        {
            _position = position;
            _displayBalls = new DisplayBall[_maximumBalls];
            for (var i = 0; i < _maximumBalls; i++)
            {
                var displayBall = Globals.Runner.CreateDisplayBall(i * _floatDisplacementPerBall);
                _displayBalls[i] = displayBall;
            }
            UpdateDisplayBalls();
        }
        public void Update()
        {
            foreach (ref var displayBall in _displayBalls.AsSpan())
            {
                displayBall.Update();
            }
        }
    }
}
