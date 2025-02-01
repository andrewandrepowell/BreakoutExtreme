using BreakoutExtreme.Systems;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.ECS;
using MonoGame.Extended;

namespace BreakoutExtreme.Components
{
    public partial class Runner
    {
        readonly private World _world;
        readonly private CollisionComponent _collisionComponent;
        public void RemoveEntity(Entity entity)
        {
            if (entity.Has<Collider>())
                _collisionComponent.Remove(entity.Get<Collider>());
            if (entity.Has<Particler>())
                entity.Get<Particler>().Dispose();
            entity.Destroy();
        }
        public Runner()
        {
            _collisionComponent = new CollisionComponent(Globals.PlayAreaBounds);

            var worldBuilder = new WorldBuilder();
            var gameWindowSystem = new GameWindowSystem();
            var colliderSystem = new ColliderSystem(_collisionComponent);
            var positionSystem = new PositionSystem();
            var renderSystem = new RenderSystem();
            worldBuilder.AddSystem(gameWindowSystem);
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
