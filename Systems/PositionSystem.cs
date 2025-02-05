using BreakoutExtreme.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended;
using System;

namespace BreakoutExtreme.Systems
{
    public class PositionSystem : EntityProcessingSystem
    {
        private ComponentMapper<Animater> _animaterMapper;
        private ComponentMapper<Collider> _colliderMapper;
        private ComponentMapper<Particler> _particlerMapper;
        public PositionSystem() : base(Aspect.All(typeof(Collider)).One(typeof(Animater), typeof(Particler)))
        {
        }
        public override void Initialize(IComponentMapperService mapperService)
        {
            _animaterMapper = mapperService.GetMapper<Animater>();
            _colliderMapper = mapperService.GetMapper<Collider>();
            _particlerMapper = mapperService.GetMapper<Particler>();
        }
        public override void Process(GameTime gameTime, int entityId)
        {
            var collider = _colliderMapper.Get(entityId);

            var position = collider.Position;
            if (collider.Bounds is RectangleF)
                position += (Vector2)(collider.Size / 2);

            if (_animaterMapper.Has(entityId))
            {
                var animater = _animaterMapper.Get(entityId);
                animater.Position = position;
            }

            if (_particlerMapper.Has(entityId))
            {
                var particler = _particlerMapper.Get(entityId);
                if (!particler.Disposed)
                    particler.Position = position;
            }
        }
    }
}
