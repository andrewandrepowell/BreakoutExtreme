using Microsoft.Xna.Framework.Graphics;
using MonoGameGum.GueDeriving;
using RenderingLibrary.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using RenderingLibrary;
using MonoGame.Extended;

namespace BreakoutExtreme.Components
{
    public partial class Menus
    {
        public class Button
        {
            private const float _pressedPeriod = 0.5f;
            private readonly static States[] _states = Enum.GetValues<States>();
            private readonly static ReadOnlyDictionary<States, StateConfig> _stateConfigs = new(new Dictionary<States, StateConfig>() 
            {
                { States.Pressed, new(nineSliceAssetName: "animations/button_1") },
                { States.Released, new(nineSliceAssetName: "animations/button_0") }
            });
            private readonly static Color _textColor = Color.Black;
            private readonly TextRuntime _textRuntime;
            private readonly NineSliceRuntime _nineSliceRuntime;
            private readonly ContainerRuntime _containerRuntime;
            private readonly Dictionary<States, StateNode> _stateNodes;
            private RectangleF _bounds;
            private bool _running;
            private Action<object> _action;
            private object _parent;
            private class StateConfig(string nineSliceAssetName)
            {
                public readonly string NineSliceAssetName = nineSliceAssetName;
            }
            private class StateNode(Texture2D nineSliceTexture)
            {
                public readonly Texture2D NineSliceTexture = nineSliceTexture;
            }
            private States _state;
            private float _pressedTime;
            private enum States { Released, Pressed }
            private void UpdateStateProperties()
            {
                var node = _stateNodes[_state];
                var config = _stateConfigs[_state];
                _nineSliceRuntime.SourceFile = node.NineSliceTexture;
            }
            public ContainerRuntime GetContainerRuntime() => _containerRuntime;
            public string Text
            {
                get => _textRuntime.Text;
                set => _textRuntime.Text = value;
            }
            public bool Running
            {
                get => _running;
                set
                {
                    _running = value;
                    if (_running)
                        UpdateBounds();
                }
            }
            public Action<object> Action
            {
                get => _action;
                set => _action = value;
            }
            public object Parent
            {
                get => _parent;
                set => _parent = value;
            }
            public void UpdateBounds()
            {
                _bounds = new(
                    x: _containerRuntime.GetAbsoluteX(),
                    y: _containerRuntime.GetAbsoluteY(),
                    width: _containerRuntime.GetAbsoluteWidth(),
                    height: _containerRuntime.GetAbsoluteHeight());
            }
            public void Update()
            {
                if (_running)
                {
                    var controlStates = Globals.ControlState;
                    if (controlStates.CursorSelectState == Controller.SelectStates.Pressed && _bounds.Contains(controlStates.CursorPosition))
                    {
                        _pressedTime = _pressedPeriod;
                        if (_action != null) _action(_parent);
                        _state = States.Pressed;
                        UpdateStateProperties();
                    }
                }

                if (_state == States.Pressed && _pressedTime <= 0)
                {
                    _state = States.Released;
                    UpdateStateProperties();
                }

                if (_pressedTime > 0)
                    _pressedTime -= Globals.GameTime.GetElapsedSeconds();
            }
            public Button()
            {
                _state = States.Released;
                _running = false;

                _stateNodes = [];
                foreach (ref var state in _states.AsSpan())
                {
                    var config = _stateConfigs[state];
                    var texture = Globals.ContentManager.Load<Texture2D>(config.NineSliceAssetName);
                    _stateNodes.Add(state, new(nineSliceTexture: texture));
                }    

                _containerRuntime = new ContainerRuntime()
                {
                    X = 0,
                    XUnits = Gum.Converters.GeneralUnitType.PixelsFromMiddle,
                    XOrigin = HorizontalAlignment.Center,
                    Y = 0,
                    YUnits = Gum.Converters.GeneralUnitType.PixelsFromSmall,
                    Width = -Globals.GameBlockSize,
                    WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer,
                    Height = Globals.GameBlockSize,
                    HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToChildren
                };

                _nineSliceRuntime = new NineSliceRuntime()
                {
                    Width = 0,
                    Height = 0,
                    WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer,
                    HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer,
                };

                _textRuntime = new TextRuntime()
                {
                    Text = "Hello World",
                    BitmapFont = GumUI.MontserratSmall,
                    X = 0,
                    XUnits = Gum.Converters.GeneralUnitType.PixelsFromMiddle,
                    XOrigin = HorizontalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Y = Globals.GameBlockSize,
                    YUnits = Gum.Converters.GeneralUnitType.PixelsFromSmall,
                    Width = -Globals.GameBlockSize,
                    WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer,
                    Height = 0,
                    HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToChildren,
                    Red = _textColor.R,
                    Green = _textColor.G,
                    Blue = _textColor.B,
                };

                _containerRuntime.Children.Add(_nineSliceRuntime);
                _containerRuntime.Children.Add(_textRuntime);

                UpdateStateProperties();
            }
        }
    }
}
