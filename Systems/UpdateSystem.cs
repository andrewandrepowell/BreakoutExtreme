using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.ECS;
using BreakoutExtreme.Utility;

namespace BreakoutExtreme.Systems
{
    public class UpdateSystem<T> : EntityProcessingSystem where T : class, IUpdate
    {
        private ComponentMapper<T> _mapper;
        public UpdateSystem() : base(Aspect.All(typeof(T)))
        {
        }
        public override void Initialize(IComponentMapperService mapperService)
        {
            _mapper = mapperService.GetMapper<T>();
        }
        public override void Process(GameTime gameTime, int entityId)
        {
            _mapper.Get(entityId).Update();
        }
    }
}
