using MonoGameGum.GueDeriving;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using RenderingLibrary.Graphics;
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
                var texture = Globals.ContentManager.Load<Texture2D>("animations/panel_0");
                var nineSlice = new NineSliceRuntime()
                {
                    SourceFile = texture,
                    Width = 0,
                    Height = 0,
                    WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer,
                    HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer,
                };
                _containerRuntime.Children.Add(nineSlice);
            }

            {
                _textRuntime = new()
                {
                    BitmapFont = new BitmapFont("fonts/montserrat/montserrat_1.fnt", SystemManagers.Default),
                    X = Globals.GameBlockSize,
                    Y = 0,
                    Width = -Globals.GameBlockSize * 2,
                    Height = 0,
                    WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer,
                    HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                UpdateTextRuntimeColor();
                UpdateTextRuntimeText();
                _containerRuntime.Children.Add(_textRuntime);
            }

            _gumDrawer = new GumDrawer(_containerRuntime);
        }
    }
}
