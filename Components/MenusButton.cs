using Microsoft.Xna.Framework.Graphics;
using MonoGameGum.GueDeriving;
using RenderingLibrary.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using RenderingLibrary;

namespace BreakoutExtreme.Components
{
    public partial class Menus
    {
        public class Button
        {
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
                _nineSliceRuntime.SourceFile = node.NineSliceTexture;
            }
            public ContainerRuntime GetContainerRuntime() => _containerRuntime;
            public string Text
            {
                get => _textRuntime.Text;
                set => _textRuntime.Text = value;
            }
            public Button()
            {
                _state = States.Released;

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
                    BitmapFont = new BitmapFont("fonts/montserrat/montserrat_0.fnt", SystemManagers.Default),
                    X = 0,
                    XUnits = Gum.Converters.GeneralUnitType.PixelsFromMiddle,
                    XOrigin = HorizontalAlignment.Center,
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
