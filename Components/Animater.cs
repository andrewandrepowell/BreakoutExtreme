using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using System;
using System.Diagnostics;
using BreakoutExtreme.Utility;

namespace BreakoutExtreme.Components
{
    public partial class Animater : IMovable
    {
        private Animations _animation = Animations.Ball;
        private Spriter _spriter;
        private Vector2 _position, _shaderDrawOffset;
        private Attacher<Animater> _attacher;
        private float _visibility = 1, _shaderVisibility = 1;
        private Color _color = Color.White;
        private void UpdateShaderFeatures()
        {
            {
                _shaderDrawOffset = Vector2.Zero;
                _shaderVisibility = 1;
                var updateDrawPosition = false;
                var updateVisibility = false;
                for (var i = 0; i < ShaderFeatures.Count; i++)
                {
                    var feature = ShaderFeatures[i];
                    updateDrawPosition |= feature.UpdateDrawOffset(ref _shaderDrawOffset);
                    updateVisibility |= feature.UpdateVisibility(ref _shaderVisibility);
                }
                if (updateDrawPosition)
                    UpdateDrawPosition();
                if (updateVisibility)
                    UpdateSpriterColor();
            }

            for (var i = 0; i < ShaderFeatures.Count; i++)
                ShaderFeatures[i].Update();
        }
        private void UpdateSpriters()
        {
            var spriter = _animationSpriters[Animation];
            var spriterAssetName = _spriterAssetNames[spriter];
            var spriterTexture = Globals.ContentManager.Load<Texture2D>(spriterAssetName);
            var spriterRegionSize = _spriterRegionSizes[spriter];
            var spriterObj = new Spriter(spriterAssetName, spriterRegionSize);
            _spriterConfigureAnimations[spriter](spriterObj);
            Debug.Assert(!_spriters.ContainsKey(spriter));
            _spriters.Add(spriter, spriterObj);
        }
        private void UpdateSpriter()
        {
            _spriter = _spriters[_animationSpriters[Animation]];
        }
        private void UpdateDrawPosition()
        {
            _spriter.Position.X = (float)Math.Floor(_position.X + _shaderDrawOffset.X);
            _spriter.Position.Y = (float)Math.Floor(_position.Y + _shaderDrawOffset.Y);
        }
        private void UpdateSpriterColor()
        {
            _spriter.Color = Color * Visibility * _shaderVisibility;
        }
        public static void Load()
        {
            var animater = new Animater();
            var animations = Enum.GetValues<Animations>();
            foreach (ref var animation in animations.AsSpan())
            {
                animater.Play(animation);
            }
        }
        public Animations Animation => _animation;
        public Vector2 Position
        {
            get => _position;
            set
            {
                if (_position == value)
                    return;
                _position = value;
                UpdateDrawPosition();
                _attacher.UpdatePositions();
            }
        }
        public float Scale
        {
            get => _spriter.Scale;
            set => _spriter.Scale = value;
        }
        public float Rotation
        {
            get => _spriter.Rotation;
            set => _spriter.Rotation = value;
        }
        public Layers Layer = Layers.Ground;
        public Attacher<Animater> GetAttacher() => _attacher;
        public float Visibility
        {
            get => _visibility;
            set
            {
                if (_visibility == value)
                    return;
                _visibility = value;
                UpdateSpriterColor();
            }
        }
        public Color Color
        {
            get => _color;
            set
            {
                if (_color == value) return;
                _color = value;
                UpdateSpriterColor();
            }
        }
        public bool ShowBase = true;
        public readonly Bag<Shaders.Feature> ShaderFeatures = [];
        public Texture2D Texture => _spriter.Texture;
        public Rectangle Region => _spriter.Region;
        public bool Running => _spriter.Running;
        public Animater()
        {
            _attacher = new(this);
            UpdateSpriters();
            UpdateSpriter();
            UpdateDrawPosition();
            UpdateSpriterColor();
        }
        public void Play(Animations animation)
        {
            var differentAnimation = Animation != animation;
            _animation = animation;

            if (differentAnimation && !_spriters.ContainsKey(_animationSpriters[Animation]))
                UpdateSpriters();
            UpdateSpriter();
            UpdateSpriterColor();
            _spriter.Play(_animationNames[Animation]);
        }
        public void Stop()
        {
            _spriter.Stop();
        }
        public void Update()
        {
            UpdateShaderFeatures();
            _spriter.Update();
        }
        public void Draw()
        {
            _spriter.Draw();
        }
    }
}
