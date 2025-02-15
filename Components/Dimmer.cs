using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGameGum.GueDeriving;

namespace BreakoutExtreme.Components
{
    public class Dimmer : IUpdate
    {
        private readonly ContainerRuntime _containerRuntime;
        private readonly ColoredRectangleRuntime _rectRuntime;
        private readonly GumDrawer _gumDrawer;
        private readonly static Color _color = Color.Black;
        private const float _period = 0.5f;
        private const float _maxVisibility = 0.75f;
        private float _time;
        private RunningStates _state;
        public RunningStates State => _state;
        public GumDrawer GetGumDrawer() => _gumDrawer;
        public void Start()
        {
            _time = _period;
            _gumDrawer.Visibility = 0;
            _state = RunningStates.Starting;
        }
        public void Stop()
        {
            _time = _period;
            _gumDrawer.Visibility = _maxVisibility;
            _state = RunningStates.Stopping;
        }
        public void ForceStart()
        {
            _gumDrawer.Visibility = _maxVisibility;
            _state = RunningStates.Running;
        }
        public void ForceStop()
        {
            _gumDrawer.Visibility = 0;
            _state = RunningStates.Waiting;
        }
        public void Update()
        {
            if (_state == RunningStates.Starting)
            {
                _gumDrawer.Visibility = MathHelper.Lerp(_maxVisibility, 0, MathHelper.Max(0, _time) / _period);
                if (_time <= 0) ForceStart();
            }
            if (_state == RunningStates.Stopping)
            {
                _gumDrawer.Visibility = MathHelper.Lerp(0, _maxVisibility, MathHelper.Max(0, _time) / _period);
                if (_time <= 0) ForceStop();
            }
            if (_state == RunningStates.Starting || _state == RunningStates.Stopping)
            {
                _time -= Globals.GameTime.GetElapsedSeconds();
            }
        }
        public Dimmer()
        {
            {
                _containerRuntime = new ContainerRuntime()
                {
                    X = 0,
                    Y = 0,
                    Width = Globals.GameWindowBounds.Width,
                    Height = Globals.GameWindowBounds.Height,
                };
            }

            {
                _rectRuntime = new ColoredRectangleRuntime()
                {
                    X = 0,
                    Y = 0,
                    Width = 0,
                    Height = 0,
                    Color = _color,
                    WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer,
                    HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer
                };
                _containerRuntime.Children.Add(_rectRuntime);
            }

            {
                _gumDrawer = new GumDrawer(_containerRuntime)
                {
                    Position = Globals.GameWindowBounds.Center,
                    Layer = Layers.Dimmer,
                    Visibility = 0,
                    Pausable = false
                };
            }

            _state = RunningStates.Waiting;
        }
    }
}
