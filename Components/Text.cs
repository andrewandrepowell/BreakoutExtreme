using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;


namespace BreakoutExtreme.Components
{
    public class Text
    {
        private static readonly ReadOnlyDictionary<Fonts, string> _fontAssetNames = new(new Dictionary<Fonts, string>() 
        {
            {  Fonts.Montserrat, "fonts/montserrat/montserrat" }
        });
        private Fonts _font = Fonts.Montserrat;
        private string _message = "";
        private BitmapFont _bitmapFont;
        private SizeF _size;
        private Vector2 _origin;
        private Vector2 _position, _drawPosition;
        private void UpdateSize()
        {
            _size = _bitmapFont.MeasureString(Message);
            _origin = _size / 2;
        }
        private void UpdateDrawPosition()
        {
            _drawPosition = new Vector2(x: (float)Math.Floor(Position.X), y: (float)Math.Floor(Position.Y));
        }
        private void UpdateBitmapFont()
        {
            _bitmapFont = Globals.ContentManager.Load<BitmapFont>(_fontAssetNames[Font]);
        }
        public enum Fonts
        {
            Montserrat
        }
        public string Message
        {
            get => _message;
            set
            {
                if (_message == value)
                    return;
                _message = value;
                UpdateSize();
            }
        }
        public Fonts Font
        {
            get => _font;
            set
            {
                if (_font == value)
                    return;
                _font = value;
                UpdateBitmapFont();
                UpdateSize();
            }
        }
        public Vector2 Position
        {
            get => _position;
            set
            {
                if (_position == value)
                    return;
                _position = value;
                UpdateDrawPosition();
            }
        }
        public SizeF Size => _size;
        public Color Color = Color.Black;
        public Text() 
        {
            UpdateBitmapFont();
        }
        public void Draw()
        {
            Globals.SpriteBatch.DrawString(
                font: _bitmapFont,
                text: Message,
                color: Color,
                position: _drawPosition,
                rotation: 0,
                origin: _origin,
                scale: 1,
                effect: SpriteEffects.None, 
                layerDepth: 0);
        }
    }
}
