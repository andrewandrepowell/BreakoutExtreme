﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static BreakoutExtreme.Components.Animater;

namespace BreakoutExtreme.Components
{
    public class Animater
    {
        private static readonly ReadOnlyDictionary<Atlases, string> _atlasAssetNames = new(new Dictionary<Atlases, string>
        {
            { Atlases.Ball, "animations/ball_0" },
            { Atlases.Paddle, "animations/paddle_0" },
            { Atlases.BrickLarge, "animations/brick_2" },
        });
        private static readonly ReadOnlyDictionary<Atlases, Size> _atlasRegionSizes = new(new Dictionary<Atlases, Size>
        {
            { Atlases.Ball, new Size(80, 80) },
            { Atlases.Paddle, new Size(128, 80) },
            { Atlases.BrickLarge, new Size(112, 80) },
        });
        private static readonly ReadOnlyDictionary<Animations, string> _animationNames = new(new Dictionary<Animations, string>
        {
            { Animations.Ball, "ball_0" },
            { Animations.Paddle, "paddle_0" },
            { Animations.BrickLarge, "brick_0" },
        });
        private static readonly ReadOnlyDictionary<Atlases, Action<SpriteSheet>> _atlasConfigureAnimations = new(new Dictionary<Atlases, Action<SpriteSheet>>
        {
            {
                Atlases.Ball,
                delegate(SpriteSheet spriteSheet)
                {
                    spriteSheet.DefineAnimation(_animationNames[Animations.Ball], builder => builder
                        .AddFrame(0, TimeSpan.Zero));
                }
            },
            {
                Atlases.Paddle,
                delegate(SpriteSheet spriteSheet)
                {
                    spriteSheet.DefineAnimation(_animationNames[Animations.Paddle], builder => builder
                        .AddFrame(0, TimeSpan.Zero));
                }
            },
            {
                Atlases.BrickLarge,
                delegate(SpriteSheet spriteSheet)
                {
                    spriteSheet.DefineAnimation(_animationNames[Animations.BrickLarge], builder => builder
                        .AddFrame(0, TimeSpan.Zero));
                }
            },
        });
        private static readonly ReadOnlyDictionary<Animations, Atlases> _animationAtlases = new(new Dictionary<Animations, Atlases>()
        {
            { Animations.Ball, Atlases.Ball },
            { Animations.Paddle, Atlases.Paddle },
            { Animations.BrickLarge, Atlases.BrickLarge },
        });
        private readonly Dictionary<Atlases, AnimatedSprite> _atlasAnimatedSprites = [];
        private Animations _animation = Animations.Ball;
        private Vector2 _position, _drawPosition;
        private float _scale = 1;
        private Vector2 _scaleVector;
        private IAnimationController _animationController;
        private void UpdateAtlasAnimatedSprites()
        {
            var atlas = _animationAtlases[Animation];
            var atlasAssetName = _atlasAssetNames[atlas];
            var atlasTexture = Globals.ContentManager.Load<Texture2D>(atlasAssetName);
            var atlasRegionSize = _atlasRegionSizes[atlas];
            var atlasObject = Texture2DAtlas.Create(atlasAssetName, atlasTexture, atlasRegionSize.Width, atlasRegionSize.Height);
            var spriteSheet = new SpriteSheet(atlasAssetName, atlasObject);
            _atlasConfigureAnimations[atlas](spriteSheet);
            var animatedSprite = new AnimatedSprite(spriteSheet, _animationNames[Animation]);
            animatedSprite.OriginNormalized = new Vector2(.5f, .5f);
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
        public static void Load()
        {
            var animater = new Animater();
            var animations = Enum.GetValues<Animations>();
            foreach (ref var animation in animations.AsSpan())
            {
                animater.Play(animation);
            }
        }
        public enum Atlases
        {
            Ball,
            Paddle,
            BrickLarge
        }
        public enum Animations
        {
            Ball,
            Paddle,
            BrickLarge
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
        public bool IsAnimating => _animationController.IsAnimating;
        public int CurrentFrame => _animationController.CurrentFrame;
        public Animater()
        {
            UpdateAtlasAnimatedSprites();
            UpdateDrawPosition();
            UpdateScaleVector();
            UpdateAnimationController();
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
            var k = _atlasAnimatedSprites[_animationAtlases[Animation]];
            
            Globals.SpriteBatch.Draw(
                sprite: _atlasAnimatedSprites[_animationAtlases[Animation]], 
                position: _drawPosition, 
                rotation: Rotation, 
                scale: _scaleVector);
        }
    }
}
