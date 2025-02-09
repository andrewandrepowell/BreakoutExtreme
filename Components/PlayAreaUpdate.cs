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
                    var cursorReleased = controlState.CursorSelectState == Controller.SelectStates.Released;

                    // When clearing, the paddle isn't available, so don't run paddle control.
                    if (State != States.Clearing)
                    {
                        // Stop moving the paddle around.
                        if (_paddle.RunningMoveToTarget && ((cursorSelected && controlState.CursorPosition.X != _paddle.TargetToMoveTo) || cursorReleased))
                        _paddle.StopMoveToTarget();

                        // Start moving the paddle around to cursor.
                        if (!_paddle.RunningMoveToTarget && cursorSelected)
                            _paddle.StartMoveToTarget(controlState.CursorPosition.X);

                        // IMMA FIRIN MA LASSSER.
                        if (State == States.GameRunning && cursorReleased)
                        {
                            var laser = Globals.Runner.CreateLaser();
                            var collider = laser.GetCollider();
                            collider.Position = _paddle.GetCollider().Bounds.BoundingRectangle.Center - (collider.Bounds.BoundingRectangle.Size / 2);
                            _lasers.Add(laser);
                            _paddle.LaserGlow();
                        }
                    }
                }

                // SPAWN NEW BALL STATE
                if (State == States.GameRunning && _balls.Count == 0 && _parent.RemainingBalls > 0)
                {
                    var ball = Globals.Runner.CreateBall(this);
                    ball.GetCollider().Position = _paddle.GetCollider().Position + _ballInitialDisplacementFromPaddle;
                    _balls.Add(ball);
                    _parent.RemainingBalls--;
                    State = States.SpawnNewBall;
                }

                // PLAYER TAKING AIM STATE 
                // It's imperative the SPAWN NEW BALL STATE takes place before this one.
                if (State == States.Loaded || State == States.SpawnNewBall)
                {
                    {
                        Debug.Assert(_balls.Count == 1);
                        var ball = _balls[0];
                        ball.Spawn();
                        ball.GetCollider().Position = _paddle.GetCollider().Position + _ballInitialDisplacementFromPaddle;
                        ball.AttachTo(_paddle);
                    }

                    State = States.PlayerTakingAim;
                }

                // GAME RUNNING STATE
                if (State == States.PlayerTakingAim && Globals.ControlState.CursorSelectState == Controller.SelectStates.Released)
                {
                    var bricksActive = true;
                    for (var i = 0; i < _bricks.Count; i++)
                    {
                        bricksActive &= _bricks[i].State == Brick.States.Active;
                    }
                    if (bricksActive)
                    {
                        Debug.Assert(_balls.Count == 1);
                        var ball = _balls[0];
                        Debug.Assert(ball.State == Ball.States.Attached);
                        ball.Detach();
                        ball.StartLaunch();
                        State = States.GameRunning;
                    }
                }

                // CLEARING
                if (State == States.GameRunning && _bricks.Count == 0 && _cannons.Count == 0)
                {
                    Debug.Assert(!_cleared.Running);

                    _cleared.Start();

                    _paddle.Despawn();
                    for (var i = 0; i < _balls.Count; i++)
                    {
                        var ball = _balls[i];
                        if (ball.State != Ball.States.Active)
                            continue;
                        ball.StopLaunch();
                        ball.Despawn();
                    }

                    _parent.LevelsCleared++;

                    State = States.Clearing;
                }

                // UNLOAD
                if (State == States.Clearing && !_cleared.Running && _balls.Count == 0 && _paddle.State == Paddle.States.Destroyed)
                {
                    Unload();
                }

                // Remove any destroyed objects.
                {
                    _balls.Destroy();
                    _bricks.Destroy();
                    _scorePopups.Destroy();
                    _lasers.Destroy();
                    _cannons.Destroy();
                }
            }
        }
    }
}
