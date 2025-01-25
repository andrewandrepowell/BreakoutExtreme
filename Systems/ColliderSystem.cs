using BreakoutExtreme.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using System;


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
            {
                var collider = _colliderMapper.Get(entityId);
                collider.Update();
            }
            _collisionComponent.Update(gameTime);
        }
    }
}
