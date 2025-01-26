﻿using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
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
        private ComponentMapper<GumDrawer> _gumDrawerMapper;
        private Bag<Animater> _animaters = new();
        private Bag<NinePatcher> _ninePatchers = new();
        private Bag<GumDrawer> _gumDrawers = new();
        private RenderTarget2D _pixelArtRenderTarget;
        public RenderSystem() : base(Aspect.One(typeof(Animater), typeof(NinePatcher), typeof(GumDrawer)))
        {
        }
        public override void Initialize(IComponentMapperService mapperService)
        {
            _animaterMapper = mapperService.GetMapper<Animater>();
            _ninePatcherMapper = mapperService.GetMapper<NinePatcher>();
            _gumDrawerMapper = mapperService.GetMapper<GumDrawer>();
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
            _gumDrawers.Clear();
            foreach (var entityId in ActiveEntities)
            {
                if (_animaterMapper.Has(entityId))
                    _animaters.Add(_animaterMapper.Get(entityId));
                if (_ninePatcherMapper.Has(entityId))
                    _ninePatchers.Add(_ninePatcherMapper.Get(entityId));
                if (_gumDrawerMapper.Has(entityId))
                    _gumDrawers.Add(_gumDrawerMapper.Get(entityId));
            }

            for (var i = 0; i < _animaters.Count; i++)
                _animaters[i].Update();
        }
        public void Draw(GameTime gameTime)
        {
            var spriteBatch = Globals.SpriteBatch;
            var graphicsDevice = spriteBatch.GraphicsDevice;

            var previousRenderTargets = graphicsDevice.GetRenderTargets();
            graphicsDevice.SetRenderTarget(_pixelArtRenderTarget);
            graphicsDevice.Clear(Color.Pink);
            {
                {
                    spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                    for (var i = 0; i < _ninePatchers.Count; i++)
                        _ninePatchers[i].Draw();
                    spriteBatch.End();
                }
                {
                    spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                    for (var i = 0; i < _animaters.Count; i++)
                        _animaters[i].Draw();
                    spriteBatch.End();
                }
                {
                    spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                    for (var i = 0; i < _gumDrawers.Count; i++)
                        _gumDrawers[i].MonoDraw();
                    spriteBatch.End();
                }
            }
            graphicsDevice.SetRenderTargets(previousRenderTargets);
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
