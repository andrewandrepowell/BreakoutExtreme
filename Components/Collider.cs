using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public partial class Collider : ICollisionActor
    {
        private readonly Action<CollideNode> _action;
        private readonly Attacher _attacher;
        public Collider(IShapeF bounds, object parent, Action<CollideNode> action = null)
        {
            Bounds = bounds;
            Parent = parent;
            _action = action;
            _attacher = new(this);
        }
        public Vector2 Position 
        { 
            get => Bounds.Position;
            set
            {
                if (Bounds.Position == value) 
                    return;
                Bounds.Position = value;
                _attacher.UpdatePositions();
            }
        }
        public SizeF Size => Bounds.BoundingRectangle.Size;
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public float Slick = 0.80f;
        public IShapeF Bounds { get; }
        public object Parent { get; }
        public Attacher GetAttacher() => _attacher;

        public readonly struct CollideNode(Collider current, Collider other, Vector2 penetrationVector)
        {
            public readonly Vector2 PenetrationVector = penetrationVector;
            public readonly Collider Current = current;
            public readonly Collider Other = other;
            public void CorrectPosition()
            {
                Current.Position -= PenetrationVector;
            }
        }
        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (_action == null)
                return;
            _action(new CollideNode(
                current: this,
                other: (Collider)collisionInfo.Other,
                penetrationVector: collisionInfo.PenetrationVector));
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
                var displacement = Velocity * elapsedTime;
#if DEBUG
                var displacementMagnitude = displacement.Length();
                Debug.Assert(displacementMagnitude <= Globals.GameHalfBlockSize);
#endif
                Position += displacement;
            }

            // Acceleration is always reset.
            // The idea is that every entity with a collider
            /// must reapply acceleration.
            Acceleration = Vector2.Zero;
        }
    }
}
