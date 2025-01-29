using MonoGame.Extended;
using System;

namespace BreakoutExtreme.Components
{
    public class DeathWall
    {
        private static readonly Action<Collider.CollideNode> _collideAction = (Collider.CollideNode node) => ((DeathWall)node.Current.Parent).ServiceCollision(node);
        private Collider _collider;
        private bool _running = true;
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
            _collider = new(bounds, this, _collideAction);
        }
    }
}
