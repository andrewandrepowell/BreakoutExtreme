﻿using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using BreakoutExtreme.Utility;

namespace BreakoutExtreme.Components
{
    public class Ball
    {
        private static readonly CircleF _bounds = new CircleF(Vector2.Zero, 8);
        private static readonly Action<Collider.Node> _collideAction = (Collider.Node node) => ((Ball)node.Current.Parent).ServiceCollision(node);
        private Animater _animater;
        private Collider _collider;
        private void ServiceCollision(Collider.Node node)
        {
            if (node.Other.Parent is Wall)
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
        public Ball()
        {
            _animater = new();
            _animater.Play(Animater.Animations.Ball);
            _collider = new(bounds: _bounds, parent: this, action: _collideAction);
        }
    }
}
