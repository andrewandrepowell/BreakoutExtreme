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
        private readonly Animater _animater;
        private readonly Collider _collider;
        private readonly Particler _particler;
        private readonly Entity _entity;
        private readonly Bricks _brick;
        private readonly Shadow _shadow;
        private readonly Features.Shake _shake;
        private readonly Features.Cracks _cracks;
        private readonly Features.Vanish _vanish;
        private readonly Features.Shine _shine;
        private readonly Features.ScaleDown _scaleDown;
        private readonly Features.LimitedFlash _limitedFlash;
        private readonly Features.Appear _appear;
        private States _state = States.Active;
        public Bricks GetBrick() => _brick;
        public Animater GetAnimater() => _animater;
        public Collider GetCollider() => _collider;
        public Particler GetParticler() => _particler;
        public readonly int TotalHP;
        public int CurrentHP;
        public States State => _state;
        public bool Destroyed => _state == States.Destroyed;
        public void Damage()
        {
            Debug.Assert(CurrentHP > 0 && State == States.Active);
            
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
            Debug.Assert(State == States.Active);
            Debug.Assert(!_vanish.Running);
            CurrentHP = 0;

            _shake.Start();
            _cracks.Degree = Features.Cracks.Degrees.None;
            _vanish.Start();
            _shadow.VanishStart();
            _animater.Play(_brickDeadAnimations[_brick]);

            _state = States.Destroying;
        }
        public Brick(Entity entity, Bricks brick, Vector2 position)
        {
            _animater = new();
            _animater.Play(_brickAnimations[brick]);
            _collider = new(bounds: _brickBounds[brick], parent: this) { Position = position };
            _particler = new(Particler.Particles.BrickBreak) { Layer = Layers.Foreground };
            _entity = entity;
            _brick = brick;
            _shadow = Globals.Runner.CreateShadow(_animater);
            {
                _shake = new() 
                { 
                    DelayPeriod = position.X * _spawnFactor, 
                    Period = _spawnPeriod
                };
                _shake.Start();
                _animater.ShaderFeatures.Add(_shake);
            }
            {
                _cracks = new(_animater);
                _animater.ShaderFeatures.Add(_cracks);
            }
            {
                _vanish = new();
                _animater.ShaderFeatures.Add(_vanish);
            }
            {
                _shine = new() 
                { 
                    RepeatPeriod = _shineRepeatPeriod, 
                    DelayPeriod = _shineDirection.Dot(position) * _shineDelayControl 
                };
                _shine.Start();
                _animater.ShaderFeatures.Add(_shine);
            }
            {
                _scaleDown = new() 
                { 
                    DelayPeriod = position.X * _spawnFactor, 
                    Period = _spawnPeriod
                };
                _scaleDown.Start();
                _animater.ShaderFeatures.Add(_scaleDown);
            }
            {
                _limitedFlash = new() 
                { 
                    LimitedPeriod = position.X * _spawnFactor + _spawnPeriod
                };
                _limitedFlash.Start();
                _animater.ShaderFeatures.Add(_limitedFlash);
            }
            {
                _appear = new() 
                { 
                    Period = _spawnPeriod, 
                    DelayPeriod = position.X * _spawnFactor
                };
                _appear.Start();
                _animater.ShaderFeatures.Add(_appear);
            }
            TotalHP = _brickTotalHPs[brick];
            CurrentHP = TotalHP;
            _state = States.Spawning;
        }
        public void RemoveEntity()
        {
            Globals.Runner.RemoveEntity(_entity);
            _shadow.RemoveEntity();
        }
        public void Update()
        {
            if (_state == States.Spawning && !_shake.Running && !_scaleDown.Running && !_limitedFlash.Running && !_appear.Running)
            {
                _shake.DelayPeriod = 0;
                _shake.Period = _spawnPeriod;
                _state = States.Active;
            }

            if (_state == States.Destroying && !_vanish.Running && !_shadow.VanishRunning)
                _state = States.Destroyed;
        }
    }
}
