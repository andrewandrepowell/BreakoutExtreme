using System.Diagnostics;
using System;
using MonoGame.Extended;

namespace BreakoutExtreme.Components
{
    public partial class PlayArea
    {
        public void Update()
        {
            if (Loaded && !Globals.Paused)
            {
                var controlState = Globals.ControlState;
                var keyInput = controlState.Input == Controller.Inputs.Keyboard;

                // Apply user control
                {
                    var cursorInPlayArea = Globals.PlayAreaBounds.Top <= controlState.CursorPosition.Y && Globals.PlayAreaBounds.Bottom >= controlState.CursorPosition.Y;
                    var cursorSelected = controlState.CursorSelectState == Controller.SelectStates.Pressed || controlState.CursorSelectState == Controller.SelectStates.Held;
                    var cursorReleased = controlState.CursorSelectState == Controller.SelectStates.Released;
                    var keyLeftSelected = keyInput && controlState.KeyLeft;
                    var keyRightSelected = keyInput && controlState.KeyRight;
                    var keyFired = keyInput && controlState.KeyFired;
                    var keyDirectionSelected = keyLeftSelected || keyRightSelected;

                    // When game ending or level clearing, the paddle isn't available, so don't run paddle control.
                    if (State != States.Clearing && State != States.GameEnding && (cursorInPlayArea || keyInput) && !_parent.MenuLocked)
                    {
                        var paddleCenter = _paddle.GetCollider().Bounds.BoundingRectangle.Center;
                        var cursorX = 
                            keyLeftSelected ? Globals.PlayAreaBounds.Left:
                            keyRightSelected ? Globals.PlayAreaBounds.Right: controlState.CursorPosition.X;
                        var cursorReachedTarget = Math.Abs(paddleCenter.X - cursorX) <= _paddle.TargetThreshold;

                        // Stop moving the paddle around.
                        if (_paddle.RunningMoveToTarget && 
                            (((cursorSelected || keyDirectionSelected) && cursorReachedTarget) || 
                             (cursorReleased || (keyInput && !keyDirectionSelected))))
                        {
                            Console.WriteLine("Stopped");
                            _paddle.StopMoveToTarget();
                        }

                        // Start moving the paddle around to cursor.
                        if (!_paddle.RunningMoveToTarget && (cursorSelected || keyDirectionSelected) && !cursorReachedTarget)
                        {
                            Console.WriteLine("Moving");
                            _paddle.StartMoveToTarget(cursorX);
                        }

                        // Execute laser firing logic.
                        if (State == States.GameRunning && (cursorReleased || controlState.KeyFired))
                        {
                            var laser = Globals.Runner.CreateLaser(this, _paddle.Empowered);
                            var collider = laser.GetCollider();
                            collider.Position = paddleCenter - (collider.Bounds.BoundingRectangle.Size / 2);
                            _lasers.Add(laser);
                            _paddle.LaserGlow();
                        }

                        // GAME RUNNING STATE
                        if (State == States.PlayerTakingAim && !_gameStart.Running && !_parent.MenuLocked && 
                            ((!keyInput && controlState.CursorSelectState == Controller.SelectStates.Released) || 
                             (keyInput && controlState.KeyFired)))
                        {
                            var bricksActive = true;
                            for (var i = 0; i < _bricks.Count; i++)
                            {
                                bricksActive &= _bricks[i].State == Brick.States.Active;
                            }
                            if (bricksActive)
                            {
                                {
                                    Debug.Assert(_balls.Count == 1);
                                    var ball = _balls[0];
                                    Debug.Assert(_paddle.Attached);
                                    _paddle.Detach();
                                    ball.StartLaunch(_ballMagnitude);
                                }
                                State = States.GameRunning;
                            }
                        }
                    }
                }

                // SPAWN NEW BALL STATE
                if (State == States.GameRunning && _balls.Count == 0 && _parent.RemainingBalls > 0)
                {
                    CreateBall();
                    _parent.DropBall();
                    State = States.SpawnNewBall;
                }

                // GAME END STATE
                if (State == States.GameRunning && _balls.Count == 0 && _parent.RemainingBalls == 0)
                {
                    Debug.Assert(!_gameEnd.Running);
                    _ballInPlay = false;
                    _gameEnd.Start();
                    _paddle.Destroy();
                    DestroyBombs();
                    State = States.GameEnding;
                }

                // PLAYER TAKING AIM STATE 
                // It's imperative the SPAWN NEW BALL STATE takes place before this one.
                if (State == States.Loaded || State == States.SpawnNewBall)
                {
                    {
                        Debug.Assert(_balls.Count == 1);
                        var ball = _balls[0];
                        _ballInPlay = true;
                        ball.Spawn();
                        _paddle.Attach(ball);
                    }
                    {
                        _intenseTime = 0;
                        _updateMagnitudeTime = 0;
                        _timeElapsedSinceLaunch = 0;
                        UpdateMagnitudes();
                    }
                    State = States.PlayerTakingAim;
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
                    DestroyBombs();

                    _parent.LevelsCleared++;

                    State = States.Clearing;
                }

                // UNLOAD
                if ((State == States.Clearing && !_cleared.Running && _balls.Count == 0 && _paddle.Destroyed && _bombs.Count == 0) ||
                    (State == States.GameEnding && !_gameEnd.Running && _paddle.Destroyed && _bombs.Count == 0))
                {
                    Unload();
                }

                // Update magnitudes of all active balls and the paddle in play.
                if (State == States.GameRunning)
                {
                    while (_updateMagnitudeTime <= 0)
                    {
                        UpdateMagnitudes();
                        _updateMagnitudeTime += _timeToUpdateMagnitudes;
                    }
                }

                // Remove any destroyed objects.
                {
                    _balls.Destroy();
                    _bricks.Destroy();
                    _scorePopups.Destroy();
                    _lasers.Destroy();
                    _cannons.Destroy();
                    _bombs.Destroy();
                }

                // Decrement timers
                var timeElapsed = Globals.GameTime.GetElapsedSeconds();
                if (_intenseTime > 0)
                    _intenseTime -= timeElapsed;
                if (_updateMagnitudeTime > 0)
                    _updateMagnitudeTime -= timeElapsed;
                if (State == States.GameRunning)
                    _timeElapsedSinceLaunch += timeElapsed;
            }
        }
    }
}
