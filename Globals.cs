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
        public static readonly RectangleF GameWindowBounds = new RectangleF(0, 0, 352, 640);
        public static readonly RectangleF PlayAreaBounds = new RectangleF(0, 0, 352, 640);
        public static float GameWindowToResizeScalar = 1;
        public static Vector2 GameWindowToResizeOffset = Vector2.Zero;
        public static SpriteBatch SpriteBatch { get; private set; }
        public static ContentManager ContentManager { get; private set; }
        public static GameTime GameTime { get; private set; }
        public static Controller.ControlState ControlState { get; private set; }
        public static Runner Runner { get; private set; }
        public static void Initialize(
            SpriteBatch spriteBatch, 
            ContentManager contentManager,
            Controller.ControlState controlState,
            Runner runner)
        {
#if DEBUG
            Debug.Assert(!_initialized);
            _initialized = true;
#endif
            SpriteBatch = spriteBatch;
            ContentManager = contentManager;
            ControlState = controlState;
            Runner = runner;
        }
        public static void UpdateGameTime(GameTime gameTime)
        {
            GameTime = gameTime;
        }
        public static void UpdateResizeParameters(float scalar, Vector2 offset)
        {
            GameWindowToResizeScalar = scalar;
            GameWindowToResizeOffset = offset;
        }
    }
}
