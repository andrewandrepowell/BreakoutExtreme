using System.Diagnostics;
using static BreakoutExtreme.Components.Brick;

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
                    var moveToTarget = _paddle.GetMoveToTarget();
                    var cursorSelected = controlState.CursorSelectState == Controller.SelectStates.Pressed || controlState.CursorSelectState == Controller.SelectStates.Held;
                    var cursorReleased = controlState.CursorSelectState == Controller.SelectStates.Released || controlState.CursorSelectState == Controller.SelectStates.None;

                    if (moveToTarget.Moving && ((cursorSelected && controlState.CursorPosition.X != moveToTarget.Target) || cursorReleased))
                        moveToTarget.Release();

                    if (!moveToTarget.Moving && cursorSelected)
                        moveToTarget.MoveTo(controlState.CursorPosition.X);
                }

                if (State == States.Loaded)
                {
                    {
                        Debug.Assert(_balls.Count == 1);
                        var ball = _balls[0];
                        _paddle.GetCollider().GetAttacher().Attach(ball.GetCollider());
                    }

                    State = States.PlayerTakingAim;
                }

                if (State == States.PlayerTakingAim && Globals.ControlState.CursorSelectState == Controller.SelectStates.Released)
                {
                    Debug.Assert(_balls.Count == 1);
                    var ball = _balls[0];
                    _paddle.GetCollider().GetAttacher().Detach(ball.GetCollider());
                    ball.GetLauncher().Launch();

                    State = States.GameRunning;
                }

                // Remove any destroyed bricks.
                {
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
