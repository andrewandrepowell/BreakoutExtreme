using MonoGameGum.GueDeriving;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using RenderingLibrary;
using RenderingLibrary.Graphics;

namespace BreakoutExtreme.Components
{
    public class Label
    {
        private readonly GumDrawer _gumDrawer;
        private readonly ContainerRuntime _containerRuntime;
        private readonly TextRuntime _textRuntime;
        private string _text = "H";
        private Color _textColor = Color.Black;
        public void UpdateTextRuntimeText()
        {
            _textRuntime.Text = Text;
        }
        public void UpdateTextRuntimeColor()
        {
            _textRuntime.Red = TextColor.R;
            _textRuntime.Green = TextColor.G;
            _textRuntime.Blue = TextColor.B;
        }
        public GumDrawer GetGumDrawer() => _gumDrawer;
        public string Text
        {
            get => _text;
            set
            {
                if (_text == value) return;
                _text = value;
                UpdateTextRuntimeText();
            }
        }
        public Color TextColor
        {
            get => _textColor;
            set
            {
                if (_textColor == value) return;
                _textColor = value;
                UpdateTextRuntimeColor();
            }
        }
        public Label(Size size)
        {
            {
                _containerRuntime = new ContainerRuntime();
                _containerRuntime.Width = size.Width;
                _containerRuntime.Height = size.Height;
            }

            {
                _textRuntime = new TextRuntime();
                _textRuntime.BitmapFont = GumUI.MontserratLarge;
                _textRuntime.Width = 0;
                _textRuntime.Height = 0;
                _textRuntime.WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer;
                _textRuntime.HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer;
                _textRuntime.VerticalAlignment = VerticalAlignment.Center;
                _containerRuntime.Children.Add(_textRuntime);
            }

            {
                UpdateTextRuntimeText();
                UpdateTextRuntimeColor();
            }

            _gumDrawer = new GumDrawer(_containerRuntime);
        }
    }
}
