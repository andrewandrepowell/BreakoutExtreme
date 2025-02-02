using MonoGame.Extended;
using System;
using BreakoutExtreme.Utility;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public class GameWindow
    {
        private readonly PlayArea _playArea;
        private readonly Panel _scorePanel;
        private readonly Panel _highScorePanel;
        private readonly RemainingBallsPanel _remainingBallsPanel;
        private readonly Button _menuButton;
        private int _score = 0;
        private void UpdateScorePanel()
        {
            _scorePanel.Text = $"{_score}";
        }
        private void OpenMenu()
        {

        }
        public int Score
        {
            get => _score;
            set
            {
                Debug.Assert(value >= 0);
                if (_score == value)
                    return;
                _score = value;
                UpdateScorePanel();
            }
        }
        public int RemainingBalls
        {
            get => _remainingBallsPanel.RemainingBalls;
            set => _remainingBallsPanel.RemainingBalls = value;
        }
        public GameWindow()
        {
            // Finally, instantiate the play area.
            _playArea = new PlayArea(this);

            // Create background area for UI.
            {
                var topPatch = Globals.Runner.CreateNinePatcher();
                topPatch.Texture = NinePatcher.Textures.GameWindowFilled;
                topPatch.Bounds = new RectangleF(0, 0, Globals.GameWindowBounds.Width, Globals.PlayAreaBounds.Y);
            }

            // Prepare score panel and label.
            {
                var scoreLabel = Globals.Runner.CreateLabel(Globals.ScoreLabelBlockBounds.Size.ToSize() * Globals.GameBlockSize);
                var gumDrawer = scoreLabel.GetGumDrawer();
                gumDrawer.Position = (gumDrawer.Size / 2).ToVector2() + Globals.ScoreLabelBlockBounds.Location.ToVector2() * Globals.GameBlockSize;
                scoreLabel.Text = "Score:";
            }
            {
                _scorePanel = Globals.Runner.CreatePanel();
                var gumDrawer = _scorePanel.GetGumDrawer();
                _scorePanel.Size = Globals.ScorePanelBlockBounds.Size.ToSize() * Globals.GameBlockSize;
                gumDrawer.Position = (gumDrawer.Size / 2).ToVector2() + Globals.ScorePanelBlockBounds.Location.ToVector2() * Globals.GameBlockSize;
                _scorePanel.Text = "0";
            }

            // Prepare high score panel and label.
            {
                var label = Globals.Runner.CreateLabel(Globals.HighScoreLabelBlockBounds.Size.ToSize() * Globals.GameBlockSize);
                var gumDrawer = label.GetGumDrawer();
                gumDrawer.Position = (gumDrawer.Size / 2).ToVector2() + Globals.HighScoreLabelBlockBounds.Location.ToVector2() * Globals.GameBlockSize;
                label.Text = "High Score:";
            }
            {
                _highScorePanel = Globals.Runner.CreatePanel();
                var gumDrawer = _highScorePanel.GetGumDrawer();
                _highScorePanel.Size = Globals.HighScorePanelBlockBounds.Size.ToSize() * Globals.GameBlockSize;
                gumDrawer.Position = (gumDrawer.Size / 2).ToVector2() + Globals.HighScorePanelBlockBounds.Location.ToVector2() * Globals.GameBlockSize;
                _highScorePanel.Text = "0";
            }

            // Prepare balls remaining label
            {
                var label = Globals.Runner.CreateLabel(Globals.BallsRemainingLabelBlockBounds.Size.ToSize() * Globals.GameBlockSize);
                var gumDrawer = label.GetGumDrawer();
                gumDrawer.Position = (gumDrawer.Size / 2).ToVector2() + Globals.BallsRemainingLabelBlockBounds.Location.ToVector2() * Globals.GameBlockSize;
                label.Text = "Balls Remaining:";
            }
            {
                _remainingBallsPanel = Globals.Runner.CreateRemainingBallsPanel(Globals.BallsRemainingPanelBlockBounds.Center.ToVector2() * Globals.GameBlockSize);
            }

            // Create the menu button.
            {
                _menuButton = Globals.Runner.CreateButton(
                    parent: this, 
                    action: (object parent) => OpenMenu(), 
                    bounds: Globals.MenuButtonBlockBounds.ToBounds(), 
                    text: "Menu");
            }
        }
        public void Update()
        {
            // temporary
            if (!_playArea.Loaded)
                _playArea.Load(PlayArea.Levels.Test);

            _menuButton.Update();
            _remainingBallsPanel.Update();
            _playArea.Update();
        }
    }
}
