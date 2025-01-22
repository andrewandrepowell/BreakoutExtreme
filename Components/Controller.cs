using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace BreakoutExtreme.Components
{
    public class Controller
    {
        private ControlStates _controlStates = new();
        private Point _mousePosition = Point.Zero;
        private ButtonState _mouseLeftButtonState = ButtonState.Released;
        private Vector2 TransformPosition(Vector2 position) => position * Scale + Offset;
        public float Scale = 1;
        public Vector2 Offset = Vector2.Zero;
        public RectangleF PaddleBox = RectangleF.Empty;
        public Vector2 PaddlePosition = Vector2.Zero;
        public float PaddleCursorThreshold = 8;
        public enum SelectStates { None, Pressed, Held, Released }
        public class ControlStates
        {
            public Vector2 CursorPosition { get; private set; } = Vector2.Zero;
            public SelectStates CursorSelectState { get; private set; } = SelectStates.None;
            public Directions PaddleMoveDirection { get; private set; } = Directions.None;
            public void Update(Vector2 cursorPosition, SelectStates cursorSelectState,  Directions paddleMoveDirection)
            {
                CursorPosition = cursorPosition;
                CursorSelectState = cursorSelectState;
                PaddleMoveDirection = paddleMoveDirection;
            }
        }
        public ControlStates GetControlStates() => _controlStates;
        public void Update()
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();
            TouchCollection touchState = TouchPanel.GetState();

            var cursorPosition = _controlStates.CursorPosition;
            var cursorSelectState = _controlStates.CursorSelectState;
            var paddleMoveDirection = _controlStates.PaddleMoveDirection;

            if (mouseState.Position != _mousePosition)
            {
                cursorPosition = TransformPosition(mouseState.Position.ToVector2());
                if (PaddleBox.Contains(cursorPosition))
                {
                    var xDistance = Math.Abs(PaddlePosition.X - cursorPosition.X);
                    if (xDistance <= PaddleCursorThreshold)
                        paddleMoveDirection = Directions.None;
                    else if (PaddlePosition.X < cursorPosition.X)
                        paddleMoveDirection = Directions.Left;
                    else if (PaddlePosition.X > cursorPosition.X)
                        paddleMoveDirection = Directions.Right;
                }
                _mousePosition = mouseState.Position;
            }

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
            } else if (mouseState.LeftButton == ButtonState.Released && _mouseLeftButtonState == ButtonState.Released)
            {
                cursorSelectState = SelectStates.None;
            }
            _mouseLeftButtonState = mouseState.LeftButton;

            _controlStates.Update(
                cursorPosition: cursorPosition,
                cursorSelectState: cursorSelectState,
                paddleMoveDirection: paddleMoveDirection);
        }
    }
}
