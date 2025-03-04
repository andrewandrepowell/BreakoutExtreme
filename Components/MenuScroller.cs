using Microsoft.Xna.Framework.Graphics;
using MonoGameGum.GueDeriving;
using Microsoft.Xna.Framework;

namespace BreakoutExtreme.Components
{
    public partial class Menus
    {
        public class Scroller : IInteractable
        {
            private readonly static Color _textColor = Color.Black;
            private readonly ContainerRuntime _containerRuntime;
            
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
            public bool Running { get; set; } = false;
            public ContainerRuntime GetContainerRuntime() => _containerRuntime;
            public void Update()
            {

            }
            public Scroller()
            {
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
