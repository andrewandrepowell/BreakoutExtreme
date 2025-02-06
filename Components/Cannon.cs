using MonoGame.Extended.ECS;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using BreakoutExtreme.Utility;

namespace BreakoutExtreme.Components
{
    public partial class Cannon : IUpdate, IRemoveEntity, IDestroyed
    {
        private readonly static ReadOnlyDictionary<Cannons, ConfigNode> _cannonConfigNodes = new(new Dictionary<Cannons, ConfigNode>() 
        {
            { Cannons.Normal, new(active: Animater.Animations.Cannon, fire: Animater.Animations.CannonFire, dead: Animater.Animations.CannonDead, totalHP: 3) }
        });
        private const float _shakePeriod = 0.5f;
        private static readonly Vector2 _shineDirection = Vector2.Normalize(new Vector2(1, 1));
        private const float _shineRepeatPeriod = 7.5f;
        private const float _shineDelayControl = 0.01f;
        private const float _spawnFactor = 0.005f;
        private const float _spawnPeriod = 0.5f;
        private static readonly CircleF _bounds = new(Vector2.Zero, Globals.GameBlockSize);
        private readonly Animater _animater;
        private readonly Collider _collider;
        private readonly Features.Shake _shake;
        private readonly Features.Cracks _cracks;
        private readonly Features.Vanish _vanish;
        private readonly Features.ScaleDown _scaleDown;
        private readonly Features.LimitedFlash _limitedFlash;
        private readonly Features.Appear _appear;
        private readonly Features.Float _float;
        private bool _initialized;
        private Entity _entity;
        private Cannons _cannon;
        private Shadow _shadow;
        private ConfigNode _configNode;
        private int _totalHP;
        private int _currentHP;
        private States _state;
        private Firer _firer;
        private class ConfigNode(
            Animater.Animations active,
            Animater.Animations fire, 
            Animater.Animations dead,
            int totalHP)
        {
            public readonly Animater.Animations Active = active;
            public readonly Animater.Animations Fire = fire;
            public readonly Animater.Animations Dead = dead;
            public readonly int TotalHP = totalHP;
        }
        public enum Cannons { Normal }
        public enum States { Spawning, Active, Destroying, Destroyed }
        public States State => _state;
        public bool Destroyed => _state == States.Destroyed;
        public Animater GetAnimater() => _animater;
        public Collider GetCollider() => _collider;
        public void Damage()
        {
            Debug.Assert(_initialized);
            Debug.Assert(_currentHP > 0 && State == States.Active);

            {
                _currentHP -= 1;
                _shake.Start();
                _cracks.Degree = (Features.Cracks.Degrees)(_totalHP - _currentHP);
            }

            if (_currentHP == 0)
            {
                Destroy();
            }

        }
        public void Destroy()
        {
            Debug.Assert(_initialized);
            Debug.Assert(State == States.Active);
            Debug.Assert(!_vanish.Running);
            _currentHP = 0;

            _shake.Start();
            _cracks.Degree = Features.Cracks.Degrees.None;
            _vanish.Start();
            _shadow.VanishStart();
            _animater.Play(_configNode.Dead);

            _state = States.Destroying;
        }
        public void Reset(Entity entity, Cannons cannon, Vector2 position)
        {
            Debug.Assert(!_initialized);
            _entity = entity;
            _cannon = cannon;
            _configNode = _cannonConfigNodes[cannon];
            _animater.Play(_configNode.Active);
            _shadow = Globals.Runner.CreateShadow(_animater);
            _shake.DelayPeriod = position.X * _spawnFactor;
            _shake.Period = _spawnPeriod;
            _shake.Start();
            _scaleDown.DelayPeriod = position.X * _spawnFactor;
            _scaleDown.Period = _spawnPeriod;
            _scaleDown.Start();
            _limitedFlash.LimitedPeriod = position.X * _spawnFactor + _spawnPeriod;
            _limitedFlash.Start();
            _appear.Period = _spawnPeriod;
            _appear.DelayPeriod = position.X * _spawnFactor;
            _appear.Start();
            _float.DelayPeriod = Globals.Random.NextSingle();
            _float.Start();
            _collider.Position = position;
            _totalHP = _configNode.TotalHP;
            _currentHP = _configNode.TotalHP;
            _firer.Reset();
            _initialized = true;
            _state = States.Spawning;
        }
        public void RemoveEntity()
        {
            Debug.Assert(_initialized);
            Globals.Runner.RemoveEntity(_entity);
            _shadow.RemoveEntity();
            _firer.RemoveEntity();
            _initialized = false;
        }
        public void Update()
        {
            if (!_initialized)
                return;
            if (_state == States.Spawning && !_shake.Running && !_scaleDown.Running && !_limitedFlash.Running && !_appear.Running)
            {
                _shake.DelayPeriod = 0;
                _shake.Period = _spawnPeriod;
                _state = States.Active;
            }

            if (_state == States.Destroying && !_vanish.Running && !_shadow.VanishRunning)
                _state = States.Destroyed;

            _firer.Update();
        }
        public Cannon()
        {
            _initialized = false;
            _animater = new();
            _shake = new();
            _animater.ShaderFeatures.Add(_shake);
            _cracks = new(_animater);
            _animater.ShaderFeatures.Add(_cracks);
            _vanish = new();
            _animater.ShaderFeatures.Add(_vanish);
            _scaleDown = new();
            _animater.ShaderFeatures.Add(_scaleDown);
            _limitedFlash = new();
            _animater.ShaderFeatures.Add(_limitedFlash);
            _appear = new();
            _animater.ShaderFeatures.Add(_appear);
            _float = new();
            _animater.ShaderFeatures.Add(_float);
            _collider = new(_bounds, this);
            _firer = new(this);
        }
    }
}
