using Microsoft.Xna.Framework.Graphics;
using MonoGameGum.GueDeriving;
using Microsoft.Xna.Framework;
using RenderingLibrary;
using MonoGame.Extended;
using System;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public partial class Menus
    {
        public class Scroller : IInteractable
        {
            private readonly static Color _textColor = Color.Black;
            private readonly static Size _spriteSize = new(8, 22);
            private readonly ContainerRuntime _containerRuntime;
            private RectangleF _spriteBounds;
            private RectangleF _containerCBounds;
            private bool _running;
            private bool _held;
            private Action<object, float> _action;
            private object _parent;
            private const float _threshold = 4;

            // Container runtimes.
            private readonly TextRuntime _textRuntime;
            private readonly ContainerRuntime _containerARuntime;

            // Container A runtimes.
            private readonly NineSliceRuntime _nineSliceRuntime;
            private readonly ContainerRuntime _containerBRuntime;

            // Container B runtimes.
            private readonly ContainerRuntime[] _containerDRuntimes;
            private readonly ContainerRuntime _containerCRuntime;

            // Container C runtimes.
            private readonly SpriteRuntime _spriteRuntime;
            public bool Running
            {
                get => _running;
                set
                {
                    _running = value;
                    if (_running)
                    {
                        UpdateSpriteBounds();
                        UpdateContainerCBounds();
                    }
                    else
                    {
                        _held = false;
                    }
                }
            }
            public Action<object, float> Action
            {
                get => _action;
                set => _action = value;
            }
            public object Parent
            {
                get => _parent;
                set => _parent = value;
            }
            public float Percent
            {
                get => _spriteRuntime.X;
                set
                {
                    Debug.Assert(value >= 0 && value <= 100);
                    _spriteRuntime.X = value;
                }
            }
            public ContainerRuntime GetContainerRuntime() => _containerRuntime;
            public string Text
            {
                get => _textRuntime.Text;
                set => _textRuntime.Text = value;
            }
            private void UpdateSpriteBounds()
            {
                var fullX = _spriteRuntime.GetAbsoluteX();
                var fullY = _spriteRuntime.GetAbsoluteY();
                var fullWidth = _spriteRuntime.GetAbsoluteWidth();
                var fullHeight = _spriteRuntime.GetAbsoluteHeight();
                _spriteBounds = new(
                    x: fullX + (fullWidth - _spriteSize.Width) / 2,
                    y: fullY + (fullHeight - _spriteSize.Height) / 2,
                    width: _spriteSize.Width,
                    height: _spriteSize.Height);
            }
            private void UpdateContainerCBounds()
            {
                _containerCBounds = new(
                    x: _containerCRuntime.GetAbsoluteX(),
                    y: _containerCRuntime.GetAbsoluteY(),
                    width: _containerCRuntime.GetAbsoluteWidth(),
                    height: _containerCRuntime.GetAbsoluteHeight());
            }
            public void Update()
            {
                if (_running)
                {
                    var controlStates = Globals.ControlState;
                    var pressed = controlStates.CursorSelectState == Controller.SelectStates.Pressed;
                    var released = controlStates.CursorSelectState == Controller.SelectStates.Released || controlStates.CursorSelectState == Controller.SelectStates.None;

                    if (_held)
                    {
                        UpdateSpriteBounds();
                        var centerX = _spriteBounds.BoundingRectangle.Center.X;
                        var diffX = controlStates.CursorPosition.X - centerX;
                        if (Math.Abs(diffX) > _threshold)
                        {
                            var containerX = _containerCRuntime.GetAbsoluteX();
                            var containerWidth = _containerCRuntime.GetAbsoluteWidth();
                            var percentPerPixel = 100 / containerWidth;
                            var newPercent = MathHelper.Clamp((controlStates.CursorPosition.X - containerX) * percentPerPixel, 0, 100);
                            Percent = newPercent;
                            _action?.Invoke(_parent, Percent);
                        }
                    }

                    if (!_held && pressed && _containerCBounds.Contains(controlStates.CursorPosition))
                        _held = true;
                    if (_held && released)
                        _held = false;
                }
            }
            public Scroller()
            {
                _running = false;
                _held = false;

                _spriteRuntime = new()
                {
                    Texture = Globals.ContentManager.Load<Texture2D>("animations/bar_0"),
                    X = 50,
                    XUnits = Gum.Converters.GeneralUnitType.Percentage,
                    XOrigin = RenderingLibrary.Graphics.HorizontalAlignment.Center,
                    YUnits = Gum.Converters.GeneralUnitType.PixelsFromMiddle,
                    YOrigin = RenderingLibrary.Graphics.VerticalAlignment.Center,
                };

                _containerCRuntime = new()
                {
                    Height = 0,
                    HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToChildren,
                    Width = 100,
                    WidthUnits = Gum.DataTypes.DimensionUnitType.Ratio
                };
                _containerCRuntime.Children.Add(_spriteRuntime);

                _containerDRuntimes = new ContainerRuntime[2];
                for (var i = 0; i < _containerDRuntimes.Length; i++)
                {
                    _containerDRuntimes[i] = new ContainerRuntime()
                    {
                        Height = 0,
                        HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer,
                        Width = 12,
                    };
                }

                _containerBRuntime = new()
                {
                    ChildrenLayout = Gum.Managers.ChildrenLayout.LeftToRightStack,
                    Height = 0,
                    HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToChildren,
                    Width = 0,
                    WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer
                };
                _containerBRuntime.Children.Add(_containerDRuntimes[0]);
                _containerBRuntime.Children.Add(_containerCRuntime);
                _containerBRuntime.Children.Add(_containerDRuntimes[1]);

                _nineSliceRuntime = new()
                {
                    Height = 48,
                    SourceFile = Globals.ContentManager.Load<Texture2D>("animations/scroll_0"),
                    Width = 0,
                    WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer
                };

                _textRuntime = new()
                {
                    Text = "Hello World",
                    BitmapFont = GumUI.MontserratSmall,
                    X = 0,
                    XUnits = Gum.Converters.GeneralUnitType.PixelsFromSmall,
                    XOrigin = RenderingLibrary.Graphics.HorizontalAlignment.Left,
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

                _containerARuntime = new()
                {
                    Height = 0,
                    HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToChildren,
                    Width = 0,
                    WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer
                };
                _containerARuntime.Children.Add(_nineSliceRuntime);
                _containerARuntime.Children.Add(_containerBRuntime);

                _containerRuntime = new()
                {
                    ChildrenLayout = Gum.Managers.ChildrenLayout.TopToBottomStack,
                    X = 0,
                    XUnits = Gum.Converters.GeneralUnitType.PixelsFromMiddle,
                    XOrigin = RenderingLibrary.Graphics.HorizontalAlignment.Center,
                    Y = 0,
                    YUnits = Gum.Converters.GeneralUnitType.PixelsFromSmall,
                    Width = -2 * Globals.GameBlockSize,
                    WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer,
                    Height = 0,
                    HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToChildren
                };
                _containerRuntime.Children.Add(_textRuntime);
                _containerRuntime.Children.Add(_containerARuntime);
            }
        }
    }
}
