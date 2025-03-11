using BreakoutExtreme.Utility;
using MonoGame.Extended;
using MonoGameGum.GueDeriving;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public class TutorialPopup : IUpdate
    {
        private const float _runPeriod = 15;
        private readonly static Color _textColor = Color.Black;
        private float _runTime;
        private readonly ContainerRuntime _containerRuntime;
        private readonly TextRuntime _textRuntime;
        private readonly GumDrawer _gumDrawer;
        private RunningStates _state;
        private bool _initialized;
        private readonly Features.Appear _appear;
        private readonly Features.Vanish _vanish;
        private readonly Features.Shine _shine;
        public RunningStates State => _state;
        public string Text
        {
            get => _textRuntime.Text;
            set
            {
                Debug.Assert(_initialized);
                _textRuntime.Text = value;
            }
        }
        public GumDrawer GetGumDrawer() => _gumDrawer;
        public void Start()
        {
            Debug.Assert(_initialized);
            _gumDrawer.Visibility = 1;
            _appear.Start();
            _vanish.Stop();
            _runTime = 0;
            _state = RunningStates.Starting;
        }
        public void Stop()
        {
            Debug.Assert(_initialized);
            _gumDrawer.Visibility = 1;
            _appear.Stop();
            _vanish.Start();
            _runTime = 0;
            _state = RunningStates.Stopping;
        }
        public void ForceStart()
        {
            Debug.Assert(_initialized);
            _gumDrawer.Visibility = 1;
            _appear.Stop();
            _vanish.Stop();
            _runTime = _runPeriod;
            _state = RunningStates.Running;
        }
        public void ForceStop()
        {
            Debug.Assert(_initialized);
            _gumDrawer.Visibility = 0;
            _appear.Stop();
            _vanish.Stop();
            _runTime = 0;
            _state = RunningStates.Waiting;
        }
        public void Reset()
        {
            Debug.Assert(!_initialized);
            _initialized = true;
            _shine.Start();
            ForceStop();
        }
        public void Update()
        {
            Debug.Assert(_initialized);
            if (_state == RunningStates.Starting && !_appear.Running)
                ForceStart();
            if (_state == RunningStates.Stopping && !_vanish.Running)
                ForceStop();
            if (_state == RunningStates.Running)
            {
                if (_runTime > 0)
                    _runTime -= Globals.GameTime.GetElapsedSeconds();
                else
                    Stop();
            }
        }
        public TutorialPopup()
        {
            _initialized = false;

            _textRuntime = new()
            {
                X = 0,
                XUnits = Gum.Converters.GeneralUnitType.PixelsFromMiddle,
                XOrigin = RenderingLibrary.Graphics.HorizontalAlignment.Center,
                Y = 55,
                YUnits = Gum.Converters.GeneralUnitType.Percentage,
                YOrigin = RenderingLibrary.Graphics.VerticalAlignment.Center,
                Width = 0,
                WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer,
                Height = 0,
                HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToChildren,
                Text = "Hello World",
                HorizontalAlignment = RenderingLibrary.Graphics.HorizontalAlignment.Center,
                BitmapFont = GumUI.MontserratLarger,
                Red = _textColor.R,
                Green = _textColor.G,
                Blue = _textColor.B,
            };

            _containerRuntime = new()
            {
                Width = Globals.PlayAreaBounds.Width - 4 * Globals.GameBlockSize,
                Height = Globals.PlayAreaBounds.Height,
            };
            _containerRuntime.Children.Add(_textRuntime);

            _gumDrawer = new GumDrawer(_containerRuntime)
            {
                Layer = Layers.Shadow,
            };
            _vanish = new();
            _appear = new() { DelayPeriod = 3.5f };
            _shine = new();
            _gumDrawer.ShaderFeatures.Add(_vanish);
            _gumDrawer.ShaderFeatures.Add(_appear);
            _gumDrawer.ShaderFeatures.Add(_shine);
        }
    }
}
