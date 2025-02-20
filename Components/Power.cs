using BreakoutExtreme.Utility;
using MonoGame.Extended.ECS;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace BreakoutExtreme.Components
{
    public class Power : IRemoveEntity, IUpdate, IDestroyed
    {
        private readonly static ReadOnlyDictionary<Powers, Config> _powerConfigs = new(new Dictionary<Powers, Config>() 
        {
            { Powers.Protection, new(Animater.Animations.PowerProtection, Animater.Animations.PowerProtectionDead) }
        });
        private readonly static RectangleF _bounds = new Rectangle(Globals.PlayAreaBlockBounds.X, Globals.PlayAreaBlockBounds.Y, 1, 1).ToBounds();
        private readonly static Action<Collider.CollideNode> _collideAction = (Collider.CollideNode node) => ((Power)node.Current.Parent).ServiceCollision(node);
        private readonly static Vector2 _acceleration = new(0, 500);
        private bool _initialized;
        private Entity _entity;
        private Powers _power;
        private Config _config;
        private Animater _animater;
        private Collider _collider;
        private States _state;
        private Glower _glower;
        private PlayArea _parent;
        private Features.LimitedFlash _flash;
        private Features.Vanish _vanish;
        private Features.Shake _shake;
        private class Config(Animater.Animations activeAnimations, Animater.Animations deadAnimation)
        {
            public readonly Animater.Animations ActiveAnimation = activeAnimations;
            public readonly Animater.Animations DeadAnimation = deadAnimation;
        }
        private void ServiceCollision(Collider.CollideNode node)
        {
            if (!_initialized)
                return;

            var deathWall = node.Other.Parent as DeathWall;
            var paddle = node.Other.Parent as Paddle;

            if (_state == States.Active && (deathWall != null || paddle != null))
            {
                node.CorrectPosition();
            }

            if (_state == States.Active && deathWall != null)
                Destroy();

            if (_state == States.Active && paddle != null)
                Despawn();
        }
        public enum States { Active, Despawning, Destroying, Destroyed }
        public States State => _state;
        public Powers GetPower() => _power;
        public bool Destroyed => _state == States.Destroyed;
        public Animater GetAnimater() => _animater;
        public Collider GetCollider() => _collider;
        public void Destroy()
        {
            Debug.Assert(_initialized);
            Debug.Assert(_state == States.Active);
            _animater.Play(_config.DeadAnimation);
            _collider.Velocity = Vector2.Zero;
            _shake.Start();
            _vanish.Start();
            _flash.Stop();
            _glower.Start();
            _state = States.Destroying;
        }
        public void Despawn()
        {
            Debug.Assert(_initialized);
            Debug.Assert(_state == States.Active);
            _collider.Velocity = Vector2.Zero;
            _shake.Stop();
            _vanish.Start();
            _flash.Start();
            _glower.Start();
            _state = States.Despawning;
        }
        public void Reset(Entity entity, Powers power, PlayArea parent)
        {
            Debug.Assert(!_initialized);
            _entity = entity;
            _power = power;
            _parent = parent;
            _config = _powerConfigs[power];
            _glower = Globals.Runner.CreateGlower(
                parent: _animater,
                color: Color.Yellow,
                minVisibility: 0.25f,
                maxVisibility: 0.75f,
                pulsePeriod: 2,
                pulseRepeating: true,
                appearVanishPeriod: 1);
            _animater.Play(_config.ActiveAnimation);
            _vanish.Stop();
            _shake.Stop();
            _flash.Start();
            _state = States.Active;
            _initialized = true;
        }
        public void RemoveEntity()
        {
            Debug.Assert(_initialized);
            Globals.Runner.RemoveEntity(_entity);
            _glower.RemoveEntity();
            _initialized = false;
        }
        public void Update()
        {
            if (!_initialized)
                return;
            if (Globals.Paused)
                return;
            if (_state == States.Active)
            {
                _collider.Acceleration += _acceleration;
            }
            if ((_state == States.Despawning || _state == States.Destroying) && !_vanish.Running && !_glower.Running)
            {
                _shake.Stop();
                _state = States.Destroyed;
            }
        }
        public Power()
        {
            _initialized = false;
            _animater = new Animater();
            _collider = new Collider(_bounds, this, _collideAction);
            _flash = new();
            _vanish = new();
            _shake = new();
            _animater.ShaderFeatures.Add(_flash);
            _animater.ShaderFeatures.Add(_vanish);
            _animater.ShaderFeatures.Add(_shake);
        }
    }
}
