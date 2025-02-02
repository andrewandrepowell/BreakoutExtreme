using MonoGame.Extended;
using System;
using System.Linq;
using System.Diagnostics;
using MonoGame.Extended.Collections;
using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;

namespace BreakoutExtreme.Components
{
    public partial class PlayArea
    {
        private readonly GameWindow _parent;
        private readonly Bag<Ball> _balls = [];
        private readonly Bag<Brick> _bricks = [];
        private readonly Bag<ScorePopup> _scorePopups = [];
        private readonly Bag<Ball> _destroyedBalls = [];
        private readonly Bag<Brick> _destroyedBricks = [];
        private readonly Bag<ScorePopup> _destroyedScorePopups = [];
        private readonly DeathWall _deathWall;
        private Paddle _paddle = null;
        private Levels _level = Levels.Test;
        private Vector2 _ballInitialDisplacementFromPaddle;
        public bool Loaded => State != States.Unloaded;
        static PlayArea()
        {
#if DEBUG
            var totalAreaBlocks = Globals.PlayAreaBlockBounds.Width * Globals.PlayAreaBlockBounds.Height;
            foreach (var area in _levelAreas.Values)
            {
                Debug.Assert(area.Length == totalAreaBlocks);
                Debug.Assert(area.All(x=> _componentSymbols.Values.Contains(x)));
                for (var y = 0; y < Globals.PlayAreaBlockBounds.Height; y++)
                {
                    for (var x = 0; x < Globals.PlayAreaBlockBounds.Width; x++)
                    {
                        if (y == 0 || y == Globals.PlayAreaBlockBounds.Height - 1 ||
                            x == 0 || x == Globals.PlayAreaBlockBounds.Width - 1)
                        {
                            Debug.Assert(area[x + y * Globals.PlayAreaBlockBounds.Width] == _componentSymbols[Components.None]);
                        }
                    }
                }
            }
#endif
        }
        public RectangleF Bounds => Globals.PlayAreaBounds;
        public Levels Level
        {
            get
            {
                Debug.Assert(Loaded);
                return _level;
            }
        }
        public States State { get; private set; } = States.Unloaded;
        public GameWindow Parent => _parent;
        public void Load(Levels level)
        {
            Debug.Assert(!Loaded);
            Debug.Assert(_balls.Count == 0);

            // Load all the components of the level.
            {
                var area = _levelAreas[level];
                for (var y = 0; y < Globals.PlayAreaBlockBounds.Height; y++)
                {
                    for (var x = 0; x < Globals.PlayAreaBlockBounds.Width; x++)
                    {
                        var symbol = area[x + y * Globals.PlayAreaBlockBounds.Width];
                        var component = _symbolComponents[symbol];
                        var position = new Point(x + Globals.PlayAreaBlockBounds.X, y + Globals.PlayAreaBlockBounds.Y).ToPosition();
                        _componentLoadActions[component](this, position);
                    }
                }
            }

            // Determine displacement for future balls.
            // Attaching the ball to the paddle occurs in later state.
            {
                Debug.Assert(_balls.Count == 1);
                Debug.Assert(_paddle != null);

                var ball = _balls[0];
                _ballInitialDisplacementFromPaddle = ball.GetCollider().Position - _paddle.GetCollider().Position;
            }

            _level = level;
            State = States.Loaded;
        }
        public void Unload()
        {
            Debug.Assert(Loaded);

            for (var i = 0; i < _balls.Count; i++)
            {
                _balls[i].RemoveEntity();
            }
            _paddle.RemoveEntity();

            _balls.Clear();
            _paddle = null;

            State = States.Unloaded;
        }
        public void UpdateScore(Brick brick)
        {
            var brickCollider = brick.GetCollider();
            var scorePopup = Globals.Runner.CreateScorePopup();
            scorePopup.Text = "+1";
            scorePopup.GetGumDrawer().Position = brickCollider.Position + (Vector2)(brickCollider.Size / 2);
            _scorePopups.Add(scorePopup);
            _parent.Score++;
        }
        public PlayArea(GameWindow parent)
        {
            _parent = parent;

            // Create the wall components.
            {
                Globals.Runner.CreateWall(new RectangleF(Globals.PlayAreaBounds.X, Globals.PlayAreaBounds.Y, Globals.GameBlockSize, Globals.PlayAreaBounds.Height));
                Globals.Runner.CreateWall(new RectangleF(Globals.PlayAreaBounds.X + Globals.PlayAreaBounds.Width - Globals.GameBlockSize, Globals.PlayAreaBounds.Y, Globals.GameBlockSize, Globals.PlayAreaBounds.Height));
                Globals.Runner.CreateWall(new RectangleF(Globals.PlayAreaBounds.X + Globals.GameBlockSize, Globals.PlayAreaBounds.Y, Globals.PlayAreaBounds.Width - 2 * Globals.GameBlockSize, Globals.GameBlockSize));
                _deathWall = Globals.Runner.CreateDeathWall(new RectangleF(Globals.PlayAreaBounds.X + Globals.GameBlockSize, Globals.PlayAreaBounds.Y + Globals.PlayAreaBounds.Height - Globals.GameBlockSize, Globals.PlayAreaBounds.Width - 2 * Globals.GameBlockSize, Globals.GameBlockSize));
            }

            // Create the wall texture.
            {
                var ninePatcher = Globals.Runner.CreateNinePatcher();
                ninePatcher.Texture = NinePatcher.Textures.PlayAreaBottomRemoved;
                ninePatcher.Bounds = Globals.PlayAreaBounds;
            }

            // Create the fill texture.
            {
                var ninePatcher = Globals.Runner.CreateNinePatcher();
                ninePatcher.Texture = NinePatcher.Textures.PlayAreaFilled;
                ninePatcher.Bounds = new Rectangle(Globals.PlayAreaBlockBounds.X + 1, Globals.PlayAreaBlockBounds.Y + 1, Globals.PlayAreaBlockBounds.Width - 2, Globals.PlayAreaBlockBounds.Height - 1).ToBounds();
            }
        }
    }
}
