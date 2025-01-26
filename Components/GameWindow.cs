using MonoGame.Extended;
using Microsoft.Xna.Framework;
using System;
using BreakoutExtreme.Utility;

namespace BreakoutExtreme.Components
{
    public class GameWindow
    {
        private readonly PlayArea _playArea;
        private readonly Panel _scorePanel;
        private readonly Panel _highScorePanel;
        public GameWindow()
        {
            GumUI.Initialize();
            _playArea = new PlayArea();

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
                _scorePanel.Text = "1111";
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
                _highScorePanel.Text = "0000";
            }
        }
        public void Update()
        {
            // temporary
            if (!_playArea.Loaded)
                _playArea.Load(PlayArea.Levels.Test);
            
            _playArea.Update();
        }
    }
}
