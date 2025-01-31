﻿using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public partial class PlayArea
    {
        public void Update()
        {
            if (Loaded)
            {

                // Apply user control
                {
                    var controlState = Globals.ControlState;
                    var cursorSelected = controlState.CursorSelectState == Controller.SelectStates.Pressed || controlState.CursorSelectState == Controller.SelectStates.Held;
                    var cursorReleased = controlState.CursorSelectState == Controller.SelectStates.Released || controlState.CursorSelectState == Controller.SelectStates.None;

                    if (_paddle.RunningMoveToTarget && ((cursorSelected && controlState.CursorPosition.X != _paddle.TargetToMoveTo) || cursorReleased))
                        _paddle.StopMoveToTarget();

                    if (!_paddle.RunningMoveToTarget && cursorSelected)
                        _paddle.StartMoveToTarget(controlState.CursorPosition.X);
                }

                if (State == States.Loaded || State == States.SpawnNewBall)
                {
                    {
                        Debug.Assert(_balls.Count == 1);
                        Debug.Assert(_paddle != null);
                        var ball = _balls[0];
                        ball.Spawn();
                        ball.GetCollider().Position = _paddle.GetCollider().Position + _ballInitialDisplacementFromPaddle;
                        _paddle.GetCollider().GetAttacher().Attach(ball.GetCollider());
                    }

                    State = States.PlayerTakingAim;
                }

                if (State == States.PlayerTakingAim && Globals.ControlState.CursorSelectState == Controller.SelectStates.Released)
                {
                    Debug.Assert(_balls.Count == 1);
                    var ball = _balls[0];
                    _paddle.GetCollider().GetAttacher().Detach(ball.GetCollider());
                    ball.StartLaunch();
                    State = States.GameRunning;
                }

                if (State == States.GameRunning && _balls.Count == 0 && _parent.RemainingBalls > 0)
                {
                    var ball = Globals.Runner.CreateBall(this);
                    ball.GetCollider().Position = _paddle.GetCollider().Position + _ballInitialDisplacementFromPaddle;
                    _balls.Add(ball);
                    _parent.RemainingBalls--;
                    State = States.SpawnNewBall;
                }

                // Remove any objects.
                {
                    // balls
                    _destroyedBalls.Clear();
                    for (var i = 0; i < _balls.Count; i++)
                    {
                        var ball = _balls[i];
                        if (ball.State == Ball.States.Destroyed)
                        {

                            _destroyedBalls.Add(ball);
                        }
                    }
                    for (var i = 0; i < _destroyedBalls.Count; i++)
                    {
                        var ball = _destroyedBalls[i];
                        ball.RemoveEntity();
                        _balls.Remove(ball);
                    }

                    // bricks
                    _destroyedBricks.Clear();
                    for (var i = 0; i < _bricks.Count; i++)
                    {
                        var brick = _bricks[i];
                        if (brick.State == Brick.States.Destroyed)
                        {
                            
                            _destroyedBricks.Add(brick);
                        }
                    }
                    for (var i = 0; i < _destroyedBricks.Count; i++)
                    {
                        var brick = _destroyedBricks[i];
                        brick.RemoveEntity();
                        _bricks.Remove(brick);
                    }
                }

                // Update all game components.
                {
                    _paddle.Update();
                    for (var i = 0; i < _balls.Count; i++)
                        _balls[i].Update();
                    for (var i = 0; i < _bricks.Count; i++)
                        _bricks[i].Update();
                }
            }
        }
    }
}
