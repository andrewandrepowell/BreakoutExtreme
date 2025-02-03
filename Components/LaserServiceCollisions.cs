namespace BreakoutExtreme.Components
{
    public partial class Laser
    {
        private void ServiceCollision(Collider.CollideNode node)
        {
            if (!_initialized || _state != States.Active)
                return;

            var brickCollision = node.Other.Parent is Brick brick && brick.State == Brick.States.Active;
            var wallCollision = node.Other.Parent is Wall;

            // Always apply correction first
            if (brickCollision || wallCollision)
            {
                node.CorrectPosition();
            }

            // Destroy upon contact.
            if (_state == States.Active && (brickCollision || wallCollision))
            {
                Destroy();
            }
        }
    }
}
