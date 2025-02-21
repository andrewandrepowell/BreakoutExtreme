using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using MonoGameGum.GueDeriving;
using RenderingLibrary;
using RenderingLibrary.Graphics;
using System.Diagnostics;
using System;

namespace BreakoutExtreme.Components
{
    public partial class Menus
    {
        public class Window : IUpdate
        {
            private readonly static float _containerWidth = Globals.GameWindowBounds.Width * 2 / 3;
            private readonly static float _shiftAmount = (_containerWidth + Globals.GameWindowBounds.Width) / 2;
            private const float _shiftPeriod = 1;
            private readonly static Color _textColor = Color.Black;
            private readonly TextRuntime _textRuntime;
            private readonly NineSliceRuntime _bgNineSliceRuntime;
            private readonly ContainerRuntime _stackContainerRuntime;
            private readonly NineSliceRuntime _fgNineSliceRuntime;
            private readonly ContainerRuntime _containerRuntime;
            private RunningStates _state;
            private float _shiftTime;
            private Bag<Button> _buttons;
            private RectangleF _bounds;
            public ContainerRuntime GetContainerRuntime() => _containerRuntime;
            public string Text
            {
                get => _textRuntime.Text;
                set => _textRuntime.Text = value;
            }
            public string ID;
            public RunningStates State => _state;
            public void ForceStart() 
            {
                _shiftTime = 0;
                _containerRuntime.Visible = true;
                _containerRuntime.X = 0;
                _fgNineSliceRuntime.Alpha = 0;
                for (var i = 0; i < _buttons.Count; i++)
                    _buttons[i].Running = true;
                UpdateBounds();
                _state = RunningStates.Running;
            }
            public void ForceStop() 
            {
                _shiftTime = 0;
                _containerRuntime.Visible = false;
                _containerRuntime.X = _shiftAmount;
                _fgNineSliceRuntime.Alpha = 255;
                for (var i = 0; i < _buttons.Count; i++)
                    _buttons[i].Running = false;
                _state = RunningStates.Waiting;
            }
            public void Start() 
            {
                _shiftTime = _shiftPeriod;
                _containerRuntime.Visible = true;
                _containerRuntime.X = -_shiftAmount;
                _fgNineSliceRuntime.Alpha = 255;
                for (var i = 0; i < _buttons.Count; i++)
                    _buttons[i].Running = false;
                _state = RunningStates.Starting;
            }
            public void Stop() 
            { 
                _shiftTime = _shiftPeriod;
                _containerRuntime.Visible = true;
                _containerRuntime.X = 0;
                _fgNineSliceRuntime.Alpha = 0;
                for (var i = 0; i < _buttons.Count; i++)
                    _buttons[i].Running = false;
                _state = RunningStates.Stopping;
            }
            public void Add(Button button)
            {
                Debug.Assert(_state == RunningStates.Waiting);
                button.Running = false;
                _buttons.Add(button);
                _stackContainerRuntime.Children.Add(button.GetContainerRuntime());
            }
            public void UpdateBounds()
            {
                _bounds = new(
                    x: _containerRuntime.GetAbsoluteX(),
                    y: _containerRuntime.GetAbsoluteY(),
                    width: _containerRuntime.GetAbsoluteWidth(),
                    height: _containerRuntime.GetAbsoluteHeight());
            }
            public bool IsCursorInBounds() => _bounds.Contains(Globals.ControlState.CursorPosition);
            public void Update()
            {
                if (_state == RunningStates.Starting)
                {
                    var shiftRatio = _shiftTime / _shiftPeriod;
                    _containerRuntime.X = MathHelper.SmoothStep(0, -_shiftAmount, shiftRatio);
                    _fgNineSliceRuntime.Alpha = (int)MathHelper.SmoothStep(0.0f, 255.0f, shiftRatio);

                    if (_shiftTime > 0)
                        _shiftTime -= Globals.GameTime.GetElapsedSeconds();
                    else
                        ForceStart();
                }

                if (_state == RunningStates.Stopping)
                {
                    var shiftRatio = _shiftTime / _shiftPeriod;
                    _containerRuntime.X = MathHelper.SmoothStep(_shiftAmount, 0, shiftRatio);
                    _fgNineSliceRuntime.Alpha = (int)MathHelper.SmoothStep(255.0f, 0.0f, shiftRatio);

                    if (_shiftTime > 0)
                        _shiftTime -= Globals.GameTime.GetElapsedSeconds();
                    else
                        ForceStop();
                }

                for (var i = 0; i < _buttons.Count; i++)
                    _buttons[i].Update();
            }
            public Window()
            {
                _buttons = [];

                _containerRuntime = new ContainerRuntime()
                {
                    X = 0,
                    XUnits = Gum.Converters.GeneralUnitType.PixelsFromMiddle,
                    XOrigin = HorizontalAlignment.Center,
                    Y = 0,
                    YUnits = Gum.Converters.GeneralUnitType.PixelsFromSmall,
                    Width = Globals.GameBlockSize,
                    WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToChildren,
                    Height = Globals.GameBlockSize,
                    HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToChildren,
                };

                _textRuntime = new TextRuntime()
                {
                    Text = "Hello World",
                    BitmapFont = GumUI.MontserratSmall,
                    X = Globals.GameBlockSize,
                    XUnits = Gum.Converters.GeneralUnitType.PixelsFromSmall,
                    Y = Globals.GameBlockSize,
                    YUnits = Gum.Converters.GeneralUnitType.PixelsFromSmall,
                    Width = _containerWidth,
                    WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute,
                    Height = 0,
                    HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToChildren,
                    Red = _textColor.R,
                    Green = _textColor.G,
                    Blue = _textColor.B,
                };

                _stackContainerRuntime = new ContainerRuntime()
                {
                    X = 0,
                    XUnits = Gum.Converters.GeneralUnitType.PixelsFromMiddle,
                    XOrigin = HorizontalAlignment.Center,
                    YUnits = Gum.Converters.GeneralUnitType.PixelsFromSmall,
                    Width = 0,
                    WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToChildren,
                    Height = 0,
                    HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToChildren,
                    ChildrenLayout = Gum.Managers.ChildrenLayout.TopToBottomStack,
                };

                {
                    var sourceFile = Globals.ContentManager.Load<Texture2D>("animations/menu_0");
                    _bgNineSliceRuntime = new NineSliceRuntime()
                    {
                        SourceFile = sourceFile,
                        Width = 0,
                        Height = 0,
                        WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer,
                        HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer,
                    };
                    _fgNineSliceRuntime = new NineSliceRuntime()
                    {
                        SourceFile = sourceFile,
                        Width = 0,
                        Height = 0,
                        WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer,
                        HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer,
                        Alpha = 0,
                        Red = Color.Black.R,
                        Green = Color.Black.G,
                        Blue = Color.Black.B,
                    };
                }

                {
                    _stackContainerRuntime.Children.Add(_textRuntime);
                    _containerRuntime.Children.Add(_bgNineSliceRuntime);
                    _containerRuntime.Children.Add(_stackContainerRuntime);
                    _containerRuntime.Children.Add(_fgNineSliceRuntime);
                }

                ForceStop();
            }
        }
    }
}
