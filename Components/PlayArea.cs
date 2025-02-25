using MonoGame.Extended;
using System;
using System.Linq;
using System.Diagnostics;
using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;

namespace BreakoutExtreme.Components
{
    public partial class PlayArea : IUpdate
    {
        private readonly GameWindow _parent;
        private readonly GameBag<Ball> _balls = new();
        private readonly GameBag<Brick> _bricks = new();
        private readonly GameBag<ScorePopup> _scorePopups = new();
        private readonly GameBag<Laser> _lasers = new();
        private readonly GameBag<Cannon> _cannons = new();
        private readonly GameBag<Bomb> _bombs = new();
        private readonly DeathWall _deathWall;
        private readonly Splasher _cleared;
        private readonly Splasher _gameEnd;
        private readonly Splasher _gameStart;
        private Paddle _paddle = null;
        private Levels _level = Levels.Test;
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
        public Levels Level
        {
            get
            {
                Debug.Assert(Loaded);
                return _level;
            }
        }
        public States State { get; private set; } = States.Unloaded;
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
            // Spawn the paddle.
            {
                Debug.Assert(_balls.Count == 1);
                Debug.Assert(_paddle != null);

                var ball = _balls[0];
                _paddle.Spawn();
            }

            _level = level;
            State = States.Loaded;
        }
        public void Unload()
        {
            Debug.Assert(Loaded);

            if (_paddle.Initialized)
                _paddle.RemoveEntity();
            for (var i = 0; i < _balls.Count; i++)
                _balls[i].RemoveEntity();
            for (var i = 0; i < _bricks.Count; i++)
                _bricks[i].RemoveEntity();
            for (var i = 0; i < _scorePopups.Count; i++)
                _scorePopups[i].RemoveEntity();
            for (var i = 0; i < _lasers.Count; i++)
                _lasers[i].RemoveEntity();
            for (var i = 0; i < _cannons.Count; i++)
                _cannons[i].RemoveEntity();

            _balls.Clear();
            _bricks.Clear();
            _scorePopups.Clear();
            _lasers.Clear();
            _cannons.Clear();
            _paddle = null;

            State = States.Unloaded;
        }
        public void GameStart()
        {
            _gameStart.Start();
        }
        public Ball CreateBall()
        {
            var ball = Globals.Runner.CreateBall(this);
            _balls.Add(ball);
            return ball;
        }
        public Bomb CreateBomb(Bomb.Bombs bombEnum, Vector2 position)
        {
            var bomb = Globals.Runner.CreateBomb(bombEnum, position);
            _bombs.Add(bomb);
            return bomb;
        }
        public void DestroyBombs()
        {
            for (var i = 0; i < _bombs.Count; i++)
            {
                var bomb = _bombs[i];
                if (bomb.State != Bomb.States.Spawning && bomb.State != Bomb.States.Active)
                    continue;
                bomb.Destroy();
            }
        }
        public void Protect()
        {
            if (_deathWall.State == DeathWall.States.Active)
                _deathWall.Protect();
            else if (_deathWall.State == DeathWall.States.Protecting && _deathWall.ProtectTimed)
                _deathWall.ReleaseProtect();
        }
        public GameWindow Parent => _parent;
        public PlayArea(GameWindow parent)
        {
            _parent = parent;

            // Create the wall components.
            {
                Globals.Runner.CreateWall(new RectangleF(Globals.PlayAreaBounds.X, Globals.PlayAreaBounds.Y, Globals.GameBlockSize, Globals.PlayAreaBounds.Height));
                Globals.Runner.CreateWall(new RectangleF(Globals.PlayAreaBounds.X + Globals.PlayAreaBounds.Width - Globals.GameBlockSize, Globals.PlayAreaBounds.Y, Globals.GameBlockSize, Globals.PlayAreaBounds.Height));
                Globals.Runner.CreateWall(new RectangleF(Globals.PlayAreaBounds.X + Globals.GameBlockSize, Globals.PlayAreaBounds.Y, Globals.PlayAreaBounds.Width - 2 * Globals.GameBlockSize, Globals.GameBlockSize));
            }

            // Create the death wall.
            {
                _deathWall = Globals.Runner.CreateDeathWall(new RectangleF(Globals.PlayAreaBounds.X + Globals.GameBlockSize, Globals.PlayAreaBounds.Y + Globals.PlayAreaBounds.Height - Globals.GameBlockSize, Globals.PlayAreaBounds.Width - 2 * Globals.GameBlockSize, Globals.GameBlockSize));
                _deathWall.ProtectTimed = true;
                _deathWall.ProtectPeriod = 15;
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

            // Create the splash animations.
            {
                _cleared = Globals.Runner.CreateSplasher(Splasher.Splashes.Cleared);
                _gameEnd = Globals.Runner.CreateSplasher(Splasher.Splashes.GameEnd);
                _gameStart = Globals.Runner.CreateSplasher(Splasher.Splashes.GameStart);
            }
        }
    }
}
