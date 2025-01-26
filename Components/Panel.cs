using MonoGameGum.GueDeriving;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using BreakoutExtreme.Utility;

namespace BreakoutExtreme.Components
{
    public class Panel
    {
        private static readonly Size _initialSize = new Size(Globals.GameBlockSize * 2, Globals.GameBlockSize * 2);
        private readonly ContainerRuntime _containerRuntime;
        private readonly GumDrawer _gumDrawer;
        private void UpdateContainerSize()
        {
            _containerRuntime.Width = Size.Width;
            _containerRuntime.Height = Size.Height;
        }
        public GumDrawer GetGumDrawer() => _gumDrawer;
        public Size _size = _initialSize;
        public Size Size
        {
            get => _size;
            set
            {
                if (_size ==  value) 
                    return;
                _size = value;
                UpdateContainerSize();
            }
        }
        public Panel()
        {
            {
                _containerRuntime = new ContainerRuntime();
                UpdateContainerSize();
            }

            {
                var nineSlice = new NineSliceRuntime();
                var texture = Globals.ContentManager.Load<Texture2D>("animations/panel_0");
                nineSlice.SourceFile = texture;
                nineSlice.Width = 0;
                nineSlice.Height = 0;
                nineSlice.WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer;
                nineSlice.HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer;
                _containerRuntime.Children.Add(nineSlice);
            }

            _gumDrawer = new GumDrawer(_containerRuntime);
        }
    }
}
