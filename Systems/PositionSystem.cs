using BreakoutExtreme.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended;

namespace BreakoutExtreme.Systems
{
    public class PositionSystem : EntityProcessingSystem
    {
        private ComponentMapper<Animater> _animaterMapper;
        private ComponentMapper<Collider> _colliderMapper;
        public PositionSystem() : base(Aspect.All(typeof(Animater), typeof(Collider)))
        {
        }
        public override void Initialize(IComponentMapperService mapperService)
        {
            _animaterMapper = mapperService.GetMapper<Animater>();
            _colliderMapper = mapperService.GetMapper<Collider>();
        }
        public override void Process(GameTime gameTime, int entityId)
        {
            var collider = _colliderMapper.Get(entityId);
            var animater = _animaterMapper.Get(entityId);

            animater.Position = collider.Position;
            if (collider.Bounds is RectangleF)
            {
                animater.Position +=  (Vector2)(collider.Size / 2);
            }
        }
    }
}
