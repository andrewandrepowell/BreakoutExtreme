using BreakoutExtreme.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace BreakoutExtreme.Systems
{
    public class PlayAreaSystem : EntityUpdateSystem
    {
        private ComponentMapper<Ball> _ballMapper;
        public PlayAreaSystem() : base(Aspect.One(typeof(Ball)))
        {
        }
        public override void Initialize(IComponentMapperService mapperService)
        {
            _ballMapper = mapperService.GetMapper<Ball>();
        }
        public override void Update(GameTime gameTime)
        {
            foreach (var entityId in ActiveEntities)
            {
                var ball = _ballMapper.Get(entityId);
                ball.Update();
            }
        }
    }
}
