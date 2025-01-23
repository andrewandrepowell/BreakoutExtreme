using BreakoutExtreme.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Diagnostics;

namespace BreakoutExtreme
{
    public static class Globals
    {
#if DEBUG
        private static bool _initialized = false;
#endif
        public static readonly RectangleF PlayAreaBox = new RectangleF(0, 0, 256, 512);
        public static SpriteBatch SpriteBatch { get; private set; }
        public static ContentManager ContentManager { get; private set; }
        public static GameTime GameTime { get; private set; }
        public static Controller.ControlState ControlState { get; private set; }
        public static void Initialize(
            SpriteBatch spriteBatch, 
            ContentManager contentManager,
            Controller.ControlState controlState)
        {
#if DEBUG
            Debug.Assert(!_initialized);
            _initialized = true;
#endif
            SpriteBatch = spriteBatch;
            ContentManager = contentManager;
            ControlState = controlState;
        }
        public static void UpdateGameTime(GameTime gameTime)
        {
            GameTime = gameTime;
        }
    }
}
