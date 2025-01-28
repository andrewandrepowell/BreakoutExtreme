using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Diagnostics;
using System;

namespace BreakoutExtreme.Components
{
    public partial class Ball
    {
        private class Launcher(Ball parent, Action<Brick> brickDestroyedAction)
        {
            private readonly Ball _parent = parent;
            private readonly Action<Brick> _brickDestroyedAction = brickDestroyedAction;
            public bool Running { get; private set; } = false;
            public Vector2 Acceleration = new(0, -5000);
            public void ServiceCollision(Collider.CollideNode node)
            {
                var collider = _parent.GetCollider();

                // Handle bounce logic.
                {
                    if (node.Other.Parent is Wall || node.Other.Parent is Paddle || (node.Other.Parent is Brick brick && brick.State == Brick.States.Active))
                    {
                        if (!node.PenetrationVector.EqualsWithTolerence(Vector2.Zero))
                        {
                            if (Math.Abs(node.PenetrationVector.X) > (Math.Abs(node.PenetrationVector.Y)))
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
                }

                // Handle paddle bounce adjustment logic.
                {
                    if (node.Other.Parent is Paddle)
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
                    if (node.Other.Parent is Brick brick && brick.State == Brick.States.Active)
                    {
                        brick.Damage();
                        if (brick.State != Brick.States.Active)
                            _brickDestroyedAction(brick);
                    }
                }
                
            }
            public void Start()
            {
                Debug.Assert(!Running);
                Running = true;
            }
            public void Stop()
            {
                Debug.Assert(Running);
                Running = false;
            }
            public void Update()
            {
                if (Running)
                {
                    _parent.GetCollider().Acceleration += Acceleration;
                }
            }
        }
    }
}
