using MonoGame.Extended.Collections;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using System.Linq;

namespace BreakoutExtreme.Components
{
    public partial class Collider
    {
        public class Attacher(Collider parent)
        {
            private readonly Collider _parent = parent;
            private readonly Bag<Node> _nodes = [];
            private readonly struct Node(Collider collider, Vector2 displacement)
            {
                public readonly Collider Collider = collider;
                public readonly Vector2 Displacement = displacement;
            }
            public void Attach(Collider other)
            {
                Debug.Assert(_nodes.All(x=>x.Collider != other));
                _nodes.Add(new Node(collider: other, displacement: other.Position - _parent.Position));
            }
            public void Detach(Collider other)
            {
                Debug.Assert(_nodes.Any(x => x.Collider == other));
                for (var i = 0; i < _nodes.Count; i++)
                {
                    var node = _nodes[i];
                    if (node.Collider == other)
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
                    var collider = node.Collider;
                    Debug.Assert(collider.Velocity == Vector2.Zero && collider.Acceleration == Vector2.Zero);
                    collider.Position = _parent.Position + node.Displacement;
                }
            }
        }
    }
}
