using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public partial class Ball
    {
        public class Launcher(Ball parent)
        {
            private readonly Ball _parent = parent;
            public bool Launched { get; private set; } = false;
            public Vector2 Acceleration = new Vector2(0, -5000);
            public void ServiceCollision(Collider.CollideNode node)
            {
                if (node.Other.Parent is Wall || node.Other.Parent is Paddle)
                {
                    var collider = _parent.GetCollider();
                    if (!node.PenetrationVector.X.EqualsWithTolerance(0))
                    {
                        Acceleration.X *= -1;
                        collider.Velocity.X *= -1;
                    }
                    if (!node.PenetrationVector.Y.EqualsWithTolerance(0))
                    {
                        Acceleration.Y *= -1;
                        collider.Velocity.Y *= -1;
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
