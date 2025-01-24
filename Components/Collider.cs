using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public class Collider : ICollisionActor
    {
        private Action<Node> _action;
        public Vector2 Position { get => Bounds.Position; set => Bounds.Position = value; }
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public float Slick = 0.80f;
        public IShapeF Bounds { get; }
        public object Parent { get; }
        public Collider(IShapeF bounds, object parent, Action<Node> action = null)
        {
            Bounds = bounds;
            Parent = parent;
            _action = action;
        }
        public struct Node
        {
            public readonly Vector2 PenetrationVector;
            public readonly Collider Current;
            public readonly Collider Other;
            public Node(Collider current, Collider other, Vector2 penetrationVector)
            {
                Current = current;
                Other = other;
                PenetrationVector = penetrationVector;
            }
            public void CorrectPosition()
            {
                Current.Position -= PenetrationVector;
            }
        }
        public void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (_action == null)
                return;
            _action(new Node(
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
        }
    }
}
