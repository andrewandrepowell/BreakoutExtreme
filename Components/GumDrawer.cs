using Gum;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MonoGame.Extended;
using System.Diagnostics;
using System.Linq;
using RenderingLibrary;
using MonoGame.Extended.Collections;
using BreakoutExtreme.Utility;

namespace BreakoutExtreme.Components
{
    public class GumDrawer
    {
        private readonly InteractiveGue _gumRuntime;
        private readonly Bag<Shaders.Feature> _shaderFeatures = new();
        private RenderTarget2D _renderTarget;
        private Vector2 _position, _drawPosition, _shaderDrawOffset;
        private Size _size;
        private Vector2 _origin;
        private Color _drawColor;
        private Color _color = Color.White;
        private float _visibility = 1, _shaderVisibility = 1;
        private void UpdateShaderFeatures()
        {
            {
                _shaderDrawOffset = Vector2.Zero;
                _shaderVisibility = 1;
                var updateDrawPosition = false;
                var updateVisibility = false;
                for (var i = 0; i < ShaderFeatures.Count; i++)
                {
                    var feature = ShaderFeatures[i];
                    updateDrawPosition |= feature.UpdateDrawOffset(ref _shaderDrawOffset);
                    updateVisibility |= feature.UpdateVisibility(ref _shaderVisibility);
                }
                if (updateDrawPosition)
                    UpdateDrawPosition();
                if (updateVisibility)
                    UpdateDrawColor();
            }

            for (var i = 0; i < ShaderFeatures.Count; i++)
                ShaderFeatures[i].Update();
        }
        private void UpdateDrawColor()
        {
            _drawColor = Color * Visibility * _shaderVisibility;
        }
        private void UpdateRenderTarget()
        {
            _renderTarget = new RenderTarget2D(
                graphicsDevice: Globals.SpriteBatch.GraphicsDevice,
                width: (int)_gumRuntime.Width,
                height: (int)_gumRuntime.Height,
                mipMap: false,
                preferredFormat: SurfaceFormat.Color,
                preferredDepthFormat: DepthFormat.None,
                preferredMultiSampleCount: 0,
                usage: RenderTargetUsage.DiscardContents);
        }
        private void UpdateDrawPosition()
        {
            _drawPosition.X = (float)Math.Ceiling(_position.X + _shaderDrawOffset.X);
            _drawPosition.Y = (float)Math.Ceiling(_position.Y + _shaderDrawOffset.Y);
        }
        private void UpdateSizeOrigin()
        {
            _size.Width = _renderTarget.Width;
            _size.Height = _renderTarget.Height;
            _origin.X = _size.Width / 2;
            _origin.Y = _size.Height / 2;
        }
        public Vector2 Position
        {
            get => _position;
            set
            {
                if (_position == value)
                    return;
                _position = value;
                UpdateDrawPosition();
            }
        }
        public Size Size => _size;
        public Color Color
        {
            get => _color;
            set
            {
                if (_color == value) 
                    return;
                _color = value;
                UpdateDrawColor();
            }
        }
        public float Visibility
        {
            get => _visibility;
            set
            {
                Debug.Assert(value >= 0);
                if (_visibility == value)
                    return;
                _visibility = value;
                UpdateDrawColor();
            }
        }
        public Bag<Shaders.Feature> ShaderFeatures => _shaderFeatures;
        public Layers Layer = Layers.Ground;
        public void UpdateSizeImmediately()
        {
            UpdateRenderTarget();
            UpdateSizeOrigin();
        }
        public GumDrawer(InteractiveGue gumRuntime)
        {
            _gumRuntime = gumRuntime;
            UpdateRenderTarget();
            UpdateSizeOrigin();
            UpdateDrawColor();
        }
        public void Update()
        {
            UpdateShaderFeatures();

            if (Visibility == 0)
                return;
            
            if ((int)_gumRuntime.Width != _renderTarget.Width || 
                (int)_gumRuntime.Height != _renderTarget.Height)
            {
                UpdateRenderTarget();
                UpdateSizeOrigin();
            }
        }
        public void GumDraw()
        {
            var graphicsDevice = Globals.SpriteBatch.GraphicsDevice;
            var gumBatch = GumUI.GetGumBatch();
            var camera = SystemManagers.Default.Renderer.Camera;
            var previousRenderTargets = graphicsDevice.GetRenderTargets();
            graphicsDevice.SetRenderTargets(_renderTarget);
            graphicsDevice.Clear(Color.Transparent);
            camera.ClientHeight = Size.Height;
            camera.ClientWidth = Size.Width;
            gumBatch.Begin();
            gumBatch.Draw(_gumRuntime);
            gumBatch.End();
            graphicsDevice.SetRenderTargets(previousRenderTargets);
        }
        public void MonoDraw()
        {
            Globals.SpriteBatch.Draw(
                texture: _renderTarget,
                position: _drawPosition,
                sourceRectangle: null,
                color: _drawColor,
                rotation: 0,
                origin: _origin,
                scale: 1,
                effects: SpriteEffects.None,
                layerDepth: 0);
        }
    }
}
