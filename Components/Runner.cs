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
        public PlayArea CreatePlayArea()
        {
            var entity = _world.CreateEntity();
            var playArea = new PlayArea();
            entity.Attach(playArea);
            return playArea;
        }
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
        public NinePatcher CreateNinePatcher()
        {
            var entity = _world.CreateEntity();
            var ninePatcher = new NinePatcher();
            entity.Attach(ninePatcher);
            return ninePatcher;
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
            _collisionComponent = new CollisionComponent(Globals.PlayAreaBounds);

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
