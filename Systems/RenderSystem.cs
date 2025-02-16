using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using BreakoutExtreme.Components;
using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Collections;
using System;
using System.Diagnostics;

namespace BreakoutExtreme.Systems
{
    public class RenderSystem : EntitySystem, IUpdateSystem, IDrawSystem
    {
        private static readonly Layers[] _layers = Enum.GetValues<Layers>();
        private ComponentMapper<Animater> _animaterMapper;
        private ComponentMapper<NinePatcher> _ninePatcherMapper;
        private ComponentMapper<GumDrawer> _gumDrawerMapper;
        private ComponentMapper<Particler> _particlerMapper;
        private ComponentMapper<Texturer> _texturerMapper;
        private Bag<Animater> _animaters = new();
        private Bag<NinePatcher> _ninePatchers = new();
        private Bag<GumDrawer> _gumDrawers = new();
        private Bag<Particler> _particlers = new();
        private Bag<Texturer> _texturers = new();
        private RenderTarget2D _pixelArtRenderTarget;
        private RenderTarget2D _smoothArtRenderTarget;
        private Shaders.Controller _shaderController = null;
        private bool _initialized = false;
        public RenderSystem() : base(Aspect.One(typeof(Animater), typeof(NinePatcher), typeof(GumDrawer), typeof(Particler), typeof(Texturer)))
        {
        }
        public override void Initialize(IComponentMapperService mapperService)
        {
            _animaterMapper = mapperService.GetMapper<Animater>();
            _ninePatcherMapper = mapperService.GetMapper<NinePatcher>();
            _gumDrawerMapper = mapperService.GetMapper<GumDrawer>();
            _particlerMapper = mapperService.GetMapper<Particler>();
            _texturerMapper = mapperService.GetMapper<Texturer>();
        }
        public void Update(GameTime gameTime)
        {
            if (!_initialized)
            {
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
                    _smoothArtRenderTarget = new RenderTarget2D(
                        graphicsDevice: Globals.SpriteBatch.GraphicsDevice,
                        width: (int)Globals.GameWindowBounds.Width,
                        height: (int)Globals.GameWindowBounds.Height,
                        mipMap: false,
                        preferredFormat: SurfaceFormat.Color,
                        preferredDepthFormat: DepthFormat.None,
                        preferredMultiSampleCount: 0,
                        usage: RenderTargetUsage.DiscardContents);
                }
                {
                    _shaderController = new();
                }
                _initialized = true;
            }

            {
                _animaters.Clear();
                _ninePatchers.Clear();
                _gumDrawers.Clear();
                _particlers.Clear();
                _texturers.Clear();
                for (var i = 0; i < ActiveEntities.Count; i++)
                {
                    var entityId = ActiveEntities[i];
                    if (_animaterMapper.Has(entityId))
                        _animaters.Add(_animaterMapper.Get(entityId));
                    if (_ninePatcherMapper.Has(entityId))
                        _ninePatchers.Add(_ninePatcherMapper.Get(entityId));
                    if (_gumDrawerMapper.Has(entityId))
                        _gumDrawers.Add(_gumDrawerMapper.Get(entityId));
                    if (_particlerMapper.Has(entityId))
                        _particlers.Add(_particlerMapper.Get(entityId));
                    if (_texturerMapper.Has(entityId))
                        _texturers.Add(_texturerMapper.Get(entityId));
                }
            }

            {
                for (var i = 0; i < _texturers.Count; i++)
                    _texturers[i].Update();

                for (var i = 0; i < _gumDrawers.Count; i++)
                    _gumDrawers[i].Update();

                for (var i = 0; i < _animaters.Count; i++)
                    _animaters[i].Update();

                for (var i = 0; i < _particlers.Count; i++)
                    _particlers[i].Update();
            }
        }
        public void Draw(GameTime gameTime)
        {
            var spriteBatch = Globals.SpriteBatch;
            var graphicsDevice = spriteBatch.GraphicsDevice;

            {
                for (var i = 0; i < _gumDrawers.Count; i++)
                {
                    var gumDraw = _gumDrawers[i];
                    if (gumDraw.Visibility != 0)
                        gumDraw.GumDraw();
                }
            }

            var previousRenderTargets = graphicsDevice.GetRenderTargets();
            graphicsDevice.SetRenderTarget(_pixelArtRenderTarget);
            graphicsDevice.Clear(Color.Transparent);
            {
                {
                    spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                    for (var i = 0; i < _ninePatchers.Count; i++)
                        _ninePatchers[i].Draw();
                    spriteBatch.End();
                }

                foreach (ref var layer in _layers.AsSpan())
                {
                    {
                        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                        for (var i = 0; i < _particlers.Count; i++)
                        {
                            var particler = _particlers[i];
                            if (particler.Layer == layer)
                                particler.Draw();
                        }
                        spriteBatch.End();
                    }

                    {
                        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                        for (var i = 0; i < _texturers.Count; i++)
                        {
                            var texturer = _texturers[i];
                            if (texturer.Layer == layer && texturer.Visibility != 0 && texturer.ShowBase)
                                texturer.Draw();
                        }
                        spriteBatch.End();

                        for (var i = 0; i < _texturers.Count; i++)
                        {
                            var texturer = _texturers[i];
                            var shaderFeatures = texturer.ShaderFeatures;
                            Debug.Assert(shaderFeatures.Count <= 32);
                            if (texturer.Layer == layer && texturer.Visibility != 0)
                            {
                                for (var j = 0; j < shaderFeatures.Count; j++)
                                {
                                    var shaderFeature = shaderFeatures[j];
                                    if (!shaderFeature.Script.HasValue)
                                        continue;
                                    _shaderController.Begin(shaderFeature);
                                    texturer.Draw();
                                    spriteBatch.End();
                                }
                            }
                        }
                    }

                    {
                        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                        for (var i = 0; i < _animaters.Count; i++)
                        {
                            var animater = _animaters[i];
                            if (animater.Layer == layer && animater.Visibility != 0 && animater.ShowBase)
                                animater.Draw();
                        }
                        spriteBatch.End();

                        for (var i = 0; i < _animaters.Count; i++)
                        {
                            var animater = _animaters[i];
                            var shaderFeatures = animater.ShaderFeatures;
                            Debug.Assert(shaderFeatures.Count <= 32);
                            if (animater.Layer == layer && animater.Visibility != 0)
                            {
                                for (var j = 0; j < shaderFeatures.Count; j++)
                                {
                                    var shaderFeature = shaderFeatures[j];
                                    if (!shaderFeature.Script.HasValue)
                                        continue;
                                    _shaderController.Begin(shaderFeature);
                                    animater.Draw();
                                    spriteBatch.End();
                                }
                            }
                        }
                    }
                }
            }

            graphicsDevice.SetRenderTargets(_smoothArtRenderTarget);
            graphicsDevice.Clear(Color.Transparent);
            {
                foreach (ref var layer in _layers.AsSpan())
                {
                    {
                        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                        for (var i = 0; i < _gumDrawers.Count; i++)
                        {
                            var gumDraw = _gumDrawers[i];
                            if (gumDraw.Layer == layer && gumDraw.Visibility != 0)
                                gumDraw.MonoDraw();
                        }
                        spriteBatch.End();
                    }

                    {
                        for (var i = 0; i < _gumDrawers.Count; i++)
                        {
                            var gumDrawer = _gumDrawers[i];
                            var shaderFeatures = gumDrawer.ShaderFeatures;
                            Debug.Assert(shaderFeatures.Count <= 32);
                            if (gumDrawer.Layer == layer && gumDrawer.Visibility != 0)
                            {
                                for (var j = 0; j < shaderFeatures.Count; j++)
                                {
                                    var shaderFeature = shaderFeatures[j];
                                    if (!shaderFeature.Script.HasValue)
                                        continue;
                                    _shaderController.Begin(shaderFeature);
                                    gumDrawer.MonoDraw();
                                    spriteBatch.End();
                                }
                            }
                        }
                    }
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

            spriteBatch.Begin(samplerState: SamplerState.LinearClamp);
            spriteBatch.Draw(
                texture: _smoothArtRenderTarget,
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
