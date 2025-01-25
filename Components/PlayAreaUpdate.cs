using System.Diagnostics;

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
                    var cursorReleased = controlState.CursorSelectState == Controller.SelectStates.Released;

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


                // Update all game components.
                {
                    _paddle.Update();
                    for (var i = 0; i < _balls.Count; i++)
                    {
                        _balls[i].Update();
                    }
                }
            }
        }
    }
}
