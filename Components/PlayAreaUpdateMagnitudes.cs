using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public partial class PlayArea
    {
        private const float _timeToUpdateMagnitudes = 10;
        private const float _timeToReachMaxMagnitudes = 60 * 1;
        private const float _timeToReachSpeedUp = 0.5f * _timeToReachMaxMagnitudes;
        private const float _ballHueOffsetMin = 0.0f;
        private const float _ballHueOffsetMax = 0.5f;
        private const float _ballMinMagnitude = 5000;
        private const float _ballMaxMagnitude = 2 * _ballMinMagnitude;
        private const float _paddleMinMagnitude = 7000;
        private const float _paddleMaxMagnitude = 2 * _paddleMinMagnitude;
        private float _updateMagnitudeTime;
        private float _ballMagnitude;
        private float _ballHueOffset;
        private void UpdateMagnitudes()
        {
            Debug.Assert(_paddle != null);
            Debug.Assert(_ballMaxMagnitude >= _ballMinMagnitude);
            Debug.Assert(_paddleMaxMagnitude >= _paddleMinMagnitude);
            var ratio = MathHelper.Clamp(_timeElapsedSinceLaunch / _timeToReachMaxMagnitudes, 0, 1);
            _ballMagnitude = MathHelper.Lerp(_ballMinMagnitude, _ballMaxMagnitude, ratio);
            _paddle.AccelerationToTarget = MathHelper.Lerp(_paddleMinMagnitude, _paddleMaxMagnitude, ratio);
            _ballHueOffset = MathHelper.Lerp(_ballHueOffsetMin, _ballHueOffsetMax, ratio);
            for (var i = 0; i < _balls.Count; i++)
            {
                var ball = _balls[i];
                ball.HueOffset = _ballHueOffset;
                ball.LaunchMagnitude = _ballMagnitude;
            }
        }
        public float BallHueOffset => _ballHueOffset;
    }
}
