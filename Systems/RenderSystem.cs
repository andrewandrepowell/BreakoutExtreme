using MonoGame.Extended.ECS;
using MonoGame.Extended;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;
using BreakoutExtreme.Components;
using Microsoft.Xna.Framework;

namespace BreakoutExtreme.Systems
{
    public class RenderSystem : EntitySystem, IUpdateSystem, IDrawSystem
    {
        private ComponentMapper<Animater> _animaterMapper;
        public RenderSystem() : base(Aspect.One(typeof(Animater)))
        {
        }
        public override void Initialize(IComponentMapperService mapperService)
        {
            _animaterMapper = mapperService.GetMapper<Animater>();
        }
        public void Update(GameTime gameTime)
        {
            foreach (var entityId in ActiveEntities)
            {
                var animater = _animaterMapper.Get(entityId);
                animater.Update();
            }
        }
        public void Draw(GameTime gameTime)
        {
            Globals.SpriteBatch.Begin(samplerState: Microsoft.Xna.Framework.Graphics.SamplerState.PointClamp);
            foreach (var entityId in ActiveEntities)
            {
                var animater = _animaterMapper.Get(entityId);
                animater.Draw();
            }
            Globals.SpriteBatch.End();
        }
    }
}
