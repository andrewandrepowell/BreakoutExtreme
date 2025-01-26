using BreakoutExtreme.Components;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using Microsoft.Xna.Framework;
using System;

namespace BreakoutExtreme.Systems
{
    public class GumRenderSystem : EntitySystem, IUpdateSystem, IDrawSystem
    {
        private ComponentMapper<GumDrawer> _gumDrawerMapper;

        public GumRenderSystem() : base(Aspect.One(typeof(GumDrawer)))
        {
        }
        public override void Initialize(IComponentMapperService mapperService)
        {
            _gumDrawerMapper = mapperService.GetMapper<GumDrawer>();
        }
        public void Update(GameTime gameTime)
        {
            foreach (var entityId in ActiveEntities)
                _gumDrawerMapper.Get(entityId).Update();
        }
        public void Draw(GameTime gameTime)
        {
            foreach (var entityId in ActiveEntities)
                _gumDrawerMapper.Get(entityId).GumDraw();
        }
    }
}
