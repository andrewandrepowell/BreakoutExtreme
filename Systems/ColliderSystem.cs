using BreakoutExtreme.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace BreakoutExtreme.Systems
{
    public class ColliderSystem : EntityUpdateSystem
    {
        private readonly CollisionComponent _collisionComponent;
        private ComponentMapper<Collider> _colliderMapper;
        public ColliderSystem(CollisionComponent collisionComponent) : base(Aspect.One(typeof(Collider)))
        {
            _collisionComponent = collisionComponent;
        }
        public override void Initialize(IComponentMapperService mapperService)
        {
            _colliderMapper = mapperService.GetMapper<Collider>();
        }
        public override void Update(GameTime gameTime)
        {
            foreach (var entityId in ActiveEntities)
                _colliderMapper.Get(entityId).Update();
            _collisionComponent.Update(gameTime);
        }
    }
}
