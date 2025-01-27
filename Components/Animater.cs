﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Collections;
using MonoGame.Extended.Graphics;
using System;
using System.Diagnostics;

namespace BreakoutExtreme.Components
{
    public partial class Animater : IMovable
    {
        private Animations _animation = Animations.Ball;
        private Vector2 _position, _drawPosition, _shaderDrawOffset;
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
            Debug.Assert(!_atlasNodes.ContainsKey(atlas));
            _atlasNodes.Add(atlas, new(animatedSprite, spriteSheet));
        }
        private void UpdateDrawPosition()
        {
            _drawPosition.X = (float)Math.Floor(_position.X + _shaderDrawOffset.X);
            _drawPosition.Y = (float)Math.Floor(_position.Y + _shaderDrawOffset.Y);
        }
        private void UpdateScaleVector()
        {
            _scaleVector.X = _scale;
            _scaleVector.Y = _scale;
        }
        private void UpdateAnimationController()
        {
            _animationController = _atlasNodes[_animationAtlases[Animation]].AnimatedSprite.SetAnimation(_animationNames[Animation]);
        }
        private void UpdateAnimatedSpriteColor()
        {
            _atlasNodes[_animationAtlases[Animation]].AnimatedSprite.Color = Color * Visibility;
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
        public bool ShowBase = true;
        public readonly Bag<Shaders.Feature> ShaderFeatures = new();
        public int CurrentFrame => _animationController.CurrentFrame;
        public Texture2D Texture => _atlasNodes[_animationAtlases[Animation]].SpriteSheet.TextureAtlas.Texture;
        public Rectangle Region => _atlasNodes[_animationAtlases[Animation]].SpriteSheet.TextureAtlas[CurrentFrame].Bounds;
        public Animater()
        {
            _attacher = new(this);
            UpdateAtlasAnimatedSprites();
            UpdateDrawPosition();
            UpdateScaleVector();
            UpdateAnimationController();
            UpdateAnimatedSpriteColor();
        }
        public void Play(Animations animation)
        {
            var differentAnimation = Animation != animation;
            _animation = animation;

            if (differentAnimation && !_atlasNodes.ContainsKey(_animationAtlases[Animation]))
                UpdateAtlasAnimatedSprites();

            UpdateAnimationController();
            UpdateAnimatedSpriteColor();
            _animationController.Play();
        }
        public void Update()
        {
            {
                _shaderDrawOffset = Vector2.Zero;
                var updateDrawPosition = false;
                for (var i = 0; i < ShaderFeatures.Count; i++)
                {
                    var feature = ShaderFeatures[i];
                    updateDrawPosition |= feature.UpdateDrawOffset(ref _shaderDrawOffset);
                }
                if (updateDrawPosition)
                    UpdateDrawPosition();
            }

            for (var i = 0; i < ShaderFeatures.Count; i++)
                ShaderFeatures[i].Update();
            _atlasNodes[_animationAtlases[Animation]].AnimatedSprite.Update(Globals.GameTime);
        }
        public void Draw()
        {
            Globals.SpriteBatch.Draw(
                sprite: _atlasNodes[_animationAtlases[Animation]].AnimatedSprite, 
                position: _drawPosition, 
                rotation: Rotation, 
                scale: _scaleVector);
        }
    }
}
