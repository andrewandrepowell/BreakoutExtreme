using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using System;
using System.Diagnostics;
using BreakoutExtreme.Utility;

namespace BreakoutExtreme.Components
{
    public partial class Animater : IMovable, ITexture
    {
        private Animations _animation = Animations.Ball;
        private Spriter _spriter;
        private Vector2 _position, _shaderDrawOffset;
        private Attacher<IMovable> _attacher;
        private float _visibility = 1, _shaderVisibility = 1, _shaderScale = 1;
        private Color _color = Color.White;
        private float _scale = 1;
        private float _rotation = 0, _shaderRotation = 0f;
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
                for (var i = 0; i < ShaderFeatures.Count; i++)
                {
                    var feature = ShaderFeatures[i];
                    feature.UpdateDrawOffset(ref _shaderDrawOffset);
                    feature.UpdateVisibility(ref _shaderVisibility);
                    feature.UpdateScale(ref  _shaderScale);
                    feature.UpdateRotation(ref _shaderRotation);
                }
                if (_shaderDrawOffset != prevDrawOffset)
                    UpdateSpriterDrawPosition();
                if (_shaderVisibility != prevVisibility)
                    UpdateSpriterColor();
                if (_shaderScale != prevScale)
                    UpdateSpriterScale();
                if (_shaderRotation != prevRotation)
                    UpdateSpriterRotation();
            }

            if (!(Globals.Paused && Pausable))
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
        private void UpdateSpriterRotation()
        {
            _spriter.Rotation = MathHelper.WrapAngle(_rotation + _shaderRotation);
        }
        private void UpdateSpriterScale()
        {
            _spriter.Scale = _scale * _shaderScale;
        }
        private void UpdateSpriterDrawPosition()
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
                UpdateSpriterDrawPosition();
                _attacher.UpdatePositions();
            }
        }
        public float Scale
        {
            get => _spriter.Scale;
            set
            {
                if (_scale == value) return;
                _scale = value;
                UpdateSpriterScale();
            }
        }
        public float Rotation
        {
            get => _rotation;
            set
            {
                if (_rotation == value) return;
                _rotation = value;
                UpdateSpriterRotation();
            }
        }
        public Vector2 Origin => _spriter.Origin;
        public Layers Layer = Layers.Ground;
        public Attacher<IMovable> GetAttacher() => _attacher;
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
        public bool Pausable = true;
        public readonly Bag<Shaders.Feature> ShaderFeatures = [];
        public Texture2D Texture => _spriter.Texture;
        public Rectangle Region => _spriter.Region;
        public bool Running => _spriter.Running;
        public Animater()
        {
            _attacher = new(this);
            UpdateSpriters();
            UpdateSpriter();
            UpdateSpriterDrawPosition();
            UpdateSpriterColor();
            UpdateSpriterScale();
            UpdateSpriterRotation();
        }
        public void Play(Animations animation)
        {
            var differentAnimation = Animation != animation;
            _animation = animation;

            if (differentAnimation && !_spriters.ContainsKey(_animationSpriters[Animation]))
                UpdateSpriters();
            UpdateSpriter();
            UpdateSpriterDrawPosition();
            UpdateSpriterColor();
            UpdateSpriterScale();
            UpdateSpriterRotation();
            _spriter.Play(_animationNames[Animation]);
        }
        public void Stop()
        {
            _spriter.Stop();
        }
        public void Update()
        {
            UpdateShaderFeatures();
            if (!(Globals.Paused && Pausable))
                _spriter.Update();
        }
        public void Draw()
        {
            _spriter.Draw();
        }
    }
}
