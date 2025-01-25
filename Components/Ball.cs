using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using BreakoutExtreme.Utility;
using MonoGame.Extended.ECS;

namespace BreakoutExtreme.Components
{
    public class Ball
    {
        private static readonly CircleF _bounds = new(Vector2.Zero, Globals.GameHalfBlockSize);
        private static readonly Action<Collider.CollideNode> _collideAction = (Collider.CollideNode node) => ((Ball)node.Current.Parent).ServiceCollision(node);
        private readonly Animater _animater;
        private readonly Collider _collider;
        private readonly Entity _entity;
        private void ServiceCollision(Collider.CollideNode node)
        {
            if (node.Other.Parent is Wall || node.Other.Parent is Paddle)
            { 
                if (!_collider.Velocity.EqualsWithTolerence(Vector2.Zero))
                {
                    if (!node.PenetrationVector.X.EqualsWithTolerance(0))
                    {
                        _collider.Acceleration.X *= -1;
                        _collider.Velocity.X *= -1;
                    }
                    else if (!node.PenetrationVector.Y.EqualsWithTolerance(0))
                    {
                        _collider.Acceleration.Y *= -1;
                        _collider.Velocity.Y *= -1;
                    }
                    node.CorrectPosition();
                }    
            }
        }
        public Animater GetAnimater() => _animater;
        public Collider GetCollider() => _collider;
        public Ball(Entity entity)
        {
            _animater = new();
            _animater.Play(Animater.Animations.Ball);
            _collider = new(bounds: _bounds, parent: this, action: _collideAction);
            _entity = entity;
        }
        public void RemoveEntity()
        {
            Globals.Runner.RemoveEntity(_entity);
        }
    }
}
