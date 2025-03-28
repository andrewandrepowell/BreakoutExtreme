﻿using MonoGame.Extended.ECS;
using System;
using Microsoft.Xna.Framework;
using BreakoutExtreme.Utility;
using System.Diagnostics;
using MonoGame.Extended;


namespace BreakoutExtreme.Components
{
    public partial class Paddle : IUpdate, IRemoveEntity, IDestroyed
    {
        private static readonly Action<Collider.CollideNode> _collideAction = (Collider.CollideNode node) => ((Paddle)node.Current.Parent).ServiceCollision(node);
        private readonly Animater _animater;
        private readonly Collider _collider;
        private readonly Particler _particler;
        private readonly LaserGlower _laserGlower;
        private readonly MoveToTarget _moveToTarget;
        private readonly Empower _empower;
        private readonly Features.LimitedFlash _limitedFlash;
        private readonly Features.Vanish _vanish;
        private readonly Features.FloatUp _floatUp;
        private readonly Features.Shake _shake;
        private readonly Features.ParticlerGlow _particlerGlow;
        private Entity _entity;
        private Shadow _shadow;
        private bool _initialized;
        private States _state;
        private PlayArea _parent;
        private Ball _attachedBall;
        private bool _attached;
        private Sounder _sounder;
        private float _lockedYPosition;
        private bool _lockedY;
        private void ServiceCollision(Collider.CollideNode node)
        {
            if (!_initialized)
                return;
            if (node.Other.Parent is Wall)
            {
                node.CorrectPosition();
            }
            else if (node.Other.Parent is Ball ball)
            {
                var ballCollider = ball.GetCollider();
                ballCollider.Position = new Vector2(
                    x: ballCollider.Position.X + (_collider.Velocity.X == 0? node.PenetrationVector.X:0),
                    y: ballCollider.Position.Y + node.PenetrationVector.Y);

                if (_collider.Velocity.X != 0)
                {
                    _collider.Position = new Vector2(
                        x: _collider.Position.X - node.PenetrationVector.X,
                        y: _collider.Position.Y);
                }
            }
            if (_lockedY && _collider.Position.Y != _lockedYPosition)
            {
#if DEBUG
                Console.WriteLine($"Paddle's Y Position changed. Correction Applied.");
#endif
                _collider.Position = new(_collider.Position.X, _lockedYPosition);
            }
            _moveToTarget.ServiceCollision(node);
        }
        public enum States { Active, Despawning, Destroying, Destroyed }
        public Animater GetAnimater() => _animater;
        public Collider GetCollider() => _collider;
        public Particler GetParticler() => _particler;
        public float TargetToMoveTo => _moveToTarget.Target;
        public bool RunningMoveToTarget => _moveToTarget.Running;
        public bool Destroyed => _state == States.Destroyed;
        public States State => _state;
        public bool Initialized => _initialized;
        public bool Attached => _attached;
        public void Attach(Ball ball)
        {
            Debug.Assert(_initialized);
            Debug.Assert(_state == States.Active);
            Debug.Assert(!_attached);
            var ballCollider = ball.GetCollider();
            ball.Attach();
            ballCollider.Position = _collider.Position + new Vector2(
                x: _collider.Size.Width / 2,
                y: -((CircleF)ballCollider.Bounds).Radius);
            _collider.GetAttacher().Attach(ballCollider);
            _attachedBall = ball;
            _attached = true;
        }
        public void Detach()
        {
            Debug.Assert(_initialized);
            Debug.Assert(_state == States.Active);
            Debug.Assert(_attached);
            _attachedBall.Detach();
            var ballCollider = _attachedBall.GetCollider();
            _collider.GetAttacher().Detach(ballCollider);
            _attached = false;
        }
        public float TargetThreshold => _moveToTarget.Threshold;
        public float AccelerationToTarget
        {
            get => _moveToTarget.Acceleration;
            set => _moveToTarget.Acceleration = value;
        }
        public void StartMoveToTarget(float x)
        {
            Debug.Assert(_initialized);
            Debug.Assert(_state == States.Active);
            _moveToTarget.Start(x);
        }
        public void StopMoveToTarget() 
        {
            Debug.Assert(_initialized);
            Debug.Assert(_state == States.Active);
            _moveToTarget.Stop(); 
        }
        public void LaserGlow()
        {
            Debug.Assert(_initialized);
            Debug.Assert(_state == States.Active);
            _laserGlower.Start();
        }
        public bool Empowered => _empower.Running;
        public void StartEmpower()
        {
            Debug.Assert(_initialized);
            Debug.Assert(_state == States.Active);
            _empower.Start();
        }
        public void RunBounceEffects()
        {
            Debug.Assert(_initialized);
            Debug.Assert(_state == States.Active);
            _sounder.Play(Sounder.Sounds.Paddle);
        }
        public void Spawn()
        {
            Debug.Assert(_initialized);
            Debug.Assert(_state == States.Active);
            _limitedFlash.Start();
        }
        public void Despawn()
        {
            Debug.Assert(_initialized);
            Debug.Assert(_state == States.Active);
            Debug.Assert(!_attached);
            _floatUp.Start();
            _vanish.Start();
            _shadow.Start();
            _state = States.Despawning;
        }
        public void LockY()
        {
            Debug.Assert(!_lockedY);
            _lockedYPosition = _collider.Position.Y;
            _lockedY = true;
        }
        public void Destroy()
        {
            Debug.Assert(_initialized);
            Debug.Assert(_state == States.Active);
            Debug.Assert(!_attached);
            _animater.Play(_sizeConfig.DeadAnimation);
            _vanish.Start();
            _shake.Start();
            _shadow.Start();
            _sounder.Play(Sounder.Sounds.PaddleBreak);
            _state = States.Destroying;
        }
        public void Reset(Entity entity)
        {
            Debug.Assert(!_initialized);
            _entity = entity;
            _size = Sizes.Normal;
            _animater.Visibility = 1;
            _shadow = Globals.Runner.CreateShadow(_animater);
            _laserGlower.Reset();
            _empower.Reset();
            _floatUp.Stop();
            _shake.Stop();
            _state = States.Active;
            _lockedY = false;
            _initialized = true;
            UpdateSizeConfig();
        }
        public Paddle()
        {
            _animater = new();
            _collider = new(bounds: null, parent: this, action: _collideAction);
            _particler = new(Particler.Particles.Empowered){ Disposable = false };
            _particler.Stop();
            _moveToTarget = new(this);
            _laserGlower = new(this);
            _empower = new(this);
            _limitedFlash = new();
            _floatUp = new();
            _vanish = new();
            _shake = new();
            _particlerGlow = new(_particler) { Color = new Color(251, 213, 218) };
            _particlerGlow.Start();
            _animater.ShaderFeatures.Add(_limitedFlash);
            _animater.ShaderFeatures.Add(_floatUp);
            _animater.ShaderFeatures.Add(_vanish);
            _animater.ShaderFeatures.Add(_shake);
            _particler.ShaderFeatures.Add(_particlerGlow);
            _sounder = Globals.Runner.GetSounder();
            _lockedY = false;
            _initialized = false;
        }
        public void RemoveEntity()
        {
            Debug.Assert(_initialized);
            Globals.Runner.RemoveEntity(_entity);
            _shadow.RemoveEntity();
            _empower.RemoveEntity();
            _laserGlower.RemoveEntity();
            _initialized = false;
        }
        public void Update()
        {
            if (Globals.Paused)
                return;
            if (!_initialized)
                return;
            if ((_state == States.Despawning && _floatUp.State == RunningStates.Running && !_vanish.Running && !_shadow.Running) ||
                (_state == States.Destroying && !_vanish.Running && !_shake.Running && !_shadow.Running))
            {
                _animater.Visibility = 0;
                _state = States.Destroyed;
            }
            _moveToTarget.Update();
            UpdateSize();
            _empower.Update();
        }
    }
}
