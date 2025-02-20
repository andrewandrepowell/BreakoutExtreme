using BreakoutExtreme.Utility;
using MonoGame.Extended.ECS;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public class Splasher : IRemoveEntity, IUpdate
    {
        private readonly static ReadOnlyDictionary<Splashes, SplashConfig> _splashConfigs = new(new Dictionary<Splashes, SplashConfig>() 
        {
            { Splashes.Cleared, new(animation: Animater.Animations.Cleared) },
            { Splashes.GameEnd, new(animation: Animater.Animations.GameEnd) },
            { Splashes.GameStart, new(animation: Animater.Animations.GameStart, scale: 1.85f) },
        });
        private readonly Animater _animater;
        private readonly Features.ScaleDown _scaleDown;
        private readonly Features.Shine _shine;
        private readonly Features.Appear _appear;
        private readonly Features.Appear _appearShadow;
        private readonly Features.Vanish _vanish;
        private readonly Features.Vanish _vanishShadower;
        private readonly Features.Vanish _vanishShadow;
        private readonly Features.Dash _dash;
        private readonly Features.FloatRight _floatRight;
        private readonly Features.FloatRight _floatRightShadower;
        private readonly Features.FloatRight _floatRightShadow;
        private Shadow _shadow;
        private Shadower _shadower;
        private bool _initialized;
        private Entity _entity;
        private bool _running;
        private Splashes _splash;
        private SplashConfig _splashConfig;
        private class SplashConfig(Animater.Animations animation, float scale = 2)
        {
            public readonly Animater.Animations Animation = animation;
            public readonly float Scale = scale;
        }
        public enum Splashes { Cleared, GameEnd, GameStart }
        public bool Running => _running;
        public Animater GetAnimater() => _animater;
        public void Start()
        {
            Debug.Assert(_initialized);
            _animater.Visibility = 1;
            _shadow.GetTexturer().Visibility = 0.5f;
            _scaleDown.Start();
            _shine.Start();
            _appear.Start();
            _appearShadow.Start();
            _dash.Start();
            _floatRight.Start();
            _floatRightShadower.Start();
            _floatRightShadow.Start();
            _vanish.Start();
            _vanishShadower.Start();
            _vanishShadow.Start();
            _running = true;
        }
        public void Reset(Entity entity, Splashes splash)
        {
            Debug.Assert(!_initialized);
            _entity = entity;
            _splash = splash;
            _splashConfig = _splashConfigs[splash];
            _animater.Position = Globals.PlayAreaBounds.Center;
            _animater.Visibility = 0;
            _animater.Play(_splashConfig.Animation);
            _shadower = Globals.Runner.CreateShadower(_animater, _animater.Position);
            {
                var texturer = _shadower.GetTexturer();
                texturer.Scale = _splashConfig.Scale;
                texturer.ShowBase = false;
                texturer.Visibility = 1f;
                Debug.Assert(texturer.ShaderFeatures.Count == 0);
                texturer.ShaderFeatures.Add(_dash);
                texturer.ShaderFeatures.Add(_floatRightShadower);
                texturer.ShaderFeatures.Add(_vanishShadower);
            }
            _shadow = Globals.Runner.CreateShadow(_animater);
            {
                var texturer = _shadow.GetTexturer();
                texturer.Visibility = 0;
                texturer.Scale = _splashConfig.Scale;
                texturer.ShaderFeatures.Add(_floatRightShadow);
                texturer.ShaderFeatures.Add(_vanishShadow);
                texturer.ShaderFeatures.Add(_appearShadow);
            }
            _scaleDown.MaxScale = 6;
            _scaleDown.MinScale = _splashConfig.Scale;
            _scaleDown.DelayPeriod = 0;
            _scaleDown.Period = 2;
            _scaleDown.Stop();
            _shine.DelayPeriod = 2;
            _shine.ActivePeriod = 1.5f;
            _shine.Stop();
            _appear.DelayPeriod = 0;
            _appear.Period = 2;
            _appear.Stop();
            _appearShadow.DelayPeriod = 0;
            _appearShadow.Period = 2;
            _appearShadow.Stop();
            _dash.Reset(_animater);
            _dash.DelayPeriod = 3.5f;
            _dash.Direction = Directions.Right;
            _dash.Spread = 5;
            _dash.Stop();
            _floatRight.Smooth = true;
            _floatRight.DelayPeriod = 3.5f;
            _floatRight.Period = 0.5f;
            _floatRight.MaxDistance = Globals.PlayAreaBounds.Width / 2;
            _floatRight.Stop();
            _floatRightShadower.Smooth = true;
            _floatRightShadower.DelayPeriod = 3.5f;
            _floatRightShadower.Period = 0.5f;
            _floatRightShadower.MaxDistance = Globals.PlayAreaBounds.Width / 2;
            _floatRightShadower.Stop();
            _floatRightShadow.Smooth = true;
            _floatRightShadow.DelayPeriod = 3.5f;
            _floatRightShadow.Period = 0.5f;
            _floatRightShadow.MaxDistance = -Globals.PlayAreaBounds.Width / 2;
            _floatRightShadow.Stop();
            _vanish.DelayPeriod = 3.5f;
            _vanish.Period = 0.5f;
            _vanish.Stop();
            _vanishShadower.DelayPeriod = 3.5f;
            _vanishShadower.Period = 0.5f;
            _vanishShadower.Stop();
            _vanishShadow.DelayPeriod = 3.5f;
            _vanishShadow.Period = 0.5f;
            _vanishShadow.Stop();
            _running = false;
            _initialized = true;
        }
        public void RemoveEntity()
        {
            Debug.Assert(_initialized);
            Globals.Runner.RemoveEntity(_entity);
            _shadow.RemoveEntity();
            {
                var texturer = _shadow.GetTexturer();
                texturer.ShaderFeatures.Remove(_floatRightShadow);
                texturer.ShaderFeatures.Remove(_vanishShadow);
                texturer.ShaderFeatures.Remove(_appearShadow);
            }
            _shadower.RemoveEntity();
            _initialized = false;
        }
        public void Update()
        {
            if (Globals.Paused)
                return;
            if (!_initialized)
                return;

            if (_running && 
                !_scaleDown.Running && 
                !_appear.Running &&
                !_appearShadow.Running &&
                _floatRight.State == RunningStates.Running &&
                _floatRightShadower.State == RunningStates.Running &&
                _floatRightShadow.State == RunningStates.Running &&
                !_vanish.Running &&
                !_vanishShadower.Running &&
                !_vanishShadow.Running)
            {
                _shadow.GetTexturer().Visibility = 0;
                _shine.Stop();
                _dash.Stop();
                _running = false;
            }
        }
        public Splasher()
        {
            _animater = new Animater() { Layer = Layers.Foreground };
            _scaleDown = new();
            _shine = new();
            _appear = new();
            _appearShadow = new();
            _floatRight = new();
            _floatRightShadower = new();
            _floatRightShadow = new();
            _dash = new();
            _vanish = new();
            _vanishShadower = new();
            _vanishShadow = new();
            _animater.ShaderFeatures.Add(_scaleDown);
            _animater.ShaderFeatures.Add(_shine);
            _animater.ShaderFeatures.Add(_appear);
            _animater.ShaderFeatures.Add(_floatRight);
            _animater.ShaderFeatures.Add(_vanish);
            _running = false;
            _initialized = false;
        }
    }
}
