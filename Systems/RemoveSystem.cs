using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.ECS;

namespace BreakoutExtreme.Systems
{
    public class RemoveSystem<T> : EntityProcessingSystem where T : class, IDestroyed, IRemoveEntity
    {
        private ComponentMapper<T> _mapper;
        public RemoveSystem() : base(Aspect.All(typeof(T)))
        {
        }
        public override void Initialize(IComponentMapperService mapperService)
        {
            _mapper = mapperService.GetMapper<T>();
        }
        public override void Process(GameTime gameTime, int entityId)
        {
            var obj = _mapper.Get(entityId);
            if (obj.Destroyed)
                obj.RemoveEntity();
        }
    }
}
