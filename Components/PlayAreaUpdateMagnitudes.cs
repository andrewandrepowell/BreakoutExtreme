using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public partial class PlayArea
    {
        private const float _timeToUpdateMagnitudes = 10;
        private const float _timeToReachMaxMagnitudes = 60 * 10; // 30 * 1;
        private const float _ballMinMagnitude = 5000;
        private const float _ballMaxMagnitude = 2.5f * 5000;
        private float _updateMagnitudeTime;
        private float _ballMagnitude;
        private void UpdateMagnitudes()
        {
            Debug.Assert(_ballMaxMagnitude >= _ballMinMagnitude);
            _ballMagnitude = MathHelper.Lerp(_ballMinMagnitude, _ballMaxMagnitude, MathHelper.Clamp(_parent.GameTimeElapsed / _timeToReachMaxMagnitudes, 0, 1));
            for (var i = 0; i < _balls.Count; i++)
            {
                var ball = _balls[i];
                if (ball.State == Ball.States.Active)
                    ball.LaunchMagnitude = _ballMagnitude;
            }
        }
    }
}
