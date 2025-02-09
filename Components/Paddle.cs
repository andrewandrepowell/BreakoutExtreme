using MonoGame.Extended.ECS;
using MonoGame.Extended;
using System;
using Microsoft.Xna.Framework;
using BreakoutExtreme.Utility;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public partial class Paddle : IUpdate
    {
        private static readonly Rectangle _blockBounds = new(Globals.PlayAreaBlockBounds.X, Globals.PlayAreaBlockBounds.Y, 5, 1);
        private static readonly RectangleF _bounds = _blockBounds.ToBounds();
        private static readonly Action<Collider.CollideNode> _collideAction = (Collider.CollideNode node) => ((Paddle)node.Current.Parent).ServiceCollision(node);
        private readonly Animater _animater;
        private readonly Collider _collider;
        private readonly LaserGlower _laserGlower;
        private readonly MoveToTarget _moveToTarget;
        private Entity _entity;
        private Shadow _shadow;
        private bool _initialized;
        private void ServiceCollision(Collider.CollideNode node)
        {
            if (!_initialized)
                return;
            if (node.Other.Parent is Wall)
            {
                node.CorrectPosition();
            }
            else if (node.Other.Parent is Ball && _collider.Velocity.X != 0)
            {
                _collider.Position = new Vector2(
                    x: _collider.Position.X - node.PenetrationVector.X, 
                    y: _collider.Position.Y);
            }
            _moveToTarget.ServiceCollision(node);
        }
        public Animater GetAnimater() => _animater;
        public Collider GetCollider() => _collider;
        public float TargetToMoveTo => _moveToTarget.Target;
        public bool RunningMoveToTarget => _moveToTarget.Running;
        public void StartMoveToTarget(float x)
        {
            Debug.Assert(_initialized);
            _moveToTarget.Start(x);
        }
        public void StopMoveToTarget() 
        {
            Debug.Assert(_initialized);
            _moveToTarget.Stop(); 
        }
        public void LaserGlow()
        {
            Debug.Assert(_initialized);
            _laserGlower.Start();
        }
        public void Reset(Entity entity)
        {
            Debug.Assert(!_initialized);
            _entity = entity;
            _animater.Play(Animater.Animations.Paddle);
            _shadow = Globals.Runner.CreateShadow(_animater);
            _laserGlower.Reset();
            _initialized = true;
        }
        public Paddle()
        {
            _animater = new();
            _collider = new(bounds: _bounds, parent: this, action: _collideAction);
            _moveToTarget = new(this);
            _laserGlower = new(this);
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
            if (!_initialized)
                return;
            _moveToTarget.Update();
        }
    }
}
