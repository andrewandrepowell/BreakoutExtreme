using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public partial class Paddle
    {
        public class MoveToTarget(Paddle parent)
        {
            private readonly Paddle _parent = parent;
            public bool Running { get; private set; } = false;
            public float Target { get; private set; }
            public float Threshold = 8;
            public float Acceleration = 7000;
            public void Start(float x)
            {
                Debug.Assert(!Running);
                Target = x;
                Running = true;
            }
            public void Stop()
            {
                Debug.Assert(Running);
                Running = false;
            }
            public void ServiceCollision(Collider.CollideNode node)
            {
                if (node.Other.Parent is Wall && Running)
                {
                    Stop();
                }
            }
            public void Update()
            {
                if (Running)
                {
                    var collider = _parent.GetCollider();
                    var position = collider.Position.X + collider.Size.Width / 2;
                    if (Math.Abs(position - Target) <= Threshold)
                    {
                        Stop();
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
