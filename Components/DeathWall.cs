using MonoGame.Extended;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public class DeathWall
    {
        private static readonly Action<Collider.CollideNode> _collideAction = (Collider.CollideNode node) => ((DeathWall)node.Current.Parent).ServiceCollision(node);
        private Collider _collider;
        private bool _running = true;
        private Spike[] _spikes;
        private void ServiceCollision(Collider.CollideNode node)
        {
            if (!_running)
                return;
            if (node.Other.Parent is Ball ball && ball.State == Ball.States.Active)
            {
                ball.Destroy();
            }
        }
        public Collider GetCollider() => _collider;
        public bool Running => _running;
        public DeathWall(RectangleF bounds)
        {
            Debug.Assert(bounds.Height == Globals.GameBlockSize);
            _collider = new(bounds, this, _collideAction);

            {
                var spikesTotal = (int)Math.Floor(bounds.Width / Globals.GameBlockSize);
                _spikes = new Spike[spikesTotal];
                var startPosition = new Vector2(x: bounds .Center.X - (spikesTotal - 1) * Globals.GameBlockSize / 2, bounds.Center.Y);
                for (var i = 0; i < spikesTotal; i++)
                {
                    var spike = Globals.Runner.CreateSpike(new Vector2(x: startPosition.X + i * Globals.GameBlockSize, y: startPosition.Y));
                    spike.Start();
                    _spikes[i] = spike;
                }
            }
        }
    }
}
