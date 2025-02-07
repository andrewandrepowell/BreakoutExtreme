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
            {
                brickCollision = node.Other.Parent is Brick brick && brick.State == Brick.States.Active;
                bombCollision = node.Other.Parent is Bomb bomb && bomb.State == Bomb.States.Active;
                wallCollision = node.Other.Parent is Wall;
            }

            // Always apply correction first
            if (brickCollision || wallCollision)
            {
                node.CorrectPosition();
            }

            // Damage bomb
            if (_state == States.Active && bombCollision)
            {
                ((Bomb)node.Other.Parent).Damage();
            }

            // Destroy upon contact.
            if (_state == States.Active && (brickCollision || wallCollision || bombCollision))
            {
                Destroy();
            }
        }
    }
}
