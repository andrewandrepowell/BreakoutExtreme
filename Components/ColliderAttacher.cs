using MonoGame.Extended.Collections;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace BreakoutExtreme.Components
{
    public partial class Collider
    {
        public class Attacher
        {
            private readonly Bag<Collider> colliders = [];
            public void Attach(Collider other)
            {
                Debug.Assert(!colliders.Contains(other));
                colliders.Add(other);
            }
            public void Detach(Collider other)
            {
                Debug.Assert(colliders.Contains(other));
                colliders.Remove(other);
            }
            public void UpdatePositions(Vector2 previousPosition, Vector2 newPosition)
            {
                var displacement = newPosition - previousPosition;
                for (var i = 0; i < colliders.Count; ++i)
                {
                    var collider = colliders[i];
                    Debug.Assert(collider.Velocity == Vector2.Zero && collider.Acceleration == Vector2.Zero);
                    collider.Position += displacement;
                }
            }
        }
    }
}
