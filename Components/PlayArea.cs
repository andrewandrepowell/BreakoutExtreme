using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics;
using MonoGame.Extended.Collections;
using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;

namespace BreakoutExtreme.Components
{
    public partial class PlayArea
    {
        private static readonly ReadOnlyDictionary<Components, Action<PlayArea, Vector2>> _componentLoadActions = new(new Dictionary <Components, Action<PlayArea, Vector2>>() 
        {
            {
                Components.None, (PlayArea playArea, Vector2 position) => { }
            },
            {
                Components.Ball, (PlayArea playArea, Vector2 position) =>
                {
                    var ball = Globals.Runner.CreateBall();
                    var collider = ball.GetCollider();
                    collider.Position = position + (Vector2)(collider.Size / 2);
                    playArea._balls.Add(ball);
                }
            },
            {
                Components.Paddle, (PlayArea playArea, Vector2 position) => 
                {
                    var paddle = Globals.Runner.CreatePaddle();
                    var collider = paddle.GetCollider();
                    collider.Position = position;
                    Debug.Assert(playArea._paddle == null);
                    playArea._paddle = paddle;
                    Console.WriteLine($"Paddle={collider.Bounds.BoundingRectangle}, Position={collider.Position}");
                }
            }
        });
        private static readonly ReadOnlyDictionary<Components, char> _componentSymbols = new(new Dictionary<Components, char>()
        {
            { Components.None, '_' },
            { Components.Ball, 'o' },
            { Components.Paddle, 'P' }
        });
        private static readonly ReadOnlyDictionary<char, Components> _symbolComponents = new(_componentSymbols.ToDictionary(e => e.Value, e => e.Key));
        private Bag<Ball> _balls = new();
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
        public void Load(Levels level)
        {
            Debug.Assert(!Loaded);
            Debug.Assert(_balls.Count == 0);

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

            _level = level;
            State = States.PlayerTakingAim;
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
        public PlayArea()
        {
            Globals.Runner.CreateWall(new RectangleF(Globals.PlayAreaBounds.X, Globals.PlayAreaBounds.Y, Globals.GameBlockSize, Globals.PlayAreaBounds.Height));
            Globals.Runner.CreateWall(new RectangleF(Globals.PlayAreaBounds.X + Globals.PlayAreaBounds.Width - Globals.GameBlockSize, Globals.PlayAreaBounds.Y, Globals.GameBlockSize, Globals.PlayAreaBounds.Height));
            Globals.Runner.CreateWall(new RectangleF(Globals.PlayAreaBounds.X + Globals.GameBlockSize, Globals.PlayAreaBounds.Y, Globals.PlayAreaBounds.Width - 2 * Globals.GameBlockSize, Globals.GameBlockSize));
            Globals.Runner.CreateWall(new RectangleF(Globals.PlayAreaBounds.X + Globals.GameBlockSize, Globals.PlayAreaBounds.Y + Globals.PlayAreaBounds.Height - Globals.GameBlockSize, Globals.PlayAreaBounds.Width - 2 * Globals.GameBlockSize, Globals.GameBlockSize));

            var ninePatcher = Globals.Runner.CreateNinePatcher();
            ninePatcher.Texture = NinePatcher.Textures.PlayAreaBottomRemoved;
            ninePatcher.Bounds = Globals.PlayAreaBounds;
        }
    }
}
