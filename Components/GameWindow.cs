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
        public GameWindow()
        {
            GumUI.Initialize();
            _playArea = new PlayArea();

            {
                var topPatch = Globals.Runner.CreateNinePatcher();
                topPatch.Texture = NinePatcher.Textures.GameWindowFilled;
                topPatch.Bounds = new RectangleF(0, 0, Globals.GameWindowBounds.Width, Globals.PlayAreaBounds.Y);
            }

            {
                _scorePanel = Globals.Runner.CreatePanel();
                var gumDrawer = _scorePanel.GetGumDrawer();
                _scorePanel.Size = new Size(5, 3) * Globals.GameBlockSize;
                gumDrawer.Position = new Vector2(gumDrawer.Size.Width / 2 + Globals.GameBlockSize * 0, gumDrawer.Size.Height / 2 + Globals.GameBlockSize);
                _scorePanel.Text = "Hello";
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
