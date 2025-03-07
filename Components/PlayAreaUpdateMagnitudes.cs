using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public partial class PlayArea
    {
        private const float _timeToUpdateMagnitudes = 10;
        private const float _timeToReachMaxMagnitudes = 60 * 5;
        private const float _ballMinMagnitude = 5000;
        private const float _ballMaxMagnitude = 2 * _ballMinMagnitude;
        private const float _paddleMinMagnitude = 7000;
        private const float _paddleMaxMagnitude = 2 * _paddleMinMagnitude;
        private float _updateMagnitudeTime;
        private float _ballMagnitude;
        private void UpdateMagnitudes()
        {
            Debug.Assert(_paddle != null);
            Debug.Assert(_ballMaxMagnitude >= _ballMinMagnitude);
            Debug.Assert(_paddleMaxMagnitude >= _paddleMinMagnitude);
            var ratio = MathHelper.Clamp(_parent.GameTimeElapsed / _timeToReachMaxMagnitudes, 0, 1);
            _ballMagnitude = MathHelper.Lerp(_ballMinMagnitude, _ballMaxMagnitude, ratio);
            _paddle.AccelerationToTarget = MathHelper.Lerp(_paddleMinMagnitude, _paddleMaxMagnitude, ratio);
            for (var i = 0; i < _balls.Count; i++)
            {
                var ball = _balls[i];
                if (ball.State == Ball.States.Active)
                    ball.LaunchMagnitude = _ballMagnitude;
            }
        }
    }
}
