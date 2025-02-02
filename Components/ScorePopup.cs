using MonoGameGum.GueDeriving;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using RenderingLibrary.Graphics;
using RenderingLibrary;
using MonoGame.Extended.ECS;
using BreakoutExtreme.Utility;

namespace BreakoutExtreme.Components
{
    public partial class ScorePopup
    {
        private static readonly Size _size = new Size(Globals.GameBlockSize * 4, Globals.GameBlockSize * 1);
        private readonly ContainerRuntime _containerRuntime;
        private readonly TextRuntime _textRuntime;
        private readonly GumDrawer _gumDrawer;
        private readonly Entity _entity;
        private readonly Features.Vanish _vanish;
        private readonly Features.FloatUp _floatUp;
        private readonly Features.Flash _flash;
        private bool _running = true;
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
        public bool Running => _running;
        public GumDrawer GetGumDrawer() => _gumDrawer;
        public Size Size => _size;
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
        public void RemoveEntity()
        {
            Globals.Runner.RemoveEntity(_entity);
        }
        public void Update()
        {
            if (_running && !_vanish.Running && _floatUp.State == RunningStates.Running)
                _running = false;
        }
        public ScorePopup(Entity entity)
        {
            {
                _entity = entity;
            }

            {
                _containerRuntime = new ContainerRuntime();
                UpdateContainerSize();
            }

            {
                _textRuntime = new TextRuntime();
                _textRuntime.X = 0;
                _textRuntime.Y = 0;
                _textRuntime.Width = 0;
                _textRuntime.Height = 0;
                _textRuntime.WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer;
                _textRuntime.HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer;
                _textRuntime.VerticalAlignment = VerticalAlignment.Center;
                _textRuntime.HorizontalAlignment = HorizontalAlignment.Center;
                _textRuntime.BitmapFont = new BitmapFont("fonts/montserrat/montserrat_0.fnt", SystemManagers.Default);
                UpdateTextRuntimeColor();
                UpdateTextRuntimeText();
                _containerRuntime.Children.Add(_textRuntime);
            }

            {
                _gumDrawer = new GumDrawer(_containerRuntime);
                _vanish = new();
                _vanish.Period = 4;
                _vanish.Start();
                _gumDrawer.ShaderFeatures.Add(_vanish);
                _floatUp = new();
                _floatUp.Period = 1;
                _floatUp.Start();
                _gumDrawer.ShaderFeatures.Add(_floatUp);
                _flash = new();
                _flash.Color = Color.White;
                _flash.Start();
                _gumDrawer.ShaderFeatures.Add(_flash);
            }
        }
    }
}
