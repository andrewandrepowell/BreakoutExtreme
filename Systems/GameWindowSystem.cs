using BreakoutExtreme.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using System.Diagnostics;

namespace BreakoutExtreme.Systems
{
    public class GameWindowSystem : EntityUpdateSystem
    {
        private ComponentMapper<PlayArea> _playAreaMapper;
        public GameWindowSystem() : base(Aspect.One(typeof(PlayArea)))
        {
        }
        public override void Initialize(IComponentMapperService mapperService)
        {
            _playAreaMapper = mapperService.GetMapper<PlayArea>();
        }
        public override void Update(GameTime gameTime)
        {
            PlayArea playArea = null;
            foreach (var entityId in ActiveEntities)
            {
                if (_playAreaMapper.Has(entityId))
                {
                    Debug.Assert(playArea == null);
                    playArea = _playAreaMapper.Get(entityId);
                }
            }

            if (!playArea.Loaded)
            {
                playArea.Load(PlayArea.Levels.Test);
            }
        }
    }
}
