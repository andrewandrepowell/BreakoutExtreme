using MonoGame.Extended.ECS;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using BreakoutExtreme.Utility;
using System.Diagnostics;


namespace BreakoutExtreme.Components
{
    public partial class Brick : IUpdate, IRemoveEntity, IDestroyed
    {
        private const float _shakePeriod = 0.5f;
        private static readonly Vector2 _shineDirection = Vector2.Normalize(new Vector2(1, 1));
        private const float _shineRepeatPeriod = 7.5f;
        private const float _shineDelayControl = 0.01f;
        private const float _spawnFactor = 0.005f;
        private const float _spawnPeriod = 0.5f;
        private const float _spawnMaxScale = 3;
        private readonly Animater _animater;
        private readonly Collider _collider;
        private readonly Particler _particler;
        private readonly Features.Shake _shake;
        private readonly Features.Cracks _cracks;
        private readonly Features.Vanish _vanish;
        private readonly Features.Shine _shine;
        private readonly Features.ScaleDown _scaleDown;
        private readonly Features.LimitedFlash _limitedFlash;
        private readonly Features.Appear _appear;
        private bool _initialized;
        private Entity _entity;
        private Bricks _brick;
        private Shadow _shadow;
        private States _state;
        private BrickConfig _config;
        private Glower _glower;
        private Powers? _power;
        public Bricks GetBrick() => _brick;
        public Animater GetAnimater() => _animater;
        public Collider GetCollider() => _collider;
        public Particler GetParticler() => _particler;
        public int TotalHP;
        public int CurrentHP;
        public States State => _state;
        public Powers? Power => _power;
        public bool Destroyed => _state == States.Destroyed;
        public void Damage()
        {
            Debug.Assert(_initialized);
            Debug.Assert(CurrentHP > 0);
            Debug.Assert(State == States.Active);
            
            { 
                CurrentHP -= 1;
                _shake.Start();
                _cracks.Degree = (Features.Cracks.Degrees)(TotalHP - CurrentHP);
                _particler.Trigger();
            }

            if (CurrentHP == 0)
            {
                Destroy();
            }
        }
        public void Destroy()
        {
            Debug.Assert(_initialized);
            Debug.Assert(State == States.Active);
            Debug.Assert(!_vanish.Running);
            CurrentHP = 0;

            _shake.Start();
            _cracks.Degree = Features.Cracks.Degrees.None;
            _vanish.Start();
            _shadow.Start();
            _animater.Play(_config.DeadAnimation);

            _state = States.Destroying;
        }
        public void Reset(Entity entity, Bricks brick, Vector2 position, Powers? power = null)
        {
            Debug.Assert(!_initialized);
            _entity = entity;
            _brick = brick;
            _config = _brickConfigs[brick];
            _power = power;
            _shadow = Globals.Runner.CreateShadow(_animater);
            if (_config.Glow.HasValue)
                _glower = Globals.Runner.CreateGlower(
                    parent: _animater,
                    color: _config.Glow.Value,
                    minVisibility: 0,
                    maxVisibility: .75f,
                    pulsePeriod: 4,
                    pulseRepeating: true,
                    appearVanishPeriod: 1);
            _animater.Color = _config.Tint;
            _animater.Play(_config.ActiveAnimation);
            _collider.Bounds = _config.Bounds;
            _collider.Position = position;
            _shake.Stop();
            _shake.DelayPeriod = position.X * _spawnFactor;
            _shake.Period = _spawnPeriod;
            _shake.Start();
            _cracks.Degree = 0;
            _shine.Stop();
            _shine.RepeatPeriod = _shineRepeatPeriod;
            _shine.DelayPeriod = _shineDirection.Dot(position) * _shineDelayControl;
            _shine.Start();
            _scaleDown.Stop();
            _scaleDown.MaxScale = _spawnMaxScale;
            _scaleDown.DelayPeriod = position.X * _spawnFactor;
            _scaleDown.Period = _spawnPeriod;
            _scaleDown.Start();
            _limitedFlash.Stop();
            _limitedFlash.LimitedPeriod = position.X * _spawnFactor + _spawnPeriod;
            _limitedFlash.Start();
            _appear.Stop();
            _appear.Period = _spawnPeriod;
            _appear.DelayPeriod = position.X * _spawnFactor;
            _appear.Start();
            TotalHP = _config.TotalHP;
            CurrentHP = TotalHP;
            _state = States.Spawning;
            _initialized = true;
        }
        public Brick()
        {
            _initialized = false;
            _animater = new();
            _collider = new(bounds: null, parent: this);
            _particler = new(Particler.Particles.BrickBreak) { Layer = Layers.Foreground, Disposable = false };
            _shake = new();
            _animater.ShaderFeatures.Add(_shake);
            _cracks = new(_animater);
            _animater.ShaderFeatures.Add(_cracks);
            _vanish = new();
            _animater.ShaderFeatures.Add(_vanish);
            _shine = new();
            _animater.ShaderFeatures.Add(_shine);
            _scaleDown = new();
            _animater.ShaderFeatures.Add(_scaleDown);
            _limitedFlash = new();
            _animater.ShaderFeatures.Add(_limitedFlash);
            _appear = new();
            _animater.ShaderFeatures.Add(_appear);
        }
        public void RemoveEntity()
        {
            Debug.Assert(_initialized);
            Globals.Runner.RemoveEntity(_entity);
            _shadow.RemoveEntity();
            if (_config.Glow.HasValue)
                _glower.RemoveEntity();
            _initialized = false;
        }
        public void Update()
        {
            if (Globals.Paused)
                return;
            if (!_initialized)
                return;
            if (_state == States.Spawning && !_shake.Running && !_scaleDown.Running && !_limitedFlash.Running && !_appear.Running)
            {
                _shake.DelayPeriod = 0;
                _shake.Period = _spawnPeriod;
                _state = States.Active;
            }

            if (_state == States.Destroying && !_vanish.Running && !_shadow.Running)
                _state = States.Destroyed;
        }
    }
}
