using Gum;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MonoGame.Extended;
using System.Diagnostics;
using System.Linq;
using RenderingLibrary;

namespace BreakoutExtreme.Components
{
    public class GumDrawer
    {
        private readonly InteractiveGue _gumRuntime;
        private RenderTarget2D _renderTarget;
        private Vector2 _position, _drawPosition;
        private Size _size;
        private Vector2 _origin;
        private Color _drawColor;
        private Color _color = Color.White;
        private float _visibility = 1;
        private void UpdateDrawColor()
        {
            _drawColor = Color * Visibility;
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
            _drawPosition.X = (float)Math.Ceiling(_position.X);
            _drawPosition.Y = (float)Math.Ceiling(_position.Y);
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
        public GumDrawer(InteractiveGue gumRuntime)
        {
            _gumRuntime = gumRuntime;
            UpdateRenderTarget();
            UpdateSizeOrigin();
            UpdateDrawColor();
        }
        public void Update()
        {
            if (Visibility == 0)
                return;
            if ((int)_gumRuntime.GetAbsoluteWidth() != _renderTarget.Width || 
                (int)_gumRuntime.GetAbsoluteHeight() != _renderTarget.Height)
            {
                UpdateRenderTarget();
                UpdateSizeOrigin();
            }
        }
        public void GumDraw()
        {
            if (Visibility == 0)
                return;
            var graphicsDevice = Globals.SpriteBatch.GraphicsDevice;
            var gumBatch = GumUI.GetGumBatch();
            var previousRenderTargets = graphicsDevice.GetRenderTargets();
            graphicsDevice.SetRenderTargets(_renderTarget);
            {
                {
                    var renderer = SystemManagers.Default.Renderer;
                    var camera = renderer.Camera;
                    var windowSize = Globals.SpriteBatch.GraphicsDevice.Viewport.Bounds.Size;
                    //camera.X = 0;
                    //camera.Y = 0;
                    //camera.ClientHeight = (int)(Globals.GameWindowBounds.Width * Globals.GameWindowToResizeScalar);
                    //camera.ClientWidth = (int)(Globals.GameWindowBounds.Height * Globals.GameWindowToResizeScalar);
                    camera.CameraCenterOnScreen = CameraCenterOnScreen.TopLeft;
                    camera.ClientHeight = Size.Height;
                    camera.ClientWidth = Size.Width;
                }
            }
            Console.WriteLine($"Size: {Size}");
            gumBatch.Begin();
            gumBatch.Draw(_gumRuntime);
            gumBatch.End();
            graphicsDevice.SetRenderTargets(previousRenderTargets);
        }
        public void MonoDraw()
        {
            if (Visibility == 0)
                return;
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
