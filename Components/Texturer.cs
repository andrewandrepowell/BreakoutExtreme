using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using MonoGame.Extended.Collections;
using BreakoutExtreme.Shaders;
using BreakoutExtreme.Utility;
using MonoGame.Extended;

namespace BreakoutExtreme.Components
{
    public class Texturer(Animater parent) : IMovable, ITexture
    {
        private Animater _parent = parent;
        private Vector2 _position, _drawPosition, _shaderDrawOffset;
        private Color _color = Color.White, _drawColor = Color.White;
        private float _scale = 1, _drawScale = 1, _shaderScale = 1;
        private float _visibility = 1, _shaderVisibility = 1;
        private float _drawRotation = 0, _rotation = 0, _shaderRotation = 0;
        private Bag<Feature> _shaderFeatures = new();
        private void UpdateShaderFeatures()
        {
            {
                var prevDrawOffset = _shaderDrawOffset;
                var prevVisibility = _shaderVisibility;
                var prevScale = _shaderScale;
                var prevRotation = _shaderRotation;
                _shaderDrawOffset = Vector2.Zero;
                _shaderVisibility = 1;
                _shaderScale = 1;
                _shaderRotation = 0;
                for (var i = 0; i < _shaderFeatures.Count; i++)
                {
                    var feature = _shaderFeatures[i];
                    feature.UpdateDrawOffset(ref _shaderDrawOffset);
                    feature.UpdateVisibility(ref _shaderVisibility);
                    feature.UpdateScale(ref _shaderScale);
                    feature.UpdateRotation(ref _shaderRotation);
                }
                if (_shaderDrawOffset != prevDrawOffset)
                    UpdateDrawPosition();
                if (_shaderVisibility != prevVisibility)
                    UpdateDrawColor();
                if (_shaderScale != prevScale)
                    UpdateDrawScale();
                if (_shaderRotation != prevRotation)
                    UpdateDrawRotation();
            }

            for (var i = 0; i < _shaderFeatures.Count; i++)
                _shaderFeatures[i].Update();
        }
        private void UpdateDrawRotation()
        {
            _drawRotation = _rotation + _shaderRotation;
        }
        private void UpdateDrawScale()
        {
            _drawScale = _scale * _shaderScale;
        }
        private void UpdateDrawPosition()
        {
            _drawPosition.X = (float)Math.Floor(_position.X + _shaderDrawOffset.X);
            _drawPosition.Y = (float)Math.Floor(_position.Y + _shaderDrawOffset.Y);
        }
        private void UpdateDrawColor()
        {
            _drawColor = _color * _visibility * _shaderVisibility;
        }
        public Texture2D Texture => _parent.Texture;
        public Rectangle Region => _parent.Region;
        public Bag<Feature> ShaderFeatures => _shaderFeatures;
        public Layers Layer = Layers.Ground;
        public bool ShowBase = true;
        public Vector2 Position
        {
            get => _position;
            set
            {
                if (value == _position) return;
                _position = value;
                UpdateDrawPosition();
            }
        }
        public float Visibility
        {
            get => _visibility;
            set
            {
                if (value == _visibility) return;
                _visibility = value;
                UpdateDrawColor();
            }
        }
        public Color Color
        {
            get => _color;
            set
            {
                if (value == _color) return;
                _color = value;
                UpdateDrawColor();
            }
        }
        public float Rotation
        {
            get => _rotation;
            set
            {
                if (_rotation == value) return;
                _rotation = value;
                UpdateDrawRotation();
            }
        }
        public float Scale
        {
            get => _scale;
            set
            {
                if (value == _scale) return;
                _scale = value;
                UpdateDrawScale();
            }
        }
        public Animater Parent { get => _parent; set => _parent = value; }
        public bool Pausable = true;
        public void Update()
        {
            if (Globals.Paused && Pausable) return;
            UpdateShaderFeatures();
        }

        public void Draw()
        {
            if (_drawColor.A == 0)
                return;
            Globals.SpriteBatch.Draw(
                texture: _parent.Texture,
                position: _drawPosition,
                sourceRectangle: _parent.Region,
                color: _drawColor,
                rotation: _drawRotation,
                origin: _parent.Origin,
                scale: _drawScale,
                effects: SpriteEffects.None,
                layerDepth: 0);
        }
    }
}
