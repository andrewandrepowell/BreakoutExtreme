using MonoGame.Extended;

namespace BreakoutExtreme.Components
{
    public class PlayArea
    {
        private const int _wallThickness = 16;
        public RectangleF Bounds => Globals.PlayAreaBounds;
        public PlayArea()
        {
            Globals.Runner.CreateWall(new RectangleF(Globals.PlayAreaBounds.X, Globals.PlayAreaBounds.Y, _wallThickness, Globals.PlayAreaBounds.Height));
            Globals.Runner.CreateWall(new RectangleF(Globals.PlayAreaBounds.X + Globals.PlayAreaBounds.Width - _wallThickness, Globals.PlayAreaBounds.Y, _wallThickness, Globals.PlayAreaBounds.Height));
            Globals.Runner.CreateWall(new RectangleF(Globals.PlayAreaBounds.X + _wallThickness, Globals.PlayAreaBounds.Y, Globals.PlayAreaBounds.Width - 2 * _wallThickness, _wallThickness));
            Globals.Runner.CreateWall(new RectangleF(Globals.PlayAreaBounds.X + _wallThickness, Globals.PlayAreaBounds.Y + Globals.PlayAreaBounds.Height - _wallThickness, Globals.PlayAreaBounds.Width - 2 * _wallThickness, _wallThickness));

            var ninePatcher = Globals.Runner.CreateNinePatcher();
            ninePatcher.Texture = NinePatcher.Textures.PlayArea;
            ninePatcher.Bounds = Globals.PlayAreaBounds;
        }
    }
}
