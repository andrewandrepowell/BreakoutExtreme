using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using MonoGame.Extended.ECS;
using System.Diagnostics;
using BreakoutExtreme.Utility;

namespace BreakoutExtreme.Components
{
    public partial class Ball : IUpdate, IRemoveEntity, IDestroyed
    {
        private static readonly CircleF _bounds = new(Globals.PlayAreaBounds.Center, Globals.GameHalfBlockSize);
        private static readonly Action<Collider.CollideNode> _collideAction = (Collider.CollideNode node) => ((Ball)node.Current.Parent).ServiceCollision(node);
        private readonly Animater _animater;
        private readonly Collider _collider;
        private readonly Particler _particler;
        private readonly Features.Vanish _vanish;
        private readonly Features.Shake _shake;
        private readonly Features.Flash _flash;
        private readonly Features.LimitedFlash _limitedFlash;
        private readonly Features.FloatUp _floatUp;
        private readonly Launcher _launcher;
        private readonly Destroyer _destroyer;
        private PlayArea _parent;
        private Entity _entity;
        private Shadow _shadow;
        private Paddle _attachedPaddle;
        private States _state;
        private bool _initialized;
        private void ServiceCollision(Collider.CollideNode node)
        {
            if (State != States.Active || !_initialized)
                return;

            // Always correction position first.
            if (node.Other.Parent is Wall || 
                node.Other.Parent is Paddle || 
                (node.Other.Parent is Brick brick && brick.State == Brick.States.Active) ||
                (node.Other.Parent is Cannon cannon && cannon.State == Cannon.States.Active) ||
                (node.Other.Parent is Ball ball && ball.State == States.Active) ||
                (node.Other.Parent is DeathWall deathWall && !deathWall.Running))
            {
                node.CorrectPosition();
            }

            // Run other service collision handlers.
            _launcher.ServiceCollision(node);
        }
        public Animater GetAnimater() => _animater;
        public Collider GetCollider() => _collider;
        public Particler GetParticler() => _particler;
        public States State => _state;
        public bool LaunchRunning => _launcher.Running;
        public bool Destroyed => _state == States.Destroyed;
        public void AttachTo(Paddle paddle)
        {
            Debug.Assert(_initialized);
            Debug.Assert(_state == States.Active);
            var paddleCollider = paddle.GetCollider();
            paddleCollider.GetAttacher().Attach(_collider);
            _attachedPaddle = paddle;
            _state = States.Attached;
        }
        public void Detach()
        {
            Debug.Assert(_initialized);
            Debug.Assert(_state == States.Attached);
            _attachedPaddle.GetCollider().GetAttacher().Detach(_collider);
            _state = States.Active;
        }
        public void StartLaunch(Vector2? acceleration = null)
        {
            Debug.Assert(_initialized);
            Debug.Assert(_state == States.Active);
            _launcher.Start(acceleration);
        }
        public void StopLaunch()
        {
            Debug.Assert(_initialized);
            Debug.Assert(_state == States.Active);
            _launcher.Stop();
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
            Debug.Assert(!_launcher.Running);
            _floatUp.Start();
            _vanish.Start();
            _shadow.Start();
            _particler.Stop();
            _state = States.Despawning;
        }
        public void Destroy()
        {
            Debug.Assert(_initialized);
            Debug.Assert(_state == States.Active);
            _launcher.Stop();
            _destroyer.Start();
            _particler.Stop();
            _state = States.Destroying;
        }
        public void Reset(Entity entity, PlayArea parent)
        {
            _parent = parent;
            _entity = entity;
            _shadow = Globals.Runner.CreateShadow(_animater);
            _animater.Visibility = 1;
            _animater.Play(Animater.Animations.Ball);
            _particler.Stop();
            _floatUp.Stop();
            _state = States.Active;
            _initialized = true;
        }
        public Ball()
        {
            _initialized = false;
            _animater = new();
            _vanish = new();
            _animater.ShaderFeatures.Add(_vanish);
            _shake = new();
            _animater.ShaderFeatures.Add(_shake);
            _flash = new();
            _animater.ShaderFeatures.Add(_flash);
            _limitedFlash = new();
            _animater.ShaderFeatures.Add(_limitedFlash);
            _floatUp = new();
            _animater.ShaderFeatures.Add(_floatUp);
            _collider = new(bounds: _bounds, parent: this, action: _collideAction);
            _particler = new(Particler.Particles.BallTrail) { Disposable = false };
            _launcher = new Launcher(this);
            _destroyer = new Destroyer(this);
            
        }
        public void RemoveEntity()
        {
            Debug.Assert(_initialized);
            Globals.Runner.RemoveEntity(_entity);
            _shadow.RemoveEntity();
            _initialized = false;
        }
        public void Update()
        {
            if (Globals.Paused)
                return;
            if (!_initialized)
                return;
            if ((_state == States.Destroying && !_destroyer.Running) ||
                (_state == States.Despawning && _floatUp.State == RunningStates.Running && !_vanish.Running && !_shadow.Running))
            {
                _animater.Visibility = 0;
                _state = States.Destroyed;
            }
            _launcher.Update();
            _destroyer.Update();
        }
    }
}
