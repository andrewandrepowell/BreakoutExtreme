using MonoGame.Extended.ECS;
using MonoGame.Extended;
using System;
using Microsoft.Xna.Framework;
using BreakoutExtreme.Utility;
using MonoGame.Extended.Collisions.Layers;

namespace BreakoutExtreme.Components
{
    public partial class Paddle
    {
        private static readonly Rectangle _blockBounds = new(Globals.PlayAreaBlockBounds.X, Globals.PlayAreaBlockBounds.Y, 5, 1); // y is set to 4 to resolve odd blazorgl compile bug.
        private static readonly RectangleF _bounds = _blockBounds.ToBounds();
        private static readonly Action<Collider.CollideNode> _collideAction = (Collider.CollideNode node) => ((Paddle)node.Current.Parent).ServiceCollision(node);
        private readonly Animater _animater;
        private readonly Collider _collider;
        private readonly Entity _entity;
        private readonly Shadow _shadow;
        private readonly MoveToTarget _moveToTarget;
        private void ServiceCollision(Collider.CollideNode node)
        {
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
        public MoveToTarget GetMoveToTarget() => _moveToTarget;
        public Paddle(Entity entity)
        {
            _animater = new();
            _animater.Play(Animater.Animations.Paddle);
            _collider = new(bounds: _bounds, parent: this, action: _collideAction);
            _entity = entity;
            _moveToTarget = new(this);
            _shadow = Globals.Runner.CreateShadow(_animater);
        }
        public void RemoveEntity()
        {
            Globals.Runner.RemoveEntity(_entity);
            _shadow.RemoveEntity();
        }
        public void Update()
        {
            _moveToTarget.Update();
        }
    }
}
