using BreakoutExtreme.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace BreakoutExtreme.Systems
{
    public class PlayAreaSystem : EntityProcessingSystem
    {
        private ComponentMapper<PlayArea> _playAreaMapper;
        public PlayAreaSystem() : base(Aspect.One(typeof(PlayArea)))
        {
        }
        public override void Initialize(IComponentMapperService mapperService)
        {
            _playAreaMapper = mapperService.GetMapper<PlayArea>();
        }
        public override void Process(GameTime gameTime, int entityId)
        {
            _playAreaMapper.Get(entityId).Update();
        }
    }
}
