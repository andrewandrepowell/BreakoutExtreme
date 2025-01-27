using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public partial class Animater : IMovable
    {
        private Animations _animation = Animations.Ball;
        private Vector2 _position, _drawPosition;
        private float _scale = 1;
        private Vector2 _scaleVector;
        private IAnimationController _animationController;
        private Attacher<Animater> _attacher;
        private float _visibility = 1;
        private Color _color = Color.White;
        private void UpdateAtlasAnimatedSprites()
        {
            var atlas = _animationAtlases[Animation];
            var atlasAssetName = _atlasAssetNames[atlas];
            var atlasTexture = Globals.ContentManager.Load<Texture2D>(atlasAssetName);
            var atlasRegionSize = _atlasRegionSizes[atlas];
            var atlasObject = Texture2DAtlas.Create(atlasAssetName, atlasTexture, atlasRegionSize.Width, atlasRegionSize.Height);
            var spriteSheet = new SpriteSheet(atlasAssetName, atlasObject);
            _atlasConfigureAnimations[atlas](spriteSheet);
            var animatedSprite = new AnimatedSprite(spriteSheet, _animationNames[Animation]) { OriginNormalized = new Vector2(.5f, .5f) };
            Debug.Assert(!_atlasAnimatedSprites.ContainsKey(atlas));
            _atlasAnimatedSprites.Add(atlas, animatedSprite);
        }
        private void UpdateDrawPosition()
        {
            _drawPosition.X = (float)Math.Floor(_position.X);
            _drawPosition.Y = (float)Math.Floor(_position.Y);
        }
        private void UpdateScaleVector()
        {
            _scaleVector.X = _scale;
            _scaleVector.Y = _scale;
        }
        private void UpdateAnimationController()
        {
            _animationController = _atlasAnimatedSprites[_animationAtlases[Animation]].SetAnimation(_animationNames[Animation]);
        }
        private void UpdateAnimatedSpriteColor()
        {
            _atlasAnimatedSprites[_animationAtlases[Animation]].Color = Color * Visibility;
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
            get => _scale;
            set
            {
                if (_scale == value)
                    return;
                _scale = value;
                UpdateScaleVector();
            }
        }
        public float Rotation = 0;
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
                UpdateAnimatedSpriteColor();
            }
        }
        public Color Color
        {
            get => _color;
            set
            {
                if (_color == value) return;
                _color = value;
                UpdateAnimatedSpriteColor();
            }
        }
        public bool ShowAnimatedSprite = true;
        public Animater()
        {
            UpdateAtlasAnimatedSprites();
            UpdateDrawPosition();
            UpdateScaleVector();
            UpdateAnimationController();
            _attacher = new(this);
        }
        public void Play(Animations animation)
        {
            var differentAnimation = Animation != animation;
            _animation = animation;

            if (differentAnimation && !_atlasAnimatedSprites.ContainsKey(_animationAtlases[Animation]))
                UpdateAtlasAnimatedSprites();

            UpdateAnimationController();
            _animationController.Play();
        }
        public void Update()
        {
            _atlasAnimatedSprites[_animationAtlases[Animation]].Update(Globals.GameTime);
        }
        public void Draw()
        {
            Globals.SpriteBatch.Draw(
                sprite: _atlasAnimatedSprites[_animationAtlases[Animation]], 
                position: _drawPosition, 
                rotation: Rotation, 
                scale: _scaleVector);
        }
    }
}
