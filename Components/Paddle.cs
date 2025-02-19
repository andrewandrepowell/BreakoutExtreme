using MonoGame.Extended.ECS;
using MonoGame.Extended;
using System;
using Microsoft.Xna.Framework;
using BreakoutExtreme.Utility;
using System.Diagnostics;


namespace BreakoutExtreme.Components
{
    public partial class Paddle : IUpdate, IRemoveEntity, IDestroyed
    {
        private static readonly Rectangle _blockBounds = new(Globals.PlayAreaBlockBounds.Center.X, Globals.PlayAreaBlockBounds.Center.Y, 5, 1);
        private static readonly RectangleF _bounds = _blockBounds.ToBounds();
        private static readonly Action<Collider.CollideNode> _collideAction = (Collider.CollideNode node) => ((Paddle)node.Current.Parent).ServiceCollision(node);
        private readonly Animater _animater;
        private readonly Collider _collider;
        private readonly LaserGlower _laserGlower;
        private readonly MoveToTarget _moveToTarget;
        private readonly Features.LimitedFlash _limitedFlash;
        private readonly Features.Vanish _vanish;
        private readonly Features.FloatUp _floatUp;
        private readonly Features.Shake _shake;
        private Entity _entity;
        private Shadow _shadow;
        private bool _initialized;
        private States _state;
        private PlayArea _parent;
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
                    x: ballCollider.Position.X,
                    y: ballCollider.Position.Y + node.PenetrationVector.Y);

                if (_collider.Velocity.X != 0)
                {
                    _collider.Position = new Vector2(
                        x: _collider.Position.X - node.PenetrationVector.X,
                        y: _collider.Position.Y);
                }
            }
            _moveToTarget.ServiceCollision(node);
        }
        public enum States { Active, Despawning, Destroying, Destroyed }
        public Animater GetAnimater() => _animater;
        public Collider GetCollider() => _collider;
        public float TargetToMoveTo => _moveToTarget.Target;
        public bool RunningMoveToTarget => _moveToTarget.Running;
        public bool Destroyed => _state == States.Destroyed;
        public States State => _state;
        public bool Initialized => _initialized;
        public float TargetThreshold => _moveToTarget.Threshold;
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
            _floatUp.Start();
            _vanish.Start();
            _shadow.Start();
            _state = States.Despawning;
        }
        public void Destroy()
        {
            Debug.Assert(_initialized);
            Debug.Assert(_state == States.Active);
            _animater.Play(Animater.Animations.PaddleDead);
            _vanish.Start();
            _shake.Start();
            _shadow.Start();
            _state = States.Destroying;
        }
        public void Reset(Entity entity)
        {
            Debug.Assert(!_initialized);
            _entity = entity;
            _animater.Visibility = 1;
            _animater.Play(Animater.Animations.Paddle);
            _shadow = Globals.Runner.CreateShadow(_animater);
            _laserGlower.Reset();
            _floatUp.Stop();
            _shake.Stop();
            _state = States.Active;
            _initialized = true;
        }
        public Paddle()
        {
            _animater = new();
            _collider = new(bounds: _bounds, parent: this, action: _collideAction);
            _moveToTarget = new(this);
            _laserGlower = new(this);
            _limitedFlash = new();
            _floatUp = new();
            _vanish = new();
            _shake = new();
            _animater.ShaderFeatures.Add(_limitedFlash);
            _animater.ShaderFeatures.Add(_floatUp);
            _animater.ShaderFeatures.Add(_vanish);
            _animater.ShaderFeatures.Add(_shake);
            _initialized = false;
        }
        public void RemoveEntity()
        {
            Debug.Assert(_initialized);
            Globals.Runner.RemoveEntity(_entity);
            _shadow.RemoveEntity();
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
        }
    }
}
