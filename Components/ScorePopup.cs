using MonoGameGum.GueDeriving;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using RenderingLibrary.Graphics;
using RenderingLibrary;
using MonoGame.Extended.ECS;
using BreakoutExtreme.Utility;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections.Generic;

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
        private readonly Features.Shake _shake;
        private Entity _entity;
        private bool _running;
        private bool _initialized;
        private Color _textColor = Color.Black;
        private string _text = "H";
        private record IntenseConfig(Color TextColor, bool Shake);
        private static ReadOnlyDictionary<bool, IntenseConfig> _intenseConfigs = new(new Dictionary<bool, IntenseConfig>() 
        {
            { false, new(Color.Black, false) },
            { true, new(Color.Red, true) }
        });
        private void UpdateContainerSize()
        {
            _containerRuntime.Width = Size.Width;
            _containerRuntime.Height = Size.Height;
        }
        private void UpdateTextRuntimeColor()
        {
            _textRuntime.Red = _textColor.R;
            _textRuntime.Green = _textColor.G;
            _textRuntime.Blue = _textColor.B;
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
            if (Globals.Paused)
                return;
            if (!_initialized)
                return;
            if (_running && !_vanish.Running && _floatUp.State == RunningStates.Running)
                _running = false;
        }
        public void Reset(Entity entity, bool intense = false)
        {
            Debug.Assert(!_initialized);
            var intenseConfig = _intenseConfigs[intense];
            _entity = entity;
            _vanish.Start();
            _floatUp.Start();
            if (intenseConfig.Shake)
                _shake.Start();
            else
                _shake.Stop();
            _textColor = intenseConfig.TextColor;
            UpdateTextRuntimeColor();
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
                    BitmapFont = GumUI.MontserratSmall,
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
                _shake = new();
                _gumDrawer.ShaderFeatures.Add(_shake);
            }
        }
    }
}
