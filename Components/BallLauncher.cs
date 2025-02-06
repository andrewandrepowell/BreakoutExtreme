using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Diagnostics;
using System;

namespace BreakoutExtreme.Components
{
    public partial class Ball
    {
        private class Launcher(Ball parent)
        {
            private readonly Ball _parent = parent;
            public bool Running { get; private set; } = false;
            public Vector2 Acceleration = new(0, -5000);
            public void ServiceCollision(Collider.CollideNode node)
            {
                Debug.Assert(_parent.State == States.Active);
                Debug.Assert(Running);
                var collider = _parent.GetCollider();

                // Handle rectangular bounce logic.
                {
                    if (_parent.State == States.Active && 
                        (node.Other.Parent is Wall || 
                         node.Other.Parent is Paddle || 
                        (node.Other.Parent is Brick brick && brick.State == Brick.States.Active) ||
                        (node.Other.Parent is DeathWall deathWall && !deathWall.Running)))
                    {
                        var otherBounds = node.Other.Bounds.BoundingRectangle;
                        var position = collider.Position;
                        var xBounce = position.Y >= otherBounds.Top && position.Y <= otherBounds.Bottom;
                        var yBounce = position.X >= otherBounds.Left && position.X <= otherBounds.Right;
                        var xDistance = MathHelper.Min(Math.Abs(position.X - otherBounds.Left), Math.Abs(position.X - otherBounds.Right));
                        var yDistance = MathHelper.Min(Math.Abs(position.Y - otherBounds.Top), Math.Abs(position.Y - otherBounds.Bottom));
                        var xClosest = xDistance <= yDistance;

                        if (xBounce || (!yBounce && xClosest))
                        {
                            if (Running)
                                Acceleration.X *= -1;
                            collider.Velocity.X *= -1;
                        }
                        else
                        {
                            if (Running)
                                Acceleration.Y *= -1;
                            collider.Velocity.Y *= -1;
                        }
                    }
                }

                // Handle circular bounce logic.
                {
                    if (_parent.State == States.Active && (
                        (node.Other.Parent is Cannon cannon && cannon.State == Cannon.States.Active)))
                    {
                        var cannonCollider = cannon.GetCollider();
                        var normalBasis = Vector2.Normalize(collider.Position - cannonCollider.Position);
                        var orthogBasis = new Vector2(x: -normalBasis.Y, y: normalBasis.X);
                        {
                            var normalMag = normalBasis.Dot(collider.Velocity);
                            var orthogMag = orthogBasis.Dot(collider.Velocity);
                            collider.Velocity = -normalMag * normalBasis + orthogMag * orthogBasis;
                        }
                        if (Running)
                        {
                            var normalMag = normalBasis.Dot(Acceleration);
                            var orthogMag = orthogBasis.Dot(Acceleration);
                            Acceleration = -normalMag * normalBasis + orthogMag * orthogBasis;
                        }
                    }
                }

                // Handle paddle bounce adjustment logic.
                {
                    if (_parent.State == States.Active && node.Other.Parent is Paddle)
                    {
                        if (Running && !Acceleration.EqualsWithTolerence(Vector2.Zero) && !collider.Velocity.EqualsWithTolerence(Vector2.Zero) && Acceleration.Y < 0)
                        {
                            var accelerationMagnitude = Acceleration.Length();
                            var accelerationDirection = Acceleration / accelerationMagnitude;
                            var accelerationRads = Math.Atan2(-accelerationDirection.Y, accelerationDirection.X);
                            var velocityMagitude = collider.Velocity.Length();

                            var paddleX = node.Other.Position.X + node.Other.Size.Width / 2;
                            var ballX = collider.Position.X;

                            var divCoef = MathHelper.Pi * 0.25f;
                            var mapCoef = MathHelper.Clamp((ballX - paddleX) / node.Other.Size.Width, -.5f, .5f) + 0.5f;
                            var radCoef = MathHelper.Lerp(MathHelper.Pi - divCoef, divCoef, mapCoef);

                            var newRads = (radCoef + accelerationRads) / 2;
                            var newDirection = new Vector2(x: (float)Math.Cos(newRads), y: (float)-Math.Sin(newRads));
                            var newAcceleration = accelerationMagnitude * newDirection;
                            var newVelocity = velocityMagitude * newDirection;
                            Acceleration = newAcceleration;
                            collider.Velocity = newVelocity;
                        }
                    }
                }

                // Handle damaging a brick logic.
                {
                    if (_parent.State == States.Active && 
                        node.Other.Parent is Brick brick && brick.State == Brick.States.Active)
                    {
                        brick.Damage();

                        // Update the score once the brick is destroyed.
                        if (brick.State != Brick.States.Active)
                            _parent._parent.UpdateScore(brick);
                    }
                }

                // Handle damaging a cannon logic.
                {
                    if (_parent.State == States.Active &&
                        node.Other.Parent is Cannon cannon && cannon.State == Cannon.States.Active)
                    {
                        cannon.Damage();

                        // Update the score once the cannon is destroyed.
                        if (cannon.State != Cannon.States.Active)
                            _parent._parent.UpdateScore(cannon);
                    }
                }

                // Handle collision with death wall.
                {
                    if (_parent.State == States.Active && 
                        node.Other.Parent is DeathWall deathWall && deathWall.Running)
                    {
                        _parent.Destroy();
                    }    
                }
            }
            public void Start()
            {
                Debug.Assert(!Running);
                Debug.Assert(!_parent._particler.Running);
                _parent._particler.Start();
                Running = true;
            }
            public void Stop()
            {
                Debug.Assert(Running);
                Debug.Assert(_parent._particler.Running);
                var collider = _parent._collider;
                collider.Acceleration = Vector2.Zero;
                collider.Velocity = Vector2.Zero;
                _parent._particler.Stop();
                Running = false;
            }
            public void Update()
            {
                if (Running)
                {
                    _parent._collider.Acceleration += Acceleration;
                }
            }
        }
    }
}
