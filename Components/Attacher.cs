using MonoGame.Extended.Collections;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Linq;
using MonoGame.Extended;
using System;

namespace BreakoutExtreme.Components
{
    public class Attacher<T>(T parent)  where T : class, IMovable
    {
        private readonly T _parent = parent;
        private readonly Bag<Node> _nodes = [];
        private readonly struct Node(T movable, Vector2 displacement)
        {
            public readonly T Movable = movable;
            public readonly Vector2 Displacement = displacement;
        }
        public void Attach(T other)
        {
            Debug.Assert(_nodes.All(x => x.Movable != other));
            _nodes.Add(new Node(movable: other, displacement: other.Position - _parent.Position));
        }
        public void Detach(T other)
        {
            Debug.Assert(_nodes.Any(x => x.Movable == other));
            for (var i = 0; i < _nodes.Count; i++)
            {
                var node = _nodes[i];
                if (node.Movable == other)
                {
                    _nodes.RemoveAt(i);
                    return;
                }
            }
        }
        public void UpdatePositions()
        {
            for (var i = 0; i < _nodes.Count; ++i)
            {
                var node = _nodes[i];
                var movable = node.Movable;
#if DEBUG
                if (movable is Collider collider)
                    Debug.Assert(collider.Velocity == Vector2.Zero && collider.Acceleration == Vector2.Zero);
#endif
                movable.Position = _parent.Position + node.Displacement;
            }
        }
    }
}
