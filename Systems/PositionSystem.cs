using BreakoutExtreme.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

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
            _animaterMapper.Get(entityId).Position = _colliderMapper.Get(entityId).Position;
        }
    }
}
