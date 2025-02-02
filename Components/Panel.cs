using MonoGameGum.GueDeriving;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGameGum;
using RenderingLibrary.Graphics;
using BreakoutExtreme.Utility;
using RenderingLibrary;

namespace BreakoutExtreme.Components
{
    public class Panel
    {
        private static readonly Size _initialSize = new(Globals.GameBlockSize * 3, Globals.GameBlockSize * 3);
        private readonly ContainerRuntime _containerRuntime;
        private readonly TextRuntime _textRuntime;
        private readonly GumDrawer _gumDrawer;
        private Size _size = _initialSize;
        private Color _textColor = Color.Black;
        private string _text = "H";
        private void UpdateContainerSize()
        {
            _containerRuntime.Width = Size.Width;
            _containerRuntime.Height = Size.Height;
        }
        private void UpdateTextRuntimeColor()
        {
            _textRuntime.Red = TextColor.R;
            _textRuntime.Green = TextColor.G;
            _textRuntime.Blue = TextColor.B;
        }
        private void UpdateTextRuntimeText()
        {
            _textRuntime.Text = Text;
        }
        public GumDrawer GetGumDrawer() => _gumDrawer;
        public Color TextColor
        {
            get => _textColor;
            set
            {
                if (_textColor == value)
                    return;
                _textColor = value;
                UpdateTextRuntimeColor();
            }
        }
        public string Text
        {
            get => _text;
            set
            {
                if (_text == value)
                    return;
                _text = value;
                UpdateTextRuntimeText();
            }
        }
        public Size Size
        {
            get => _size;
            set
            {
                if (_size ==  value) 
                    return;
                _size = value;
                UpdateContainerSize();
                _gumDrawer.UpdateSizeImmediately();
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

            {
                _textRuntime = new();
                _textRuntime.BitmapFont = new BitmapFont("fonts/montserrat/montserrat_1.fnt", SystemManagers.Default);
                _textRuntime.X = Globals.GameBlockSize;
                _textRuntime.Y = 0;
                _textRuntime.Width = -Globals.GameBlockSize * 2;
                _textRuntime.Height = 0;
                _textRuntime.WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer;
                _textRuntime.HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer;
                _textRuntime.VerticalAlignment = VerticalAlignment.Center;
                UpdateTextRuntimeColor();
                UpdateTextRuntimeText();
                _containerRuntime.Children.Add(_textRuntime);
            }

            _gumDrawer = new GumDrawer(_containerRuntime);
        }
    }
}
