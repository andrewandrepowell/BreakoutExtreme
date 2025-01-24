using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;

namespace BreakoutExtreme.Components
{
    public class Collider : ICollisionActor
    {
        public Vector2 Position { get => Bounds.Position; set => Bounds.Position = value; }
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public float Slick = 0.80f;
        public float Bounce = 1f;
        public IShapeF Bounds { get; }
        public object Parent { get; }
        public Collider(IShapeF bounds, object parent)
        {
            Bounds = bounds;
            Parent = parent;
        }
        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            var otherCollider = collisionInfo.Other as Collider;
            if (!Velocity.EqualsWithTolerence(Vector2.Zero))
            {
                if (Bounce > 0)
                {
                    if (!collisionInfo.PenetrationVector.X.EqualsWithTolerance(0))
                    {
                        Acceleration.X *= -1;
                        Velocity.X *= -Bounce;
                    }
                    else if (!collisionInfo.PenetrationVector.Y.EqualsWithTolerance(0))
                    {
                        Acceleration.Y *= -1;
                        Velocity.Y *= -Bounce;
                    }
                }
                else
                {
                    Velocity = Vector2.Zero;
                }
                Position -= collisionInfo.PenetrationVector;
                Console.WriteLine($"Pena: {collisionInfo.PenetrationVector}. Velo: {otherCollider.Velocity}. Posi: {otherCollider.Position}");
            }
        }
        public void Update()
        {
            var elapsedTime = Globals.GameTime.GetElapsedSeconds();
            Velocity += Acceleration * elapsedTime;
            Velocity *= Slick;
            if (Velocity.EqualsWithTolerence(Vector2.Zero))
            {
                Velocity = Vector2.Zero;
            }
            else
            {
                //var displacement = Velocity * elapsedTime;
                //var displacementMagnitude = displacement.Length();
                //var displacementDirection = displacement / displacementMagnitude;
                //if (displacementMagnitude >= 8)
                //    displacementMagnitude = 8;
                //Position += displacementDirection * displacementMagnitude;
                Position += Velocity * elapsedTime;
            }
        }
    }
}
