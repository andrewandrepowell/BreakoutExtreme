using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Collections;
using MonoGameGum.GueDeriving;
using RenderingLibrary;
using System;
using System.Diagnostics;
using System.Linq;


namespace BreakoutExtreme.Components
{
    public partial class Menus : IUpdate
    {
        private const float _breakOutSplashY = (float)50 / 3;
        private readonly Bag<Window> _windows;
        private readonly ContainerRuntime _containerRuntime;
        private readonly ContainerRuntime _menuContainerRuntime;
        private readonly SpriteRuntime _breakOutSplashRuntime;
        private readonly GumDrawer _gumDrawer;
        private readonly Features.Appear _appear;
        private readonly Features.Vanish _vanish;
        private RunningStates _state;
        private Window _window;
        public GumDrawer GetGumDrawer() => _gumDrawer;
        public RunningStates State => _state;
        public void Goto(string id = "")
        {
            for (var i = 0; i < _windows.Count; i++)
            {
                var window = _windows[i];
                if (window.ID == id)
                {
                    _window = window;
                    window.Start();
                }
                else if (window.State != RunningStates.Waiting)
                    window.Stop();
            }
        }
        public bool IsCursorInWindow()
        {
            if (_window == null)
                return false;
            return _window.IsCursorInBounds();
        }
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
            for (var i = 0; i < _windows.Count; i++)
                _windows[i].UpdateBounds();
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
        public void Add(Window window)
        {
            Debug.Assert(_windows.All(x=>x.ID != window.ID));
            _windows.Add(window);
            _menuContainerRuntime.Children.Add(window.GetContainerRuntime());
        }
        public void Update()
        {
            if (_state == RunningStates.Starting && !_appear.Running)
                ForceStart();
            if (_state == RunningStates.Stopping && !_vanish.Running)
                ForceStop();

            for (var i = 0; i < _windows.Count; i++)
                _windows[i].Update();
        }
        public Menus()
        {
            _windows = [];

            _containerRuntime = new ContainerRuntime()
            {
                X = 0,
                Y = 0,
                Width = Globals.GameWindowBounds.Width,
                WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute,
                Height = Globals.GameWindowBounds.Height,
                HeightUnits = Gum.DataTypes.DimensionUnitType.Absolute
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
                Y = _breakOutSplashY
            };

            _menuContainerRuntime = new()
            {
                X = 0,
                XUnits = Gum.Converters.GeneralUnitType.PixelsFromMiddle,
                XOrigin = RenderingLibrary.Graphics.HorizontalAlignment.Center,
                YUnits = Gum.Converters.GeneralUnitType.PixelsFromSmall,
                Width = 0,
                WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToContainer,
                HeightUnits = Gum.DataTypes.DimensionUnitType.Absolute
            };

            _containerRuntime.Children.Add(_breakOutSplashRuntime);
            _containerRuntime.Children.Add(_menuContainerRuntime);

            _menuContainerRuntime.Y = _breakOutSplashRuntime.GetAbsoluteBottom();
            _menuContainerRuntime.Height = Globals.GameWindowBounds.Height - _breakOutSplashRuntime.GetAbsoluteBottom();

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

            ForceStop();
        }
    }
}
