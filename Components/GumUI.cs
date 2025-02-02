using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using RenderingLibrary;
using RenderingLibrary.Graphics;
using System.Diagnostics;
using ToolsUtilities;

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
            _gumBatch = new GumBatch();

            // Load gum files.
            {
#pragma warning disable CA1806
                new BitmapFont("fonts/montserrat/montserrat_0.fnt", SystemManagers.Default);
                new BitmapFont("fonts/montserrat/montserrat_1.fnt", SystemManagers.Default);
#pragma warning restore CA1806
            }
        }
    }
}
