using MonoGame.Extended;

namespace BreakoutExtreme.Components
{
    public class PlayArea
    {
        public RectangleF Bounds => Globals.PlayAreaBounds;
        public PlayArea()
        {
            Globals.Runner.CreateWall(new RectangleF(Globals.PlayAreaBounds.X, Globals.PlayAreaBounds.Y, Globals.GameBlockSize, Globals.PlayAreaBounds.Height));
            Globals.Runner.CreateWall(new RectangleF(Globals.PlayAreaBounds.X + Globals.PlayAreaBounds.Width - Globals.GameBlockSize, Globals.PlayAreaBounds.Y, Globals.GameBlockSize, Globals.PlayAreaBounds.Height));
            Globals.Runner.CreateWall(new RectangleF(Globals.PlayAreaBounds.X + Globals.GameBlockSize, Globals.PlayAreaBounds.Y, Globals.PlayAreaBounds.Width - 2 * Globals.GameBlockSize, Globals.GameBlockSize));
            Globals.Runner.CreateWall(new RectangleF(Globals.PlayAreaBounds.X + Globals.GameBlockSize, Globals.PlayAreaBounds.Y + Globals.PlayAreaBounds.Height - Globals.GameBlockSize, Globals.PlayAreaBounds.Width - 2 * Globals.GameBlockSize, Globals.GameBlockSize));

            var ninePatcher = Globals.Runner.CreateNinePatcher();
            ninePatcher.Texture = NinePatcher.Textures.PlayAreaBottomRemoved;
            ninePatcher.Bounds = Globals.PlayAreaBounds;
        }
    }
}
