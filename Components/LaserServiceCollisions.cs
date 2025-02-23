using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public partial class Laser
    {
        private void ServiceCollision(Collider.CollideNode node)
        {
            if (!_initialized || _state != States.Active)
                return;

            bool brickCollision;
            bool bombCollision;
            bool wallCollision;
            bool cannonCollision;
            {
                brickCollision = node.Other.Parent is Brick brick && brick.State == Brick.States.Active;
                bombCollision = node.Other.Parent is Bomb bomb && bomb.State == Bomb.States.Active;
                wallCollision = node.Other.Parent is Wall;
                cannonCollision = node.Other.Parent is Cannon cannon && cannon.State == Cannon.States.Active;
            }

            // Always apply correction first
            if (brickCollision || wallCollision || cannonCollision)
            {
                node.CorrectPosition();
            }

            // Damage bomb
            if (_state == States.Active && bombCollision)
            {
                var bomb = ((Bomb)node.Other.Parent);
                bomb.Damage();

                if (bomb.State == Bomb.States.Destroying)
                    _parent.UpdateScore(bomb);
            }

            // Reuse the service damage operations from the ball if the laser is empowered.
            if (_state == States.Active && _empowered)
            {
                Ball.ServiceApplyDamage(node: node, playArea: _parent, powerBricks: _powerBricks);
            }

            // Destroy upon contact.
            if (_state == States.Active && (brickCollision || wallCollision || bombCollision || cannonCollision))
            {
                Destroy();
            }
        }
    }
}
