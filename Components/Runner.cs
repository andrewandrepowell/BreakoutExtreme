using BreakoutExtreme.Systems;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.ECS;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public partial class Runner
    {
        private const int _poolSize = 64;
        readonly private World _world;
        readonly private CollisionComponent _collisionComponent;
        readonly private Deque<ScorePopup> _scorePopupPool = new();
        private bool _initialized = false;
        public void RemoveEntity(Entity entity)
        {
            Debug.Assert(_initialized);
            if (entity.Has<ScorePopup>())
                _scorePopupPool.AddToBack(entity.Get<ScorePopup>());
            if (entity.Has<Collider>())
                _collisionComponent.Remove(entity.Get<Collider>());
            if (entity.Has<Particler>())
                entity.Get<Particler>().Dispose();
            entity.Destroy();
        }
        public void Initialize()
        {
            Debug.Assert(!_initialized);

            GumUI.Initialize();

            // Initialize all the pools.
            {
                for (var i = 0; i < _poolSize; i++)
                {
                    var scorePopup = new ScorePopup(null);
                    _scorePopupPool.AddToBack(scorePopup);
                }
            }

            _initialized = true;
        }
        public Runner()
        {
            _collisionComponent = new CollisionComponent(Globals.PlayAreaBounds);

            {
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
        }
        public void Update()
        {
            Debug.Assert(_initialized);
            _world.Update(Globals.GameTime);
        }
        public void Draw()
        {
            Debug.Assert(_initialized);
            _world.Draw(Globals.GameTime);
        }
    }
}
