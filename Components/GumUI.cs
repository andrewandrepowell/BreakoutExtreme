using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using RenderingLibrary;
using RenderingLibrary.Graphics;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public static class GumUI
    {
        private static bool _initialized = false;
        private static GumBatch _gumBatch;
        private static BitmapFont _montserratSmall;
        private static BitmapFont _montserratLarge;
        public static BitmapFont MontserratSmall
        {
            get
            {
                Debug.Assert(_initialized);
                return _montserratSmall;
            }
        }
        public static BitmapFont MontserratLarge
        {
            get
            {
                Debug.Assert(_initialized);
                return _montserratLarge;
            }
        }
        public static GumBatch GetGumBatch()
        {
            Debug.Assert(_initialized);
            return _gumBatch;
        }
        public static void Initialize()
        {
            Debug.Assert(!_initialized);
            _initialized = true;
            GumService.Default.Initialize(Globals.SpriteBatch.GraphicsDevice);
            _gumBatch = new GumBatch();

            // Load gum files.
            {
#pragma warning disable CA1806
                _montserratSmall = new BitmapFont("fonts/montserrat/montserrat_0.fnt", SystemManagers.Default);
                _montserratLarge = new BitmapFont("fonts/montserrat/montserrat_1.fnt", SystemManagers.Default);
                Globals.ContentManager.Load<Texture2D>("animations/panel_0");
                Globals.ContentManager.Load<Texture2D>("animations/button_0");
                Globals.ContentManager.Load<Texture2D>("animations/button_1");
                Globals.ContentManager.Load<Texture2D>("animations/break_out_0");
                Globals.ContentManager.Load<Texture2D>("animations/menu_0");
#pragma warning restore CA1806
            }
        }
    }
}
