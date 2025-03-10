using Microsoft.Xna.Framework;

namespace BreakoutExtreme.Components
{
    public partial class PlayArea
    {
        private const float _intensePeriod = 1;
        private float _intenseTime;
        public void UpdateScore(Vector2 position, bool bomb = false)
        {
            bool intense;
            bool speedUp;
            int increment;
            if (bomb)
            { 
                intense = false;
                speedUp = false;
                increment = 1;
            }
            else
            {
                intense = _intenseTime > 0;
                speedUp = _timeElapsedSinceLaunch >= _timeToReachSpeedUp;
                increment = (1 + _parent.LevelsCleared) * 10 * (intense ? 2 : 1) * (speedUp ? 2 : 1);
            }
            var scorePopup = Globals.Runner.CreateScorePopup(intense, speedUp);
            scorePopup.Text = $"+{increment}";
            scorePopup.GetGumDrawer().Position = position;
            _intenseTime = _intensePeriod;
            _scorePopups.Add(scorePopup);
            _parent.Score += increment;
        }
        public void UpdateScore(Cannon cannon)
        {
            var collider = cannon.GetCollider();
            UpdateScore(collider.Position);
        }
        public void UpdateScore(Brick brick)
        {
            var collider = brick.GetCollider();
            UpdateScore(collider.Position + (Vector2)(collider.Size / 2));
        }
        public void UpdateScore(Bomb bomb)
        {
            var collider = bomb.GetCollider();
            UpdateScore(collider.Position, true);
        }
    }
}
