using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Diagnostics;
using System;

namespace BreakoutExtreme.Components
{
    public partial class Ball
    {
        public class Launcher(Ball parent)
        {
            private readonly Ball _parent = parent;
            public bool Launched { get; private set; } = false;
            public Vector2 Acceleration = new(0, -5000);
            public void ServiceCollision(Collider.CollideNode node)
            {
                var collider = _parent.GetCollider();
                
                if (node.Other.Parent is Wall || node.Other.Parent is Paddle)
                {
                    if (!node.PenetrationVector.EqualsWithTolerence(Vector2.Zero))
                    {
                        if (Math.Abs(node.PenetrationVector.X) > (Math.Abs(node.PenetrationVector.Y)))
                        {
                            if (Launched)
                                Acceleration.X *= -1;
                            collider.Velocity.X *= -1;
                        }
                        else
                        {
                            if (Launched)
                                Acceleration.Y *= -1;
                            collider.Velocity.Y *= -1;
                        }
                    }
                }

                if (node.Other.Parent is Paddle)
                {
                    if (Launched && !Acceleration.EqualsWithTolerence(Vector2.Zero) && !collider.Velocity.EqualsWithTolerence(Vector2.Zero) && Acceleration.Y < 0)
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
            public void Launch()
            {
                Debug.Assert(!Launched);
                Launched = true;
            }
            public void Update()
            {
                if (Launched)
                {
                    _parent.GetCollider().Acceleration += Acceleration;
                }
            }
        }
    }
}
