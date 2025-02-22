using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace BreakoutExtreme.Systems
{
    public class GameObjectSystem<T> : EntityProcessingSystem where T : GameObject
    {
        private ComponentMapper<T> _mapper;
        public GameObjectSystem() : base(Aspect.All(typeof(T)))
        {
        }
        public override void Initialize(IComponentMapperService mapperService)
        {
            _mapper = mapperService.GetMapper<T>();
        }
        public override void Process(GameTime gameTime, int entityId)
        {
            var obj = _mapper.Get(entityId);
            if (obj.RemoveLater)
                obj.Remove();
            obj.Update();
        }
    }
}
