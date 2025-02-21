using MonoGameGum.GueDeriving;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using MonoGame.Extended;
using RenderingLibrary.Graphics;
using RenderingLibrary;
using BreakoutExtreme.Utility;

namespace BreakoutExtreme.Components
{
    public class Button : IUpdate
    {
        private static readonly ReadOnlyDictionary<States, string> _stateNineSliceAssetNames = new(new Dictionary<States, string>()
        {
            { States.Pressed, "animations/button_1" },
            { States.Released, "animations/button_0" }
        });
        private static readonly ReadOnlyDictionary<States, Vector2> _stateTextRuntimeOffsets = new(new Dictionary<States, Vector2>()
        {
            { States.Pressed, new(16, 16) },
            { States.Released, new(16, 12) }
        });
        private static readonly ReadOnlyDictionary<States, Vector2> _stateTextRuntimeSizes = new(new Dictionary<States, Vector2>()
        {
            { States.Pressed, new(-16 -16, -16 -13) },
            { States.Released, new(-16 -16, -12 -19 ) }
        });
        private static readonly States[] _states = Enum.GetValues<States>();
        private const float _pressedPeriod = 0.5f;
        private readonly Dictionary<States, Texture2D> _stateNineSlaceTextures = new();
        private readonly ContainerRuntime _containerRuntime;
        private readonly TextRuntime _textRuntime;
        private readonly NineSliceRuntime _nineSliceRuntime;
        private readonly GumDrawer _gumDrawer;
        private readonly Action<object> _action;
        private readonly object _parent;
        private readonly RectangleF _bounds;
        private States _state;
        private float _pressedTime;

        private enum States { Released, Pressed }
        private void UpdateTextRuntime()
        {
            var offset = _stateTextRuntimeOffsets[_state];
            var size = _stateTextRuntimeSizes[_state];
            _textRuntime.X = offset.X;
            _textRuntime.Y = offset.Y;
            _textRuntime.Width = size.X;
            _textRuntime.Height = size.Y;
        }
        private void UpdateNineSlice()
        {
            _nineSliceRuntime.SourceFile = _stateNineSlaceTextures[_state];
        }
        public object Parent => _parent;
        public GumDrawer GetGumDrawer() => _gumDrawer;
        public bool Running = true;
        public Button(object parent, Action<object> action, RectangleF bounds, string text)
        {
            _parent = parent;
            _action = action;
            _bounds = bounds;
            _state = States.Released;

            {
                _containerRuntime = new ContainerRuntime();
                _containerRuntime.Width = bounds.Width;
                _containerRuntime.Height = bounds.Height;
            }

            {
                foreach (ref var state in _states.AsSpan())
                {
                    _stateNineSlaceTextures.Add(state, Globals.ContentManager.Load<Texture2D>(_stateNineSliceAssetNames[state]));
                }
                _nineSliceRuntime = new NineSliceRuntime()
                {
                    Width = 0,
                    Height = 0,
                    WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer,
                    HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer,
                };
                UpdateNineSlice();
                _containerRuntime.Children.Add(_nineSliceRuntime);
            }

            {
                _textRuntime = new()
                {
                    BitmapFont = GumUI.MontserratLarge,
                    WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer,
                    HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Text = text,
                    Color = Color.Black
                };
                UpdateTextRuntime();
                _containerRuntime.Children.Add(_textRuntime);
            }

            _gumDrawer = new(_containerRuntime) { Position = bounds.Center };
        }
        public void Update()
        {
            if (Running)
            {
                var controlStates = Globals.ControlState;
                if (controlStates.CursorSelectState == Controller.SelectStates.Pressed && _bounds.Contains(controlStates.CursorPosition))
                {
                    _pressedTime = _pressedPeriod;
                    _state = States.Pressed;
                    UpdateNineSlice();
                    UpdateTextRuntime();
                    _action(_parent);
                }
            }

            if (_state == States.Pressed && _pressedTime <= 0)
            {
                _state = States.Released;
                UpdateNineSlice();
                UpdateTextRuntime();
            }

            if (_pressedTime > 0)
                _pressedTime -= Globals.GameTime.GetElapsedSeconds();
        }
    }
}
