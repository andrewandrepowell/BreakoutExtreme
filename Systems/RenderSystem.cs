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
        private RenderTarget2D _pixelArtRenderTarget;
        private RenderTargetBinding[] _previousRenderTargets = null;
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
            if (_pixelArtRenderTarget == null)
            {
                _pixelArtRenderTarget = new RenderTarget2D(
                    graphicsDevice: Globals.SpriteBatch.GraphicsDevice,
                    width: (int)Globals.GameWindowBounds.Width,
                    height: (int)Globals.GameWindowBounds.Height,
                    mipMap: false,
                    preferredFormat: SurfaceFormat.Color,
                    preferredDepthFormat: DepthFormat.None,
                    preferredMultiSampleCount: 0,
                    usage: RenderTargetUsage.DiscardContents);
            }

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
            var spriteBatch = Globals.SpriteBatch;
            var graphicsDevice = spriteBatch.GraphicsDevice;

            _previousRenderTargets = graphicsDevice.GetRenderTargets();
            graphicsDevice.SetRenderTarget(_pixelArtRenderTarget);
            graphicsDevice.Clear(Color.Pink);
            {
                spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                for (var i = 0; i < _ninePatchers.Count; i++)
                    _ninePatchers[i].Draw();
                spriteBatch.End();

                spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                for (var i = 0; i < _animaters.Count; i++)
                    _animaters[i].Draw();
                spriteBatch.End();
            }
            graphicsDevice.SetRenderTargets(_previousRenderTargets);
            graphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(
                texture: _pixelArtRenderTarget,
                position: Globals.GameWindowToResizeOffset,
                sourceRectangle: null,
                color: Color.White,
                rotation: 0,
                origin: Vector2.Zero,
                scale: Globals.GameWindowToResizeScalar,
                effects: SpriteEffects.None,
                layerDepth: 0);
            spriteBatch.End();
        }
    }
}
