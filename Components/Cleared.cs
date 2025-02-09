using BreakoutExtreme.Utility;
using MonoGame.Extended.ECS;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public class Cleared : IRemoveEntity, IUpdate
    {
        private readonly Animater _animater;
        private readonly Features.ScaleDown _scaleDown;
        private readonly Features.Shine _shine;
        private readonly Features.Appear _appear;
        private readonly Features.Vanish _vanish;
        private readonly Features.Vanish _vanishShadower;
        private readonly Features.Dash _dash;
        private readonly Features.FloatRight _floatRight;
        private readonly Features.FloatRight _floatRightShadower;
        private Shadower _shadower;
        private bool _initialized;
        private Entity _entity;
        private bool _running;
        public bool Running => _running;
        public Animater GetAnimater() => _animater;
        public void Start()
        {
            Debug.Assert(_initialized);
            _animater.Visibility = 1;
            _scaleDown.Start();
            _shine.Start();
            _appear.Start();
            _dash.Start();
            _floatRight.Start();
            _floatRightShadower.Start();
            _vanish.Start();
            _vanishShadower.Start();
        }
        public void Reset(Entity entity)
        {
            Debug.Assert(!_initialized);
            _entity = entity;
            _animater.Position = Globals.PlayAreaBounds.Center;
            _animater.Visibility = 0;
            _animater.Play(Animater.Animations.Cleared);
            _shadower = Globals.Runner.CreateShadower(_animater, _animater.Position);
            {
                var texturer = _shadower.GetTexturer();
                texturer.Scale = 2;
                texturer.ShowBase = false;
                texturer.Visibility = 0.5f;
                Debug.Assert(texturer.ShaderFeatures.Count == 0);
                texturer.ShaderFeatures.Add(_dash);
                texturer.ShaderFeatures.Add(_floatRightShadower);
                texturer.ShaderFeatures.Add(_vanishShadower);
            }
            _scaleDown.MaxScale = 6;
            _scaleDown.MinScale = 2;
            _scaleDown.DelayPeriod = 0;
            _scaleDown.Period = 2;
            _scaleDown.Stop();
            _shine.DelayPeriod = 2;
            _shine.ActivePeriod = 1.5f;
            _shine.Stop();
            _appear.DelayPeriod = 0;
            _appear.Period = 2;
            _appear.Stop();
            _dash.Reset(_animater);
            _dash.DelayPeriod = 3.5f;
            _dash.Direction = Directions.Right;
            _dash.Spread = 3;
            _dash.Stop();
            _floatRight.Smooth = true;
            _floatRight.DelayPeriod = 3.5f;
            _floatRight.Period = 0.5f;
            _floatRight.MaxDistance = Globals.PlayAreaBounds.Width;
            _floatRight.Stop();
            _floatRightShadower.Smooth = true;
            _floatRightShadower.DelayPeriod = 3.5f;
            _floatRightShadower.Period = 0.5f;
            _floatRightShadower.MaxDistance = Globals.PlayAreaBounds.Width;
            _floatRightShadower.Stop();
            _vanish.DelayPeriod = 3.5f;
            _vanish.Period = 0.5f;
            _vanish.Stop();
            _vanishShadower.DelayPeriod = 3.5f;
            _vanishShadower.Period = 0.5f;
            _vanishShadower.Stop();
            _running = false;
            _initialized = true;
        }
        public void RemoveEntity()
        {
            Debug.Assert(_initialized);
            Globals.Runner.RemoveEntity(_entity);
            _shadower.RemoveEntity();
            _initialized = false;
        }
        public void Update()
        {
            if (!_initialized)
                return;

            if (_running && 
                !_scaleDown.Running && 
                !_appear.Running &&  
                _floatRight.State == RunningStates.Running &&
                _floatRightShadower.State == RunningStates.Running &&
                !_vanish.Running &&
                !_vanishShadower.Running)
            {
                _shine.Stop();
                _dash.Stop();
                _running = false;
            }
        }
        public Cleared()
        {
            _animater = new Animater();
            _scaleDown = new();
            _shine = new();
            _appear = new();
            _floatRight = new();
            _floatRightShadower = new();
            _dash = new();
            _vanish = new();
            _vanishShadower = new();
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
