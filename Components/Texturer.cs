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
                _shaderDrawOffset = Vector2.Zero;
                _shaderVisibility = 1;
                _shaderScale = 1;
                _shaderRotation = 0;
                var updateDrawPosition = false;
                var updateVisibility = false;
                var updateScale = false;
                var updateRotation = false;
                for (var i = 0; i < _shaderFeatures.Count; i++)
                {
                    var feature = _shaderFeatures[i];
                    updateDrawPosition |= feature.UpdateDrawOffset(ref _shaderDrawOffset);
                    updateVisibility |= feature.UpdateVisibility(ref _shaderVisibility);
                    updateScale |= feature.UpdateScale(ref _shaderScale);
                    updateRotation |= feature.UpdateRotation(ref _shaderRotation);
                }
                if (updateDrawPosition)
                    UpdateDrawPosition();
                if (updateVisibility)
                    UpdateDrawColor();
                if (updateScale)
                    UpdateDrawScale();
                if (updateRotation)
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
        public Animater Parent { get => _parent; set => _parent = value; }
        public void Update()
        {
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
