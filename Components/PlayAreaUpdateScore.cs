using Microsoft.Xna.Framework;

namespace BreakoutExtreme.Components
{
    public partial class PlayArea
    {
        public void UpdateScore(Vector2 position)
        {
            var scorePopup = Globals.Runner.CreateScorePopup();
            scorePopup.Text = "+1";
            scorePopup.GetGumDrawer().Position = position;
            _scorePopups.Add(scorePopup);
            _parent.Score++;
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
            UpdateScore(collider.Position);
        }
    }
}
