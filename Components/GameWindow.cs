using MonoGame.Extended;
using System;

namespace BreakoutExtreme.Components
{
    public class GameWindow
    {
        private PlayArea _playArea;
        public GameWindow()
        {
            _playArea = new PlayArea();
            
            var topPatch = Globals.Runner.CreateNinePatcher();
            topPatch.Texture = NinePatcher.Textures.GameWindowFilled;
            topPatch.Bounds = new RectangleF(0, 0, Globals.GameWindowBounds.Width, Globals.PlayAreaBounds.Y);
        }
        public void Update()
        {
            if (!_playArea.Loaded)
                _playArea.Load(PlayArea.Levels.Test);
            
            _playArea.Update();
        }
    }
}
