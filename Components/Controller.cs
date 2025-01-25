using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Linq;

namespace BreakoutExtreme.Components
{
    public class Controller
    {
        private readonly ControlState _controlState = new();
        private Vector2 _mousePosition = Vector2.Zero, _touchPosition = Vector2.Zero;
        private ButtonState _mouseLeftButtonState = ButtonState.Released;
        private bool _touchPressed = false;
        private static Vector2 TransformPosition(Vector2 position) => (position - Globals.GameWindowToResizeOffset) / Globals.GameWindowToResizeScalar;
        public RectangleF PaddleBox = RectangleF.Empty;
        public Vector2 PaddlePosition = Vector2.Zero;
        public float PaddleCursorThreshold = 8;
        public enum SelectStates { None, Pressed, Held, Released }
        public enum Inputs { Mouse, Touch }
        public class ControlState
        {
            public Vector2 CursorPosition { get; private set; } = Vector2.Zero;
            public SelectStates CursorSelectState { get; private set; } = SelectStates.None;
            public void Update(Vector2 cursorPosition, SelectStates cursorSelectState)
            {
                CursorPosition = cursorPosition;
                CursorSelectState = cursorSelectState;
            }
        }
        public ControlState GetControlState() => _controlState;
        public Inputs Input { get; private set; } = Inputs.Mouse;
        public void Update()
        {
            // Update controls
            {
                MouseState mouseState = Mouse.GetState();
                TouchCollection touchCollection = TouchPanel.GetState();

                var cursorPosition = _controlState.CursorPosition;
                var cursorSelectState = _controlState.CursorSelectState;

                // Mouse controls.
                {
                    var mousePosition = TransformPosition(mouseState.Position.ToVector2());
                    if (mousePosition != _mousePosition)
                    {
                        cursorPosition = TransformPosition(mouseState.Position.ToVector2());
                        Input = Inputs.Mouse;
                        _mousePosition = mousePosition;
                    }

                    if (Input == Inputs.Mouse)
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed && _mouseLeftButtonState == ButtonState.Released)
                        {
                            cursorSelectState = SelectStates.Pressed;
                        }
                        else if (mouseState.LeftButton == ButtonState.Pressed && _mouseLeftButtonState == ButtonState.Pressed)
                        {
                            cursorSelectState = SelectStates.Held;
                        }
                        else if (mouseState.LeftButton == ButtonState.Released && _mouseLeftButtonState == ButtonState.Pressed)
                        {
                            cursorSelectState = SelectStates.Released;
                        }
                        else if (mouseState.LeftButton == ButtonState.Released && _mouseLeftButtonState == ButtonState.Released)
                        {
                            cursorSelectState = SelectStates.None;
                        }
                        _mouseLeftButtonState = mouseState.LeftButton;
                    }
                }

                // Touch controls
                {
                    var touchPressed = touchCollection.Count > 0;

                    if (touchCollection.Count > 0)
                    {
                        var touchLocation = touchCollection[^1];

                        var touchPosition = TransformPosition(touchLocation.Position);
                        if (touchPosition != _touchPosition)
                        {
                            cursorPosition = touchPosition;
                            Input = Inputs.Touch;
                            _touchPosition = touchPosition;
                        }
                    }

                    if (Input == Inputs.Touch)
                    {
                        if (touchPressed && !_touchPressed)
                        {
                            cursorSelectState = SelectStates.Pressed;
                        }
                        else if (touchPressed && _touchPressed)
                        {
                            cursorSelectState = SelectStates.Held;
                        }
                        else if (!touchPressed && _touchPressed)
                        {
                            cursorSelectState = SelectStates.Released;
                        }
                        else if (!touchPressed && !_touchPressed)
                        {
                            cursorSelectState = SelectStates.None;
                        }
                        _touchPressed = touchPressed;
                    }
                }

                _controlState.Update(
                    cursorPosition: cursorPosition,
                    cursorSelectState: cursorSelectState);
            }
        }
    }
}
