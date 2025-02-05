using BreakoutExtreme.Components;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using Microsoft.Xna.Framework;

namespace BreakoutExtreme.Systems
{
    public class PulseGlowerSystem : EntityProcessingSystem
    {
        private ComponentMapper<PulseGlower> _mapper;
        public PulseGlowerSystem() : base(Aspect.All(typeof(PulseGlower)))
        {
        }
        public override void Initialize(IComponentMapperService mapperService)
        {
            _mapper = mapperService.GetMapper<PulseGlower>();
        }
        public override void Process(GameTime gameTime, int entityId)
        {
            _mapper.Get(entityId).Update();
        }
    }
}
