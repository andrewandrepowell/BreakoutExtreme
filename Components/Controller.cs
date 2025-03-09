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
        private readonly static Keys[] _leftKeys = [Keys.Left, Keys.A];
        private readonly static Keys[] _rightKeys = [Keys.Right, Keys.D];
        private readonly static Keys[] _fireKeys = [Keys.Space];
        private readonly static Keys[] _pauseKeys = [Keys.Enter, Keys.Escape, Keys.Back];
        private Vector2 _mousePosition = Vector2.Zero, _touchPosition = Vector2.Zero;
        private ButtonState _mouseLeftButtonState = ButtonState.Released;
        private bool _touchPressed = false;
        private bool _keyFiredHeld = false;
        private bool _keyPausedHeld = false;
        private static Vector2 TransformPosition(Vector2 position) => (position - Globals.GameWindowToResizeOffset) / Globals.GameWindowToResizeScalar;
        public RectangleF PaddleBox = RectangleF.Empty;
        public Vector2 PaddlePosition = Vector2.Zero;
        public float PaddleCursorThreshold = 8;
        public enum SelectStates { None, Pressed, Held, Released }
        public enum Inputs { Mouse, Touch, Keyboard }
        public class ControlState
        {
            public Vector2 CursorPosition { get; private set; } = Vector2.Zero;
            public SelectStates CursorSelectState { get; private set; } = SelectStates.None;
            public Inputs Input { get; private set; } = Inputs.Mouse;
            public bool KeyFired { get; private set; } = false;
            public bool KeyPaused { get; private set; } = false;
            public bool KeyLeft { get; private set; } = false;
            public bool KeyRight { get; private set; } = false;
            public void Update(
                Inputs input,
                Vector2 cursorPosition, 
                SelectStates cursorSelectState,
                bool keyFired, bool keyPaused, bool keyLeft, bool keyRight)
            {
                Input = input;
                CursorPosition = cursorPosition;
                CursorSelectState = cursorSelectState;
                KeyFired = keyFired;
                KeyPaused = keyPaused;
                KeyLeft = keyLeft;
                KeyRight = keyRight;
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
                KeyboardState keyboardState = Keyboard.GetState();

                // Determine keyboard input.
                bool keyFiredHeld = false;
                bool keyFiredPressed = false;
                bool keyPausedHeld = false;
                bool keyPausedPressed = false;
                bool keyLeftHeld = false;
                bool keyRightHeld = false;
                {
                    for (var i = 0; i < _fireKeys.Length; i++)
                        if (keyboardState.IsKeyDown(_fireKeys[i]))
                            keyFiredHeld = true;
                    for (var i = 0; i < _pauseKeys.Length; i++)
                        if (keyboardState.IsKeyDown(_pauseKeys[i]))
                            keyPausedHeld = true;
                    for (var i = 0; i < _leftKeys.Length; i++)
                        if (keyboardState.IsKeyDown(_leftKeys[i]))
                            keyLeftHeld = true;
                    for (var i = 0; i < _rightKeys.Length; i++)
                        if (keyboardState.IsKeyDown(_rightKeys[i]))
                            keyRightHeld = true;

                    keyFiredPressed = keyFiredHeld & !_keyFiredHeld;
                    _keyFiredHeld = keyFiredHeld;

                    keyPausedPressed = keyPausedHeld & !_keyPausedHeld;
                    _keyPausedHeld = keyPausedHeld;
                }


                var cursorPosition = _controlState.CursorPosition;
                var cursorSelectState = _controlState.CursorSelectState;

                // Determine input
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                        Input = Inputs.Mouse;
                    else if (touchCollection.Count > 0)
                        Input = Inputs.Touch;
                    else if (keyFiredHeld || keyPausedHeld || keyLeftHeld || keyRightHeld)
                        Input = Inputs.Keyboard;
                }

                // Mouse controls.
                if (Input == Inputs.Mouse)
                {
                    var mousePosition = TransformPosition(mouseState.Position.ToVector2());
                    if (mousePosition != _mousePosition)
                    {
                        cursorPosition = TransformPosition(mouseState.Position.ToVector2());
                        _mousePosition = mousePosition;
                    }

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
                if (Input == Inputs.Touch)
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

                Globals.Logger.Message = $"Cursor Position: {cursorPosition}. Cursor Select State: {cursorSelectState}. Input: {Input}";

                _controlState.Update(
                    input: Input,
                    cursorPosition: cursorPosition,
                    cursorSelectState: cursorSelectState,
                    keyFired: keyFiredPressed,
                    keyPaused: keyPausedPressed,
                    keyLeft: keyLeftHeld,
                    keyRight: keyRightHeld);
            }
        }
    }
}
