using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;
using System.Diagnostics;
using System.Xml.Linq;

namespace BreakoutExtreme.Components
{
    public partial class Collider : ICollisionActor, IMovable
    {
        private readonly Action<CollideNode> _action;
        private readonly Attacher<Collider> _attacher;
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
        public IShapeF Bounds { get; set; }
        public object Parent { get; }
        public Attacher<Collider> GetAttacher() => _attacher;

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
            if (Globals.Paused)
                return;
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
                if (displacementMagnitude > Globals.GameBlockSize)
                {
                    Console.WriteLine($"Current Velocity: {Velocity}, Acceleration: {Acceleration}, AccelMag: {Acceleration.Length()}");
                    Debug.Assert(false);
                }
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
