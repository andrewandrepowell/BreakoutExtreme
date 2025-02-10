using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum.GueDeriving;

namespace BreakoutExtreme.Components
{
    public class Menus : IUpdate
    {
        private readonly ContainerRuntime _containerRuntime;
        private readonly SpriteRuntime _breakOutSplashRuntime;
        private readonly GumDrawer _gumDrawer;
        private readonly Features.Appear _appear;
        private readonly Features.Vanish _vanish;
        private RunningStates _state;
        public GumDrawer GetGumDrawer() => _gumDrawer;
        public RunningStates State => _state;
        public void Start()
        {
            _gumDrawer.Visibility = 1;
            _appear.Start();
            _vanish.Stop();
            _state = RunningStates.Starting;
        }
        public void ForceStart()
        {
            _gumDrawer.Visibility = 1;
            _appear.Stop();
            _vanish.Stop();
            _state = RunningStates.Running;
        }
        public void Stop()
        {
            _gumDrawer.Visibility = 1;
            _appear.Stop();
            _vanish.Start();
            _state = RunningStates.Stopping;
        }
        public void ForceStop()
        {
            _gumDrawer.Visibility = 0;
            _appear.Stop();
            _vanish.Stop();
            _state = RunningStates.Waiting;
        }
        public void Update()
        {
            if (_state == RunningStates.Starting && !_appear.Running)
                ForceStart();
            if (_state == RunningStates.Stopping && !_vanish.Running)
                ForceStop();
        }
        public Menus()
        {
            _state = RunningStates.Waiting;

            _containerRuntime = new ContainerRuntime()
            {
                X = 0,
                Y = 0,
                Width = Globals.GameWindowBounds.Width,
                Height = Globals.GameWindowBounds.Height,
            };

            _breakOutSplashRuntime = new SpriteRuntime()
            {
                Texture = Globals.ContentManager.Load<Texture2D>("animations/break_out_0"),
                Height = 150,
                HeightUnits = Gum.DataTypes.DimensionUnitType.PercentageOfSourceFile,
                Width = 150,
                WidthUnits = Gum.DataTypes.DimensionUnitType.PercentageOfSourceFile,
                XOrigin = RenderingLibrary.Graphics.HorizontalAlignment.Center,
                XUnits = Gum.Converters.GeneralUnitType.PixelsFromMiddle,
                YOrigin = RenderingLibrary.Graphics.VerticalAlignment.Center,
                YUnits = Gum.Converters.GeneralUnitType.Percentage,
                Y = (float)100 / 3 
            };
            _containerRuntime.Children.Add(_breakOutSplashRuntime);

            _gumDrawer = new GumDrawer(_containerRuntime)
            {
                Position = Globals.GameWindowBounds.Center,
                Layer = Layers.Menus,
                Visibility = 0,
                Pausable = false
            };
            _appear = new();
            _vanish = new();
            _gumDrawer.ShaderFeatures.Add(_appear);
            _gumDrawer.ShaderFeatures.Add(_vanish);
        }
    }
}
