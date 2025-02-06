using BreakoutExtreme.Utility;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace BreakoutExtreme.Components
{
    public partial class Bomb : IUpdate, IRemoveEntity, IDestroyed
    {
        private readonly static ReadOnlyDictionary<Bombs, ConfigNode> _configNodes = new(new Dictionary<Bombs, ConfigNode>()
        {
            { Bombs.Normal, new(active: Animater.Animations.Bomb, dead: Animater.Animations.BombDead, totalHP: 3) }
        });
        private static readonly Action<Collider.CollideNode> _collideAction = (Collider.CollideNode node) => ((Bomb)node.Current.Parent).ServiceCollision(node);
        private static readonly CircleF _bounds = new(Vector2.Zero, Globals.GameHalfBlockSize);
        private const float _spawnFactor = 0.005f;
        private const float _spawnPeriod = 0.5f;
        private readonly Animater _animater;
        private readonly Collider _collider;
        private readonly Particler _particler;
        private readonly Features.Shake _shake;
        private readonly Features.Cracks _cracks;
        private readonly Features.Vanish _vanish;
        private readonly Features.LimitedFlash _limitedFlash;
        private readonly Features.Appear _appear;
        private bool _initialized;
        private Entity _entity;
        private Bombs _bomb;
        private Shadow _shadow;
        private ConfigNode _configNode;
        private int _totalHP;
        private int _currentHP;
        private States _state;
        private Detonater _detonater;
        private class ConfigNode(
            Animater.Animations active,
            Animater.Animations dead,
            int totalHP)
        {
            public readonly Animater.Animations Active = active;
            public readonly Animater.Animations Dead = dead;
            public readonly int TotalHP = totalHP;
        }
        private void ServiceCollision(Collider.CollideNode node)
        {
            if (!_initialized)
                return;

            if (_state == States.Active && node.Other.Parent is DeathWall deathWall)
            {
                Detonate();
            }
        }
        public enum Bombs { Normal }
        public enum States { Spawning, Active, Destroying, Destroyed, Detonating }
        public States State => _state;
        public bool Destroyed => _state == States.Destroyed;
        public Animater GetAnimater() => _animater;
        public Collider GetCollider() => _collider;
        public Particler GetParticler() => _particler;
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
        public void Detonate()
        {
            Debug.Assert(_state == States.Active);
            _detonater.Start();
            _state = States.Detonating;
        }
        public void RemoveEntity()
        {
            Debug.Assert(_initialized);
            Globals.Runner.RemoveEntity(_entity);
            _shadow.RemoveEntity();
            _initialized = false;
        }
        public void Reset(Entity entity, Bombs bomb, Vector2 position)
        {
            Debug.Assert(!_initialized);
            _entity = entity;
            _bomb = bomb;
            _configNode = _configNodes[bomb];
            _animater.Play(_configNode.Active);
            _shadow = Globals.Runner.CreateShadow(_animater);
            _shake.DelayPeriod = 0;
            _shake.Period = _spawnPeriod;
            _shake.Start();
            _limitedFlash.LimitedPeriod = _spawnPeriod;
            _limitedFlash.Start();
            _appear.Period = _spawnPeriod;
            _appear.Start();
            _collider.Position = position;
            _totalHP = _configNode.TotalHP;
            _currentHP = _configNode.TotalHP;
            _particler.Stop();
            _detonater.Reset();
            _state = States.Spawning;
            _initialized = true;
        }
        public void Update()
        {
            if (!_initialized)
                return;
            if (_state == States.Spawning && !_shake.Running && !_limitedFlash.Running && !_appear.Running)
            {
                _shake.DelayPeriod = 0;
                _shake.Period = _spawnPeriod;
                _state = States.Active;
            }

            if (_state == States.Destroying && !_vanish.Running && !_shadow.VanishRunning)
                _state = States.Destroyed;

            if (_state == States.Detonating && _detonater.State == Detonater.States.Finished)
                Destroy();

            _detonater.Update();
        }
        public Bomb()
        {
            _initialized = false;
            _animater = new();
            _shake = new();
            _animater.ShaderFeatures.Add(_shake);
            _cracks = new(_animater);
            _animater.ShaderFeatures.Add(_cracks);
            _vanish = new();
            _animater.ShaderFeatures.Add(_vanish);
            _limitedFlash = new();
            _animater.ShaderFeatures.Add(_limitedFlash);
            _appear = new();
            _animater.ShaderFeatures.Add(_appear);
            _collider = new(_bounds, this);
            _particler = new() { Disposable = false };
            _detonater = new(this);
        }
    }
}
