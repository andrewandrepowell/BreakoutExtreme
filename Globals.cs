using BreakoutExtreme.Components;
using BreakoutExtreme.Utility;
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
        public const int GameBlockSize = 16;
        public const int GameHalfBlockSize = GameBlockSize / 2;
        public static readonly Rectangle GameWindowBlockBounds = new(0, 0, 22, 40);
        public static readonly Rectangle PlayAreaBlockBounds = new(0, 5, 22, 35);
        public static readonly Rectangle ScoreLabelBlockBounds = new(1, 1, 5, 2);
        public static readonly Rectangle ScorePanelBlockBounds = new(6, 1, 5, 2);
        public static readonly Rectangle HighScoreLabelBlockBounds = new(1, 3, 5, 2);
        public static readonly Rectangle HighScorePanelBlockBounds = new(6, 3, 5, 2);
        public static readonly RectangleF GameWindowBounds = GameWindowBlockBounds.ToBounds();
        public static readonly RectangleF PlayAreaBounds = PlayAreaBlockBounds.ToBounds();
        public static float GameWindowToResizeScalar = 1;
        public static Vector2 GameWindowToResizeOffset = Vector2.Zero;
        public static SpriteBatch SpriteBatch { get; private set; }
        public static ContentManager ContentManager { get; private set; }
        public static GameTime GameTime { get; private set; }
        public static Controller.ControlState ControlState { get; private set; }
        public static Game Game { get; private set; }
        public static Runner Runner { get; private set; }
        public static void Initialize(
            SpriteBatch spriteBatch, 
            ContentManager contentManager,
            Controller.ControlState controlState,
            Game game,
            Runner runner)
        {
#if DEBUG
            Debug.Assert(!_initialized);
            _initialized = true;
#endif
            SpriteBatch = spriteBatch;
            ContentManager = contentManager;
            ControlState = controlState;
            Game = game;
            Runner = runner;
        }
        public static void Update(GameTime gameTime)
        {
            GameTime = gameTime;

            {
                var windowSize = SpriteBatch.GraphicsDevice.Viewport.Bounds.Size;

                var widthScalar = windowSize.X / GameWindowBounds.Width;
                var heightScaledByWidthScalar = GameWindowBounds.Height * widthScalar;
                if (heightScaledByWidthScalar < windowSize.Y || heightScaledByWidthScalar.EqualsWithTolerance(windowSize.Y))
                    GameWindowToResizeScalar = widthScalar;
                else
                    GameWindowToResizeScalar = windowSize.Y / GameWindowBounds.Height;

                var widthScaledByResizeScalar = GameWindowBounds.Width * GameWindowToResizeScalar;
                GameWindowToResizeOffset.X = (windowSize.X - widthScaledByResizeScalar) / 2;
            }
        }
    }
}
