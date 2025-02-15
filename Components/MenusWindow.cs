using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGameGum.GueDeriving;
using RenderingLibrary;
using RenderingLibrary.Graphics;

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
            private readonly NineSliceRuntime _fgNineSliceRuntime;
            private readonly ContainerRuntime _containerRuntime;
            private RunningStates _state;
            private float _shiftTime;
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
                _state = RunningStates.Running;
            }
            public void ForceStop() 
            {
                _shiftTime = 0;
                _containerRuntime.Visible = false;
                _containerRuntime.X = _shiftAmount;
                _fgNineSliceRuntime.Alpha = 255;
                _state = RunningStates.Waiting;
            }
            public void Start() 
            {
                _shiftTime = _shiftPeriod;
                _containerRuntime.Visible = true;
                _containerRuntime.X = -_shiftAmount;
                _fgNineSliceRuntime.Alpha = 255;
                _state = RunningStates.Starting;
            }
            public void Stop() 
            {
                _shiftTime = _shiftPeriod;
                _containerRuntime.Visible = true;
                _containerRuntime.X = 0;
                _fgNineSliceRuntime.Alpha = 0;
                _state = RunningStates.Stopping;
            }
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
            }
            public Window()
            {
                _state = RunningStates.Waiting;

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
                    HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToChildren
                };

                _textRuntime = new TextRuntime()
                {
                    Text = "Hello World",
                    BitmapFont = new BitmapFont("fonts/montserrat/montserrat_0.fnt", SystemManagers.Default),
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
                    _containerRuntime.Children.Add(_bgNineSliceRuntime);
                    _containerRuntime.Children.Add(_textRuntime);
                    _containerRuntime.Children.Add(_fgNineSliceRuntime);
                }
            }
        }
    }
}
