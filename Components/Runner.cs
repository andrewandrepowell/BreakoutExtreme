using BreakoutExtreme.Systems;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.ECS;
using MonoGame.Extended;

namespace BreakoutExtreme.Components
{
    public class Runner
    {
        private World _world;
        private CollisionComponent _collisionComponent;
        public Ball CreateBall()
        {
            var entity = _world.CreateEntity();
            var ball = new Ball();
            var animator = ball.GetAnimater();
            var collider = ball.GetCollider();
            entity.Attach(ball);
            entity.Attach(animator);
            entity.Attach(collider);
            _collisionComponent.Insert(collider);
            return ball;
        }
        public Wall CreateWall(RectangleF bounds)
        {
            var entity = _world.CreateEntity();
            var wall = new Wall(bounds);
            var collider = wall.GetCollider();
            entity.Attach(wall);
            entity.Attach(collider);
            _collisionComponent.Insert(collider);
            return wall;
        }
        public void RemoveEntity(int entityId) => RemoveEntity(_world.GetEntity(entityId));
        public void RemoveEntity(Entity entity)
        {
            if (entity.Has<Collider>())
                _collisionComponent.Remove(entity.Get<Collider>());
            entity.Destroy();
        }
        public Runner()
        {
            _collisionComponent = new CollisionComponent(Globals.PlayAreaBox);

            var worldBuilder = new WorldBuilder();
            var playAreaSystem = new PlayAreaSystem();
            var colliderSystem = new ColliderSystem(_collisionComponent);
            var positionSystem = new PositionSystem();
            var renderSystem = new RenderSystem();
            worldBuilder.AddSystem(playAreaSystem);
            worldBuilder.AddSystem(colliderSystem);
            worldBuilder.AddSystem(positionSystem);
            worldBuilder.AddSystem(renderSystem);
            _world = worldBuilder.Build();

            {
                var thickness = 48;
                CreateWall(new RectangleF(Globals.PlayAreaBox.X, Globals.PlayAreaBox.Y, thickness, Globals.PlayAreaBox.Height));
                CreateWall(new RectangleF(Globals.PlayAreaBox.X + Globals.PlayAreaBox.Width - thickness, Globals.PlayAreaBox.Y, thickness, Globals.PlayAreaBox.Height));
                CreateWall(new RectangleF(Globals.PlayAreaBox.X + thickness, Globals.PlayAreaBox.Y, Globals.PlayAreaBox.Width - 2 * thickness, thickness));
                CreateWall(new RectangleF(Globals.PlayAreaBox.X + thickness, Globals.PlayAreaBox.Y + Globals.PlayAreaBox.Height - thickness, Globals.PlayAreaBox.Width - 2 * thickness, thickness));
            }
        }
        public void Update()
        {
            _world.Update(Globals.GameTime);
        }
        public void Draw()
        {
            _world.Draw(Globals.GameTime);
        }
    }
}
