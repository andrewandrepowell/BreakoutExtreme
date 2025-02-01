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
                    var ball = Globals.Runner.CreateBall(playArea);
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
                }
            },
            {
                Components.ThickBrick, (PlayArea playArea, Vector2 position) =>
                {
                    var brick = Globals.Runner.CreateBrick(Brick.Bricks.ThickBrick, position);
                    var collider = brick.GetCollider();
                    playArea._bricks.Add(brick);
                }
            }
        });
        private static readonly ReadOnlyDictionary<Components, char> _componentSymbols = new(new Dictionary<Components, char>()
        {
            { Components.None, '_' },
            { Components.Ball, 'o' },
            { Components.Paddle, 'P' },
            { Components.ThickBrick, 'B' }
        });
        private static readonly ReadOnlyDictionary<char, Components> _symbolComponents = new(_componentSymbols.ToDictionary(e => e.Value, e => e.Key));
    }
}
