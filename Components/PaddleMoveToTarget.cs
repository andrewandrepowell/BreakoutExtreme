using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public partial class Paddle
    {
        public class MoveToTarget
        {
            private Paddle _parent;
            public MoveToTarget(Paddle parent)
            {
                _parent = parent;
            }
            public bool Moving { get; private set; } = false;
            public float Target { get; private set; }
            public float Threshold = 16;
            public float Acceleration = 4000;
            public void MoveTo(float x)
            {
                Debug.Assert(!Moving);
                Target = x;
                Moving = true;
            }
            public void Release()
            {
                Debug.Assert(Moving);
                Moving = false;
            }
            public void ServiceCollision(Collider.Node node)
            {
                if (node.Other.Parent is Wall && Moving)
                {
                    Release();
                }
            }
            public void Update()
            {
                if (Moving)
                {
                    var collider = _parent.GetCollider();
                    var position = collider.Position.X + collider.Size.Width / 2;
                    if (Math.Abs(position - Target) <= Threshold)
                    {
                        Release();
                    }
                    else if (position < Target)
                    {
                        collider.Acceleration.X += Acceleration;
                    }
                    else 
                    {
                        collider.Acceleration.X -= Acceleration;
                    }
                }
            }
        }
    }
}
