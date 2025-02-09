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
        private readonly Features.Dash _dash;
        private readonly Features.FloatRight _floatRight;
        private Shadower _shadower;
        private bool _initialized;
        private Entity _entity;
        private bool _running;
        public bool Running => _running;
        public void Start()
        {
            Debug.Assert(_initialized);
            _scaleDown.Start();
            _shine.Start();
            _appear.Start();
        }
        public void Reset(Entity entity)
        {
            Debug.Assert(!_initialized);
            _entity = entity;
            _animater.Position = Globals.PlayAreaBounds.Center;
            _animater.Play(Animater.Animations.Cleared);
            _shadower = Globals.Runner.CreateShadower(_animater, _animater.Position);
            {
                var texturer = _shadower.GetTexturer();
                texturer.ShowBase = false;
                Debug.Assert(texturer.ShaderFeatures.Count == 0);
                texturer.ShaderFeatures.Add(_dash);
                texturer.ShaderFeatures.Add(_floatRight);
            }
            _scaleDown.MaxScale = 4;
            _scaleDown.MinScale = 1;
            _scaleDown.DelayPeriod = 0;
            _scaleDown.Period = 2;
            _scaleDown.Stop();
            _shine.DelayPeriod = 2;
            _shine.ActivePeriod = 2;
            _shine.Stop();
            _appear.DelayPeriod = 0;
            _appear.Period = 2;
            _appear.Stop();
            _dash.Stop();
            _floatRight.Smooth = true;
            _floatRight.DelayPeriod = 4;
            _floatRight.Period = 2;
            _floatRight.Stop();
            _vanish.DelayPeriod = 4;
            _vanish.Period = 2; 
            _vanish.Stop();
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

        }
        public Cleared()
        {
            _animater = new Animater();
            _scaleDown = new();
            _shine = new();
            _appear = new();
            _floatRight = new();
            _animater.ShaderFeatures.Add(_scaleDown);
            _animater.ShaderFeatures.Add(_shine);
            _animater.ShaderFeatures.Add(_appear);
            _animater.ShaderFeatures.Add(_floatRight);
            _running = false;
            _initialized = false;
        }
    }
}
