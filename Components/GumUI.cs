using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using RenderingLibrary;
using RenderingLibrary.Graphics;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public static class GumUI
    {
#if DEBUG
        private static bool _initialized = false;
#endif
        private static GumBatch _gumBatch;
        public static GumBatch GetGumBatch()
        {
#if DEBUG
            Debug.Assert(_initialized);
#endif
            return _gumBatch;
        }
        public static void Initialize()
        {
#if DEBUG
            Debug.Assert(!_initialized);
            _initialized = true;
#endif
            GumService.Default.Initialize(Globals.SpriteBatch.GraphicsDevice);
            {
                //var renderer = SystemManagers.Default.Renderer;
                //var camera = renderer.Camera;
                //var windowSize = Globals.SpriteBatch.GraphicsDevice.Viewport.Bounds.Size;
                //camera.ClientHeight = 64;
                //camera.ClientWidth = 64;
                //camera.Zoom = 4;
            }
            _gumBatch = new GumBatch();
        }
    }
}
