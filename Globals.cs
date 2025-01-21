using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace BreakoutExtreme
{
    public static class Globals
    {
#if DEBUG
        private static bool _initialized = false;
#endif
        public static SpriteBatch SpriteBatch;
        public static ContentManager ContentManager;
        public static GameTime GameTime;
        public static void Initialize(SpriteBatch spriteBatch, ContentManager contentManager)
        {
#if DEBUG
            Debug.Assert(!_initialized);
            _initialized = true;
#endif
            SpriteBatch = spriteBatch;
            ContentManager = contentManager;
        }
        public static void UpdateGameTime(GameTime gameTime)
        {
            GameTime = gameTime;
        }
    }
}
