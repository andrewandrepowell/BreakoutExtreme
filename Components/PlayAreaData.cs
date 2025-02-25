using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;


namespace BreakoutExtreme.Components
{
    public partial class PlayArea
    {
        private static readonly ReadOnlyDictionary<Components, Action<PlayArea, Vector2>> _componentLoadActions = new(new Dictionary<Components, Action<PlayArea, Vector2>>()
        {
            {
                Components.None, (PlayArea playArea, Vector2 position) => { }
            },
            {
                Components.Ball, (PlayArea playArea, Vector2 position) =>
                {
                    var ball = playArea.CreateBall();
                    var collider = ball.GetCollider();
                    collider.Position = position + (Vector2)(collider.Size / 2);
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
                }
            },
            {
                Components.PowerEmpowered, (PlayArea playArea, Vector2 position) =>
                {
                    var brick = Globals.Runner.CreateBrick(Brick.Bricks.Power, position, Utility.Powers.Empowered);
                    playArea._bricks.Add(brick);
                }
            },
            {
                Components.PowerEnlarge, (PlayArea playArea, Vector2 position) =>
                {
                    var brick = Globals.Runner.CreateBrick(Brick.Bricks.Power, position, Utility.Powers.EnlargePaddle);
                    playArea._bricks.Add(brick);
                }
            },
            {
                Components.PowerNewBall, (PlayArea playArea, Vector2 position) =>
                {
                    var brick = Globals.Runner.CreateBrick(Brick.Bricks.Power, position, Utility.Powers.NewBall);
                    playArea._bricks.Add(brick);
                }
            },
            {
                Components.PowerMultiBall, (PlayArea playArea, Vector2 position) =>
                {
                    var brick = Globals.Runner.CreateBrick(Brick.Bricks.Power, position, Utility.Powers.MultiBall);
                    playArea._bricks.Add(brick);
                }
            },
            {
                Components.PowerProtection, (PlayArea playArea, Vector2 position) =>
                {
                    var brick = Globals.Runner.CreateBrick(Brick.Bricks.Power, position, Utility.Powers.Protection);
                    playArea._bricks.Add(brick);
                }
            },
            {
                Components.BrickSmall, (PlayArea playArea, Vector2 position) =>
                {
                    var brick = Globals.Runner.CreateBrick(Brick.Bricks.Small, position);
                    playArea._bricks.Add(brick);
                }
            },
            {
                Components.BrickMedium, (PlayArea playArea, Vector2 position) =>
                {
                    var brick = Globals.Runner.CreateBrick(Brick.Bricks.Medium, position);
                    playArea._bricks.Add(brick);
                }
            },
            {
                Components.BrickLarge, (PlayArea playArea, Vector2 position) =>
                {
                    var brick = Globals.Runner.CreateBrick(Brick.Bricks.Large, position);
                    playArea._bricks.Add(brick);
                }
            },
            {
                Components.Cannon, (PlayArea playArea, Vector2 position) =>
                {
                    var cannon = Globals.Runner.CreateCannon(Cannon.Cannons.Normal, position, playArea);
                    playArea._cannons.Add(cannon);
                }
            }
        });
        private static readonly ReadOnlyDictionary<Components, char> _componentSymbols = new(new Dictionary<Components, char>()
        {
            { Components.None, '_' },
            { Components.Ball, 'o' },
            { Components.Paddle, 'P' },
            { Components.PowerMultiBall, '0' },
            { Components.PowerProtection, '1' },
            { Components.PowerNewBall, '2' },
            { Components.PowerEnlarge, '3' },
            { Components.PowerEmpowered, '4' },
            { Components.BrickSmall, 'S' },
            { Components.BrickMedium, 'M' },
            { Components.BrickLarge, 'L' },
            { Components.Cannon, 'C' }
        });
        private static readonly ReadOnlyDictionary<char, Components> _symbolComponents = new(_componentSymbols.ToDictionary(e => e.Value, e => e.Key));
    }
}
