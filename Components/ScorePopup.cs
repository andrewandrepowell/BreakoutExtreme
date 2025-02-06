using MonoGameGum.GueDeriving;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using RenderingLibrary.Graphics;
using RenderingLibrary;
using MonoGame.Extended.ECS;
using BreakoutExtreme.Utility;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public partial class ScorePopup : IUpdate, IRemoveEntity, IDestroyed
    {
        private static readonly Size _size = new(Globals.GameBlockSize * 4, Globals.GameBlockSize * 1);
        private readonly ContainerRuntime _containerRuntime;
        private readonly TextRuntime _textRuntime;
        private readonly GumDrawer _gumDrawer;
        private readonly Features.Vanish _vanish;
        private readonly Features.FloatUp _floatUp;
        private readonly Features.Flash _flash;
        private Entity _entity;
        private bool _running;
        private bool _initialized;
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
        public bool Initialized => _initialized;
        public GumDrawer GetGumDrawer() => _gumDrawer;
#pragma warning disable CA1822
        public Size Size => _size;
#pragma warning restore CA1822
        public Color TextColor
        {
            get => _textColor;
            set
            {
                Debug.Assert(_initialized);
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
                Debug.Assert(_initialized);
                if (_text == value)
                    return;
                _text = value;
                UpdateTextRuntimeText();
            }
        }
        public bool Destroyed => !_running;
        public void RemoveEntity()
        {
            Debug.Assert(_initialized);
            _initialized = false;
            Globals.Runner.RemoveEntity(_entity);
        }
        public void Update()
        {
            if (!_initialized)
                return;
            if (_running && !_vanish.Running && _floatUp.State == RunningStates.Running)
                _running = false;
        }
        public void Reset(Entity entity)
        {
            Debug.Assert(!_initialized);
            _entity = entity;
            _vanish.Start();
            _floatUp.Start();
            _initialized = true;
            _running = true;
        }
        public ScorePopup(Entity entity = null)
        {
            {
                _entity = entity;
                _initialized = entity != null;
                _running = _initialized;
            }

            {
                _containerRuntime = new ContainerRuntime();
                UpdateContainerSize();
            }

            {
                _textRuntime = new()
                {
                    BitmapFont = new BitmapFont("fonts/montserrat/montserrat_0.fnt", SystemManagers.Default),
                    X = 0,
                    Y = 0,
                    Width = 0,
                    Height = 0,
                    WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer,
                    HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                };
                UpdateTextRuntimeColor();
                UpdateTextRuntimeText();
                _containerRuntime.Children.Add(_textRuntime);
            }

            {
                _gumDrawer = new GumDrawer(_containerRuntime);
                _vanish = new() { Period = 2 };
                _vanish.Start();
                _gumDrawer.ShaderFeatures.Add(_vanish);
                _floatUp = new() { Period = 1 };
                _floatUp.Start();
                _gumDrawer.ShaderFeatures.Add(_floatUp);
                _flash = new() { Color = Color.White };
                _flash.Start();
                _gumDrawer.ShaderFeatures.Add(_flash);
            }
        }
    }
}
