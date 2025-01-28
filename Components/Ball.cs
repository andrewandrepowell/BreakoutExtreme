﻿using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using MonoGame.Extended.ECS;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public partial class Ball
    {
        private static readonly CircleF _bounds = new(Vector2.Zero, Globals.GameHalfBlockSize);
        private static readonly Action<Collider.CollideNode> _collideAction = (Collider.CollideNode node) => ((Ball)node.Current.Parent).ServiceCollision(node);
        private readonly Animater _animater;
        private readonly Collider _collider;
        private readonly Entity _entity;
        private readonly Launcher _launcher;
        private readonly Destroyer _destroyer;
        private readonly Shadow _shadow;
        private readonly Features.Vanish _vanish;
        private readonly Features.Shake _shake;
        private readonly Features.Flash _flash;
        private States _state = States.Active;
        private void ServiceCollision(Collider.CollideNode node)
        {
            if (node.Other.Parent is Wall || 
                node.Other.Parent is Paddle || 
                (node.Other.Parent is Brick brick && brick.State == Brick.States.Active))
            { 
                    node.CorrectPosition();
            }
            _launcher.ServiceCollision(node);
        }
        public Animater GetAnimater() => _animater;
        public Collider GetCollider() => _collider;
        public States State => _state;
        public bool LaunchRunning => _launcher.Running;
        public void StartLaunch()
        {
            Debug.Assert(_state == States.Active);
            _launcher.Start();
        }
        public void StopLaunch()
        {
            Debug.Assert(_state == States.Active);
            _launcher.Stop();
        }
        public void Destroy()
        {
            Debug.Assert(_state == States.Active);
            Debug.Assert(!_launcher.Running);
            _destroyer.Start();
            _state = States.Destroying;
        }
        public Ball(Entity entity, Action<Brick> brickDestroyedAction = null)
        {
            _animater = new();
            _vanish = new();
            _animater.ShaderFeatures.Add(_vanish);
            _shake = new();
            _animater.ShaderFeatures.Add(_shake);
            _flash = new();
            _animater.ShaderFeatures.Add(_flash);
            _animater.Play(Animater.Animations.Ball);
            _collider = new(bounds: _bounds, parent: this, action: _collideAction);
            _entity = entity;
            _launcher = new Launcher(this, brickDestroyedAction);
            _destroyer = new Destroyer(this);
            _shadow = Globals.Runner.CreateShadow(_animater);
        }
        public void RemoveEntity()
        {
            Globals.Runner.RemoveEntity(_entity);
            _shadow.RemoveEntity();
        }
        public void Update()
        {
            if (_state == States.Destroying && !_destroyer.Running)
                _state = States.Destroyed;
            _launcher.Update();
            _destroyer.Update();
        }
    }
}
