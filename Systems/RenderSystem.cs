using MonoGame.Extended.ECS;
using MonoGame.Extended;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;
using BreakoutExtreme.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Collections;

namespace BreakoutExtreme.Systems
{
    public class RenderSystem : EntitySystem, IUpdateSystem, IDrawSystem
    {
        private ComponentMapper<Animater> _animaterMapper;
        private ComponentMapper<NinePatcher> _ninePatcherMapper;
        private Bag<Animater> _animaters = new();
        private Bag<NinePatcher> _ninePatchers = new();
        public RenderSystem() : base(Aspect.One(typeof(Animater), typeof(NinePatcher)))
        {
        }
        public override void Initialize(IComponentMapperService mapperService)
        {
            _animaterMapper = mapperService.GetMapper<Animater>();
            _ninePatcherMapper = mapperService.GetMapper<NinePatcher>();
        }
        public void Update(GameTime gameTime)
        {
            _animaters.Clear();
            _ninePatchers.Clear();
            foreach (var entityId in ActiveEntities)
            {
                if (_animaterMapper.Has(entityId))
                    _animaters.Add(_animaterMapper.Get(entityId));
                if (_ninePatcherMapper.Has(entityId))
                    _ninePatchers.Add(_ninePatcherMapper.Get(entityId));
            }

            for (var i = 0; i < _animaters.Count; i++)
                _animaters[i].Update();
        }
        public void Draw(GameTime gameTime)
        {
            Globals.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            for (var i = 0; i < _ninePatchers.Count; i++)
                _ninePatchers[i].Draw();
            Globals.SpriteBatch.End();

            Globals.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            for (var i = 0; i < _animaters.Count; i++)
                _animaters[i].Draw();
            Globals.SpriteBatch.End();
        }
    }
}
